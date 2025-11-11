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
    private void Awake()
    {
#if !UNITY_EDITOR && UNITY_WEBGL
        AppStateSubscribe(gameObject.name, nameof(OnAppState));
#endif
    }

    public void RegisterAppState(Action<AppState> onAppStateChange)
    {
        this.onAppStateChange = onAppStateChange;
    }

    /// <summary>
    /// JS에서 SendMessage(goName, methodName, json)으로 호출되는 수신 메서드.
    /// .jslib의 send(state, evt)에서 내려온 JSON을 AppState로 매핑하여 콜백에 전달합니다.
    /// </summary>
    /// <param name="json">
    /// { "state":"visible"|"hidden", "eventType":"visibilitychange|pagehide|pageshow|blur|focus|init|freeze", "hidden":true|false, "ts":number }
    /// </param>
    public void OnAppState(string json)
    {
        try
        {
            var data = JsonUtility.FromJson<VisibilityPayload>(json);
            bool isHidden = data.hidden || string.Equals(data.state, "hidden", StringComparison.OrdinalIgnoreCase);

            var mapped = isHidden ? AppState.BACKGROUND : AppState.FOREGROUND;

            // 디버그 로그(원하면 주석 해제)
            // Debug.Log($"[AppStateJSBridge] state={data.state}, event={data.eventType}, hidden={data.hidden}, ts={data.ts} → {mapped}");

            onAppStateChange?.Invoke(mapped);
        }
        catch (Exception e)
        {
            Debug.LogError($"[AppStateJSBridge] JSON 파싱 실패: {e.Message}\nraw={json}");
        }
    }

    /// <summary>
    /// JS에서 전달되는 가시성 이벤트 페이로드
    /// </summary>
    [Serializable]
    private class VisibilityPayload
    {
        /// <summary>
        /// "visible" 또는 "hidden"
        /// </summary>
        public string state;

        /// <summary>
        /// "visibilitychange" | "pagehide" | "pageshow" | "blur" | "focus" | "init" | "freeze"
        /// </summary>
        public string eventType;

        /// <summary>
        /// true면 hidden 상태
        /// </summary>
        public bool hidden;

        /// <summary>
        /// JS 측 타임스탬프(ms)
        /// </summary>
        public long ts;
    }
}