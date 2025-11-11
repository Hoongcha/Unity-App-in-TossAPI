using System;
using System.Runtime.InteropServices;
using UnityEngine;


public enum AppState
{
    BACKGROUND,
    FOREGROUND
}

/// <summary>
/// React(웹)에서 브라우저 상태 변화를 콜백으로 받는 브리지
/// </summary>
public class AppStateJSBridge : MonoBehaviour
{
    private Action<AppState> onAppStateChange;
#if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void AppStateSubscribe(string objName, string method);
#endif
    
    // Unity 초기화 시 JS에 구독 요청
    private void Start()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        AppStateSubscribe(gameObject.name, nameof(OnAppState));
#endif
    }

    public void RegisterAppState(Action<AppState> onAppStateChange)
    {
        this.onAppStateChange = onAppStateChange;
    }

    // JS에서 SendMessage로 호출됨
    public void OnAppState(string json)
    {
        var data = JsonUtility.FromJson<AppStatePayload>(json);

        if (data.state == "background")
        {
            Debug.Log("현재 상태: 백그라운드");
            onAppStateChange(AppState.BACKGROUND);
        }
        else if (data.state == "foreground")
        {
            Debug.Log("현재 상태: 포그라운드");
            onAppStateChange(AppState.FOREGROUND);
        }
    }

    [System.Serializable]
    private class AppStatePayload
    {
        public string state;
    }
}