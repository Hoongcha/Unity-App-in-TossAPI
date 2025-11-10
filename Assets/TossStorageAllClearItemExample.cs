using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TossStorageAllClearItemExample : MonoBehaviour
{
    [SerializeField] private TMP_Text debug_Text;
    private TossStorageJSBridge tossStorageJsBridge;

    void Start()
    {
        tossStorageJsBridge = transform.AddComponent<TossStorageJSBridge>();
        
        // Storage 전체 초기화 (콜백)
        tossStorageJsBridge.AllClearItem(OnClearSuccess, OnError);
    }

    /// <summary>
    /// 초기화 성공 시 호출됩니다.
    /// </summary>
    /// <param name="message">성공 메시지</param>
    private void OnClearSuccess(string message)
    {
        debug_Text.text = $"초기화 성공: {message}";
    }
    
    /// <summary>
    /// 에러를 받아 처리합니다.
    /// </summary>
    /// <param name="error">에러 메시지</param>
    private void OnError(string error)
    {
        if (error.Contains("지원하지 않는 앱 버전"))
        {
            
        }else
        {
            // 잡다한 에러 혹은 알 수 없는 에러
            debug_Text.text = $"에러: {error}";
        }
    }
}