// Assets/Scripts/GameExample.cs

using System;
using Hoongcha_TossAPI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TossSubmitLeaderBoardScoreAsyncExample : MonoBehaviour
{
    [SerializeField] private Button debug_BTN;
    [SerializeField] private TMP_Text debug_Text;
    private TossLeaderBoardJsBridge tossLeaderBoardJsBridge;

    #region 생명주기

    void Start()
    {
        debug_BTN.onClick.AddListener(async delegate
        {
            tossLeaderBoardJsBridge = transform.AddComponent<TossLeaderBoardJsBridge>();
            // 리더보드에 점수 보내기 요청
            try
            {
                debug_Text.text = await tossLeaderBoardJsBridge.QuerySubmitLeaderBoardScoreAsync("1");
                // 아무문제도 없으면 성공!!
            }
            catch (NotSupportedException e)
            {
                // 호환 되지않는 OS
            }
            catch (InvalidOperationException e)
            {
                if (e.Message.Contains("이미 진행 중"))
                {
                    // 이미 요청 했었었음.
                }
            }
            catch (OperationCanceledException e)
            {
                // 요청이 도중에 취소되었습니다.
            }
            catch (Exception e)
            {
                switch (e.Message)
                {
                    case "LEADERBOARD_NOT_FOUND":
                        // 리더보드를 찾지 못했습니다.
                        break;
                    case "PROFILE_NOT_FOUND":
                        // 프로파일을 찾지 못했습니다.
                        break;
                    case "UNPARSABLE_SCORE":
                        // 점수를 Parse할 수 없습니다.
                        break;
                    default:
                        if (e.Message.Contains("지원하지 않는 앱 버전"))
                        {
                            
                        }else if (e.Message.Contains("JSON PARSE ERROR"))
                        {
                            // 도착한 데이터의 JSON이 잘못되었습니다.(아마 플러그인 개발자 잘못일듯)
                        }
                        else
                        {
                            // 잡다한 에러 혹은 알 수 없는 에러
                        }
                        break;
                }
            }
        });
    }

    private void OnDestroy()
    {
        debug_BTN.onClick.RemoveAllListeners();
    }

    #endregion
}