// Assets/Scripts/GameExample.cs

using System;
using Hoongcha_TossAPI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TossSubmitLeaderBoardScoreExample : MonoBehaviour
{
    [SerializeField] private Button debug_BTN;
    [SerializeField] private TMP_Text debug_Text;
    private TossLeaderBoardJsBridge tossLeaderBoardJsBridge;

    #region 생명주기

    void Start()
    {
        debug_BTN.onClick.AddListener(delegate
        {
            tossLeaderBoardJsBridge = transform.AddComponent<TossLeaderBoardJsBridge>();
            // 리더보드에 점수 보내기 요청
            tossLeaderBoardJsBridge.QuerySubmitLeaderBoardScore(OnOpenSuccess,OnError,"0.1");
        });
    }

    private void OnDestroy()
    {
        debug_BTN.onClick.RemoveAllListeners();
    }

    #endregion

    #region 콜백
    /// <summary>
    /// 리더 보드에 점수 보내기에 성공하면 콜백됩니다.
    /// </summary>
    private void OnOpenSuccess(string msg) { }
    
    /// <summary>
    /// 에러를 받아 처리합니다.
    /// </summary>
    /// <param name="error"></param>
    private void OnError(string error)
    {
        switch (error)
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
            case "JSON PARSE ERROR":
                // 도착한 데이터의 JSON이 잘못되었습니다.(아마 플러그인 개발자 잘못일듯)
                break;
            default:
                // 잡다한 혹은 알 수 없는 에러
                break;
        }
        debug_Text.text = error;
    }
    #endregion
}