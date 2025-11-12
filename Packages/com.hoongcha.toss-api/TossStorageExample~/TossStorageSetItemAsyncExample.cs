using System;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TossStorageSetItemAsyncExample : MonoBehaviour
{
    [SerializeField] private TMP_Text debug_Text;
    private TossStorageJSBridge tossStorageJsBridge;

    async void Start()
    {
        tossStorageJsBridge = transform.AddComponent<TossStorageJSBridge>();
        
        // Storage에 값 저장 (Async)
        try
        {
            string result = await tossStorageJsBridge.SetItemAsync("my-key", "my-value");
            debug_Text.text = $"저장 성공: {result}";
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
            // 잡다한 에러 혹은 알 수 없는 에러
            debug_Text.text = $"에러 발생: {e.Message}";
        }
    }
}