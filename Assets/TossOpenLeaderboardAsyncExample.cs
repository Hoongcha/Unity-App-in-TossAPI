// Assets/Scripts/GameExample.cs

using System;
using Hoongcha_TossAPI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TossOpenLeaderboardAsyncExample : MonoBehaviour
{
    [SerializeField] private Button debug_BTN;
    [SerializeField] private TMP_Text debug_Text;
    private TossLeaderBoardJsBridge TossOpenLeaderboardJSBridge;

    void Start()
    {
        debug_BTN.onClick.AddListener(async delegate
        {
            TossOpenLeaderboardJSBridge = transform.AddComponent<TossLeaderBoardJsBridge>();
            // 리더보드 오픈 요청
            try
            {
                debug_Text.text = await TossOpenLeaderboardJSBridge.QueryOpenLeaderBoardAsync();
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
                if (e.Message.Contains("지원하지 않는 앱 버전"))
                {
            
                }else
                {
                    // 잡다한 에러 혹은 알 수 없는 에러
                }
            }
        });
    }

    private void OnDestroy()
    {
        debug_BTN.onClick.RemoveAllListeners();
    }

}