using System;
using Hoongcha_TossAPI;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class JSAppStateChangeExample : MonoBehaviour
{
    [SerializeField] private TMP_Text debug_Text;
    private AppStateJSBridge appStateJsBridge;

    void Start()
    {
        appStateJsBridge = transform.AddComponent<AppStateJSBridge>();
        // 콜백 구독
        appStateJsBridge.RegisterAppState(OnAppState);
    }

    /// <summary>
    /// 앱 상태가 변경되면 호출됩니다.
    /// </summary>
    /// <param name="appState">앱 상태</param>
    private void OnAppState(AppState appState)
    {
        switch (appState)
        {
            case AppState.BACKGROUND:
                // 백그라운드
                break;
            case AppState.FOREGROUND:
                // 포그라운드
                break;
        }
    }
    
}