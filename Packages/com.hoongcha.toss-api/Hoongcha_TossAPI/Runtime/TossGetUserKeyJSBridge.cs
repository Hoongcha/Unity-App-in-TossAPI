using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Hoongcha_TossAPI
{
    /// <summary>
    /// Toss API getUserKeyForGame 실행시키는 JS브릿지입니다. <br></br>
    /// 해당 함수는 JS에선 비동기 함수입니다. <br></br>
    /// 로그인되어있으면 키값을/아니라면 로그인을 수행합니다. <br></br>
    /// </summary>
    public class TossGetUserKeyJSBridge : MonoBehaviour
    {
        #region GetUserKey
        
        [DllImport("__Internal")]
        private static extern void RequestUserKey(string gameObjectName, string callbackMethod);

        private Action<string> onGetUserKeyReceived;
        private Action<string> onGetUserKeyError;

        
        /// <summary>
        /// 유저키를 얻어오거나 로그인을 시행합니다.
        /// </summary>
        /// <param name="onUserKeyReceived">성공 했을때 호출</param>
        /// <param name="onError">에러날때 호출</param>
        public void GetUserKey(Action<string> onUserKeyReceived, Action<string> onError)
        {
        #if UNITY_WEBGL && !UNITY_EDITOR
            this.onGetUserKeyReceived = onUserKeyReceived;
            this.onGetUserKeyError = onError;
            RequestUserKey(gameObject.name, "OnUserKeyCallback");
        #else
            Debug.LogWarning("getUserKeyForGame은 WebGL 빌드에서만 동작합니다.");
            onGetUserKeyError("WebGL 환경이 아닙니다.");
        #endif
        }

        public void OnUserKeyCallback(string jsonResponse)
        {
            try
            {
                JSBridgeResponse response =
                    JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

                if (response.status == "success")
                {
                    onGetUserKeyReceived(response.hash);
                }
                else
                {
                    onGetUserKeyError(response.message);
                }
            }
            catch (Exception e)
            {
                onGetUserKeyError($"{e.Message}");
            }
        }

        #endregion

        #region GetUserKeyAsync


        /// <summary>
        /// 현재 진행 중인 UserKey 요청의 완료를 통지하는 TCS
        /// </summary>
        private TaskCompletionSource<string> userKeyTCS;

        /// <summary>
        /// 호출자가 넘긴 취소 토큰 등록 핸들(취소 시 해제)
        /// </summary>
        private CancellationTokenRegistration userKeyCtr;


        /// <summary>
        /// UserKey를 요청합니다. (Task 기반 / 타임아웃 미사용)
        /// </summary>
        /// <param name="cancellationToken">캔슬 토큰</param>
        /// <returns>성공 시 JS 측에서 넘어온 user key(hash 우선, 없으면 message)</returns>
        /// <exception cref="InvalidOperationException">이미 진행 중인 요청이 있을 때</exception>
        /// <exception cref="OperationCanceledException">취소 시</exception>
        /// <exception cref="NotSupportedException">WebGL 환경이 아닐 때</exception>
        public Task<string> GetUserKeyAsync(CancellationToken cancellationToken = default)
        {
        #if UNITY_WEBGL && !UNITY_EDITOR
            // 동시 호출 방지: 이전 요청이 완료되지 않았다면 예외
            if (userKeyTCS != null && !userKeyTCS.Task.IsCompleted)
                throw new InvalidOperationException("이미 진행 중인 UserKey 요청이 있습니다.");

            // 콜백 스레드 안전성을 위해 RunContinuationsAsynchronously 사용
            userKeyTCS = new TaskCompletionSource<string>(TaskCreationOptions.RunContinuationsAsynchronously);

            // 취소 토큰이 트리거되면 Task 취소 및 정리
            if (cancellationToken.CanBeCanceled)
            {
                userKeyCtr = cancellationToken.Register(() =>
                {
                    userKeyTCS?.TrySetCanceled(cancellationToken);
                    CleanupUserKeyTaskState();
                });
            }

            // JS 쪽에 콜백 대상 메서드 이름을 넘겨 호출
            RequestUserKey(gameObject.name, "OnUserKeyCallbackAsync");

            return userKeyTCS.Task;
        #else
            return Task.FromException<string>(new NotSupportedException("getUserKeyForGame은 WebGL 빌드에서만 동작합니다."));
        #endif
        }

        public void OnUserKeyCallbackAsync(string jsonResponse)
        {
            Debug.Log($"Received from React (GetUserKey): {jsonResponse}");

            try
            {
                var response = JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

                if (userKeyTCS == null)
                {
                    // 대기 중인 요청이 없을 때 들어온 콜백 — 로그만 남김
                    Debug.LogWarning("대기 중인 GetUserKey 요청이 없는데 콜백이 도착했습니다.");
                    return;
                }

                if (string.Equals(response.status, "success", StringComparison.OrdinalIgnoreCase))
                {
                    userKeyTCS.TrySetResult(response.hash);
                }
                else
                {
                    userKeyTCS.TrySetException(new Exception(response.message));
                }
            }
            catch (Exception e)
            {
                userKeyTCS?.TrySetException(new Exception($"JSON PARSE ERROR\n{e.Message}"));
            }
            finally
            {
                CleanupUserKeyTaskState();
            }
        }

        /// <summary>
        /// 내부 상태/리소스 정리 (UserKey용)
        /// </summary>
        private void CleanupUserKeyTaskState()
        {
            try { userKeyCtr.Dispose(); } catch { /* 무시 */ }
            userKeyCtr = default;
            userKeyTCS = null;
        }

        #endregion
    }
}