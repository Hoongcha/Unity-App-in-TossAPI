using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;

namespace Hoongcha_TossAPI
{
    /// <summary>
    /// Toss API submitGameCenterLeaderBoardScore을 실행시키는 JS브릿지입니다. <br></br>
    /// 해당 함수는 JS에선 비동기 함수입니다. 
    /// </summary>
    public class TossLeaderBoardJsBridge : MonoBehaviour
    {
        #region SubmitLeaderBoardScore
        #if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void RequestSubmitLeaderBoardScore(string gameObjectName, string callbackMethod,string scoreString);
        #endif
        private Action<string> onSubmitLeaderBoardSuccess;
        private Action<string> onSubmitLeaderBoardError;

        /// <summary>
        /// 리더보드에 점수를 보냅니다.
        /// </summary>
        /// <param name="onSuccess">성공 했을때 호출</param>
        /// <param name="onError">에러날때 호출</param>
        /// <param name="score">점수 <br></br> 링크 참조 : 
        /// https://developers-apps-in-toss.toss.im/bedrock/reference/framework/%EA%B2%8C%EC%9E%84/submitGameCenterLeaderBoardScore.html#%ED%8C%8C%EB%9D%BC%EB%AF%B8%ED%84%B0
        /// </param>
        public void QuerySubmitLeaderBoardScore(Action<string> onSuccess, Action<string> onError,string score)
        {
        #if UNITY_WEBGL && !UNITY_EDITOR
            onSubmitLeaderBoardSuccess = onSuccess;
            onSubmitLeaderBoardError = onError;
            RequestSubmitLeaderBoardScore(gameObject.name, "ReceiveSubmitLeaderBoardScore",score);
        #else
            Debug.LogWarning("submitGameCenterLeaderBoardScore은 WebGL 빌드에서만 동작합니다.");
            onError("WebGL 환경이 아닙니다.");
        #endif
        }

        public void ReceiveSubmitLeaderBoardScore(string jsonResponse)
        {
            Debug.Log($"Received from React: {jsonResponse}");

            try
            {
                JSBridgeResponse response =
                    JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);
                if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
                {
                    onSubmitLeaderBoardSuccess(response.message);
                }
                else
                {
                    onSubmitLeaderBoardError(response.message);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"JSON 파싱 오류: {e.Message}");
                onSubmitLeaderBoardError($"{e.Message}");
            }
        }
        #endregion
        
        #region SubmitLeaderBoardScoreAsync
        /// <summary>
        /// 현재 진행 중인 제출 요청의 완료를 통지하는 TCS
        /// </summary>
        private TaskCompletionSource<string> submitLeaderBoardScoreTCS;

        /// <summary>
        /// 호출자가 넘긴 취소 토큰 등록 핸들(취소 시 해제)
        /// </summary>
        private CancellationTokenRegistration submitCtr;

        /// <summary>
        /// 리더보드에 점수를 보냅니다. (Task 기반 / 타임아웃 미사용)
        /// </summary>
        /// <param name="score">
        /// 점수 문자열(플랫폼 스펙 요구에 맞춰 문자열로 전달)
        /// <br></br> 링크 참조 :
        /// https://developers-apps-in-toss.toss.im/bedrock/reference/framework/%EA%B2%8C%EC%9E%84/submitGameCenterLeaderBoardScore.html#%ED%8C%8C%EB%9D%BC%EB%AF%B8%ED%84%B0
        /// </param>
        /// <param name="cancellationToken">캔슬 토큰</param>
        /// <returns>성공 시 JS 측에서 넘어온 message 문자열</returns>
        /// <exception cref="InvalidOperationException">이미 진행 중인 요청이 있을 때</exception>
        /// <exception cref="OperationCanceledException">취소 시</exception>
        /// <exception cref="NotSupportedException">WebGL 환경이 아닐 때</exception>
        public Task<string> QuerySubmitLeaderBoardScoreAsync(string score, CancellationToken cancellationToken = default)
        {
    #if UNITY_WEBGL && !UNITY_EDITOR
            // 동시 호출 방지: 이전 요청이 완료되지 않았다면 예외
            if (submitLeaderBoardScoreTCS != null && !submitLeaderBoardScoreTCS.Task.IsCompleted)
                throw new InvalidOperationException("이미 진행 중인 리더보드 점수 제출 요청이 있습니다.");

            // 콜백 스레드 안전성을 위해 RunContinuationsAsynchronously 사용
            submitLeaderBoardScoreTCS = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            // 취소 토큰이 트리거되면 Task 취소 및 정리
            if (cancellationToken.CanBeCanceled)
            {
                submitCtr = cancellationToken.Register(() =>
                {
                    submitLeaderBoardScoreTCS?.TrySetCanceled(cancellationToken);
                    CleanupSubmitTaskState();
                });
            }

            // JS 쪽에 콜백 대상 메서드 이름을 넘겨 호출
            RequestSubmitLeaderBoardScore(gameObject.name, "ReceiveSubmitLeaderBoardScoreAsync", score);

            return submitLeaderBoardScoreTCS.Task;
    #else
            return Task.FromException<string>(new NotSupportedException("submitGameCenterLeaderBoardScore은 WebGL 빌드에서만 동작합니다."));
    #endif
        }

        public void ReceiveSubmitLeaderBoardScoreAsync(string jsonResponse)
        {
            try
            {
                var response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

                if (submitLeaderBoardScoreTCS == null)
                {
                    Debug.LogWarning("SubmitLeaderBoardScore 요청이 없는데 콜백이 도착했습니다.");
                    return;
                }
                if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
                {
                    submitLeaderBoardScoreTCS.TrySetResult(response.message);
                }
                else
                {
                    submitLeaderBoardScoreTCS.TrySetException(new Exception(response.message));
                }
            }
            catch (Exception e)
            {
                submitLeaderBoardScoreTCS?.TrySetException(new Exception($"JSON PARSE ERROR\n{e.Message}"));
            }
            finally
            {
                CleanupSubmitTaskState();
            }
        }

        /// <summary>
        /// 내부 상태/리소스 정리
        /// </summary>
        private void CleanupSubmitTaskState()
        {
            try { submitCtr.Dispose(); } catch { /* 무시 */ }
            submitCtr = default;
            submitLeaderBoardScoreTCS = null;
        }

        #endregion
        
        #region OpenLeaderboard
        #if UNITY_WEBGL
        [DllImport("__Internal")]
        private static extern void RequestOpenLeaderboard(string gameObjectName, string callbackMethod);
        #endif
        private Action<string> onOpenSuccess;
        private Action<string> onOpenError;
        
        
        /// <summary>
        /// 리더보드를 엽니다.
        /// </summary>
        /// <param name="onOpenSuccess">성공 했을때 호출</param>
        /// <param name="onError">에러날때 호출</param>

        public void OpenLeaderboard(Action<string> onOpenSuccess, Action<string> onError)
        {
        #if UNITY_WEBGL && !UNITY_EDITOR
            this.onOpenSuccess = onOpenSuccess;
            this.onOpenError = onError;
            RequestOpenLeaderboard(gameObject.name, "OnOpenLeaderBoardCallback");
        #else
            Debug.LogWarning("openGameCenterLeaderboard은 WebGL 빌드에서만 동작합니다.");
            onOpenError("WebGL 환경이 아닙니다.");
        #endif
        }

        public void OnOpenLeaderBoardCallback(string jsonResponse)
        {
            Debug.Log($"Received from React: {jsonResponse}");

            try
            {
                JSBridgeResponse response =
                    JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

                if (response.status == "success")
                {
                    onOpenSuccess(response.message);
                }
                else
                {
                    onOpenError(response.message);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"JSON 파싱 오류: {e.Message}");
                onOpenError($"{e.Message}");
            }
        }
        #endregion

        #region OpenLeaderboardAsync

         /// <summary>
        /// 현재 진행 중인 "리더보드 열기" 요청의 완료를 통지하는 TCS
        /// </summary>
        private TaskCompletionSource<string> openLeaderboardTCS;

        /// <summary>
        /// 호출자가 넘긴 취소 토큰 등록 핸들(취소 시 해제)
        /// </summary>
        private CancellationTokenRegistration openCtr;

        /// <summary>
        /// 리더보드를 엽니다. (Task 기반 / 타임아웃 미사용)
        /// </summary>
        /// <param name="cancellationToken">캔슬 토큰</param>
        /// <returns>
        /// 성공 시 JS 측에서 넘어온 <c>hash</c>(없으면 <c>message</c>) 문자열
        /// </returns>
        /// <exception cref="InvalidOperationException">이미 진행 중인 요청이 있을 때</exception>
        /// <exception cref="OperationCanceledException">취소 시</exception>
        /// <exception cref="NotSupportedException">WebGL 환경이 아닐 때</exception>
        public Task<string> QueryOpenLeaderBoardAsync(CancellationToken cancellationToken = default)
        {
        #if UNITY_WEBGL && !UNITY_EDITOR
            // 동시 호출 방지: 이전 요청이 완료되지 않았다면 예외
            if (openLeaderboardTCS != null && !openLeaderboardTCS.Task.IsCompleted)
                throw new InvalidOperationException("이미 진행 중인 리더보드 오픈 요청이 있습니다.");

            // 콜백 스레드 안전성을 위해 RunContinuationsAsynchronously 사용
            openLeaderboardTCS = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            // 취소 토큰이 트리거되면 Task 취소 및 정리
            if (cancellationToken.CanBeCanceled)
            {
                openCtr = cancellationToken.Register(() =>
                {
                    openLeaderboardTCS?.TrySetCanceled(cancellationToken);
                    CleanupOpenTaskState();
                });
            }

            // JS 쪽에 콜백 대상 메서드 이름을 넘겨 호출
            RequestOpenLeaderboard(gameObject.name, "OnOpenLeaderBoardCallbackAsync");

            return openLeaderboardTCS.Task;
        #else
            return Task.FromException<string>(new NotSupportedException("openGameCenterLeaderboard은 WebGL 빌드에서만 동작합니다."));
        #endif
        }

        public void OnOpenLeaderBoardCallbackAsync(string jsonResponse)
        {
            try
            {
                var response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

                if (openLeaderboardTCS == null)
                {
                    Debug.LogWarning("대기 중인 OpenLeaderboard 요청이 없는데 콜백이 도착했습니다.");
                    return;
                }

                if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
                {
                    var result = response.message;
                    openLeaderboardTCS.TrySetResult(result);
                }
                else
                {
                    openLeaderboardTCS.TrySetException(new Exception(response.message));
                }
            }
            catch (Exception e)
            {
                openLeaderboardTCS?.TrySetException(new Exception($"JSON PARSE ERROR\n{e.Message}"));
            }
            finally
            {
                CleanupOpenTaskState();
            }
        }

        /// <summary>
        /// 내부 상태/리소스 정리 (OpenLeaderboard용)
        /// </summary>
        private void CleanupOpenTaskState()
        {
            try { openCtr.Dispose(); } catch { /* 무시 */ }
            openCtr = default;
            openLeaderboardTCS = null;
        }

        #endregion
    }
}