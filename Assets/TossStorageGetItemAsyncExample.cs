
using Hoongcha_TossAPI;
using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TossStorageGetItemAsyncExample : MonoBehaviour
{
    [SerializeField] private TMP_Text debug_Text;
    private TossStorageJSBridge tossStorageJsBridge;

    async void Start()
    {
        tossStorageJsBridge = transform.AddComponent<TossStorageJSBridge>();
        
        // Storage에서 값 가져오기 (Async)
        try
        {
            string value = await tossStorageJsBridge.GetItemAsync("my-key");
            debug_Text.text = $"가져온 값: {value}";
            // 아무문제도 없으면 성공!!
        }
        catch (NotSupportedException e)
        {
            // 호환 되지않는 OS
            debug_Text.text = "WebGL 환경이 아닙니다.";
        }
        catch (InvalidOperationException e)
        {
            if (e.Message.Contains("이미 진행 중"))
            {
                // 이미 요청 했었었음.
                debug_Text.text = "이미 진행 중인 요청이 있습니다.";
            }
        }
        catch (OperationCanceledException e)
        {
            // 요청이 도중에 취소되었습니다.
            debug_Text.text = "요청이 취소되었습니다.";
        }
        catch (Exception e)
        {
            if (e.Message.Contains("지원하지 않는 앱 버전"))
            {
            
            }else
            {
                // 잡다한 에러 혹은 알 수 없는 에러
                debug_Text.text = $"에러: {e.Message}";
            }
        }
    }
}