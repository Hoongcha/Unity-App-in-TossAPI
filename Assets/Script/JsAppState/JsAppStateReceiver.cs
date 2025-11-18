using System;
using Hoongcha_TossAPI;
using UnityEngine;

public class JsAppStateReceiver : AppStateJSBridge
{
    /// <summary> JS 브릿지 </summary>
    private AppStateJSBridge appStateJsBridge;
    /// <summary> JS에서 전달한 앱 상태를 알려주는 Action</summary>
    private Action<AppState> onAppStateChanged;

    void Start()
    {
        DontDestroyOnLoad(gameObject);
        
        // 콜백 설정
        appStateJsBridge = this;
        appStateJsBridge.RegisterAppState(OnAppState);
    }

    /// <summary> 앱 상태 변경 시 호출될 콜백을 등록합니다.</summary>
    /// <param name="listener">등록할 콜백 메서드</param>
    public void AddListener(Action<AppState> listener)
    {
        onAppStateChanged += listener;
    }

    /// <summary> 등록된 콜백을 제거합니다.</summary>
    /// <param name="listener">제거할 콜백 메서드</param>
    public void RemoveListener(Action<AppState> listener)
    {
        onAppStateChanged -= listener;
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
                onAppStateChanged?.Invoke(appState);
                break;
            case AppState.FOREGROUND:
                // 포그라운드
                onAppStateChanged?.Invoke(appState);
                break;
        }
    }
    
}