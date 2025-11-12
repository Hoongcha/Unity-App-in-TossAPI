using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class TossStorageRemoveItemExample : MonoBehaviour
{
    [SerializeField] private TMP_Text debug_Text;
    private TossStorageJSBridge tossStorageJsBridge;

    void Start()
    {
        tossStorageJsBridge = transform.AddComponent<TossStorageJSBridge>();
        
        // Storage에서 값 삭제 (콜백)
        tossStorageJsBridge.RemoveItem("my-key", OnRemoveSuccess, OnError);
    }

    /// <summary>
    /// 삭제 성공 시 호출됩니다.
    /// </summary>
    /// <param name="message">성공 메시지</param>
    private void OnRemoveSuccess(string message)
    {
        debug_Text.text = $"삭제 성공: {message}";
    }
    
    /// <summary>
    /// 에러를 받아 처리합니다.
    /// </summary>
    /// <param name="error">에러 메시지</param>
    private void OnError(string error)
    {
        debug_Text.text = $"에러: {error}";
    }
}