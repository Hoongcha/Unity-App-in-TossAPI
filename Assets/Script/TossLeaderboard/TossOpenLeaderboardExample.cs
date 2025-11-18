// Assets/Scripts/GameExample.cs

using System;
using Hoongcha_TossAPI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TossOpenLeaderboardExample : MonoBehaviour
{
    [SerializeField] private Button debug_BTN;
    [SerializeField] private TMP_Text debug_Text;
    private TossLeaderBoardJsBridge TossOpenLeaderboardJSBridge;

    #region 생명주기

    void Start()
    {
        debug_BTN.onClick.AddListener(delegate
        {
            TossOpenLeaderboardJSBridge = transform.AddComponent<TossLeaderBoardJsBridge>();
            // 리더보드 오픈 요청
            TossOpenLeaderboardJSBridge.OpenLeaderboard(OnOpenSuccess,OnError);
        });
    }

    private void OnDestroy()
    {
        debug_BTN.onClick.RemoveAllListeners();
    }

    #endregion

    #region 콜백
    /// <summary>
    /// 리더보드 오픈에 성공했을때 콜백됩니다.
    /// </summary>
    private void OnOpenSuccess(string msg) { }
    
    /// <summary>
    /// 에러를 받아 처리합니다.
    /// </summary>
    /// <param name="error"></param>
    private void OnError(string error)
    {
        if (error.Contains("지원하지 않는 앱 버전"))
        {
            
        }else
        {
            // 잡다한 에러 혹은 알 수 없는 에러
        }
        debug_Text.text = error;
    }
    #endregion
}