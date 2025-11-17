// Assets/Scripts/GameExample.cs

using System;
using Hoongcha_TossAPI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TossGetUserKeyForGameAyncExample : MonoBehaviour
{
    [SerializeField] private TMP_Text debug_Text;
    private TossGetUserKeyJSBridge tossGetUserKeyJsBridge;

    async void Start()
    {
        tossGetUserKeyJsBridge = transform.AddComponent<TossGetUserKeyJSBridge>();
        // 사용자 키 요청
        try
        {
            debug_Text.text = await tossGetUserKeyJsBridge.GetUserKeyAsync();
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
                case "INVALID_CATEGORY":
                    // '게임 카테고리가 아닌 미니앱이에요.'
                    break;
                case "ERROR":
                    // 사용자 키 조회 중 오류가 발생했어요.
                    break;
                default:
                    if (e.Message.Contains("지원하지 않는 앱 버전"))
                    {
                    }
                    else
                    {
                        // 잡다한 에러 혹은 알 수 없는 에러
                    }
                    break;
            }
        }
    }

    /// <summary>
    /// 해시로 이루어진 키를 받습니다.
    /// </summary>
    /// <param name="hash">고유 해시값</param>
    private void OnGetKey(string hash)
    {
        debug_Text.text = hash;
    }
    
    /// <summary>
    /// 에러를 받아 처리합니다.
    /// </summary>
    /// <param name="error"></param>
    private void OnError(string error)
    {
        debug_Text.text = error;
    }
}