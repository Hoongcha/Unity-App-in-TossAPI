

using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TossStorageGetItemExample : MonoBehaviour
{
    [SerializeField] private TMP_Text debug_Text;
    private TossStorageJSBridge tossStorageJsBridge;

    void Start()
    {
        tossStorageJsBridge = transform.AddComponent<TossStorageJSBridge>();
        
        // Storage에서 값 가져오기 (콜백)
        tossStorageJsBridge.GetItem("my-key", OnGetSuccess, OnError);
    }

    /// <summary>
    /// 값 가져오기 성공 시 호출됩니다.
    /// </summary>
    /// <param name="value">저장된 값</param>
    private void OnGetSuccess(string value)
    {
        debug_Text.text = $"가져온 값: {value}";
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