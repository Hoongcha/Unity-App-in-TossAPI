// Assets/Scripts/GameExample.cs

using Hoongcha_TossAPI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class TossGetUserKeyForGameExample : MonoBehaviour
{
    [SerializeField] private TMP_Text debug_Text;
    private TossGetUserKeyJSBridge tossGetUserKeyJsBridge;

    void Start()
    {
        tossGetUserKeyJsBridge = transform.AddComponent<TossGetUserKeyJSBridge>();
        // 사용자 키 요청
        tossGetUserKeyJsBridge.GetUserKey(OnGetKey,OnError);
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