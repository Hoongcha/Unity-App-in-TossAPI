using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using Hoongcha_TossAPI;
using UnityEngine;


/// <summary>
/// AdMob 광고 이벤트 타입을 나타냅니다.
/// </summary>
public enum AdMobEventType
{
    /// <summary> 광고 보여주기 요청 완료 </summary>
    REQUESTED,
    /// <summary> 광고 클릭 </summary>
    CLICKED,
    /// <summary> 광고 닫힘  </summary>
    DISMISSED,
    /// <summary>  광고 노출 </summary>
    IMPRESSION,
    /// <summary> 광고 컨텐츠 보여졌음 </summary>
    SHOW,
    /// <summary>
    /// 사용자가 광고 시청을 완료하고 보상을 획득했습니다.
    /// (보상형 광고 전용)
    /// </summary>
    USER_EARNED_REWARD,
    /// <summary> 광고 보여주기 실패 </summary>
    FAILED_TO_SHOW,
    ERROR
}



/// <summary>
/// TossShowAD JS 브리지에서 유니티로 전달하는 응답 데이터
/// </summary>
[Serializable]
public class JSBridgeAdResponse
{
    public string status;
    public string message;
    /// <summary>
    /// 보상형 광고일 때 제공되는 보상 타입입니다.
    /// userEarnedReward 이벤트에서만 제공됩니다.
    /// </summary>
    public string unitType;
    /// <summary>
    /// 보상형 광고일 때 제공되는 보상 개수입니다.
    /// userEarnedReward 이벤트에서만 제공됩니다.
    /// </summary>
    public int unitAmount;
}

/// <summary>
/// 토스 애드몹 브릿지 <br></br>
/// 비동기 불가능 <br></br>
/// 로드는 될것도 같은데 예제가 콜백이라 일단 둘다 콜백형으로 만듦 
/// </summary>
public class TossAdMobJSBridge : MonoBehaviour
{
    #region loadAppsInTossAdMob

    private Action<string> onLoadAppsInTossAdMobSuccess;
    private Action<string> onLoadAppsInTossAdMobError;
    
    #if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void RequestLoadAD(string objName, string method,string AD_GROUP_ID);
    #endif  

    /// <summary>
    /// 광고를 로드합니다.
    /// </summary>
    /// <param name="AD_GROUP_ID">토스에서 발급한 AD_GROUP_ID</param>
    /// <param name="onSuccess">성공 했을때 호출</param>
    /// <param name="onError">에러날때 호출</param>
    public void LoadAppsInTossAdMob(string AD_GROUP_ID,Action<string> onSuccess, Action<string> onError)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
            this.onLoadAppsInTossAdMobSuccess = onSuccess;
            this.onLoadAppsInTossAdMobError = onError;
            RequestLoadAD(gameObject.name, "OnLoadAppsInTossAdMob",AD_GROUP_ID);
    #else
        Debug.LogWarning("showAppsInTossAdMob은 WebGL 빌드에서만 동작합니다.");
        onError("WebGL 환경이 아닙니다.");
    #endif
    }
    
    /// <summary> JS가 호출합니다. 사용하지 마세요. </summary>
    public void OnLoadAppsInTossAdMob(string jsonResponse)
    {
        try
        {
            JSBridgeResponse response =
                JsonUtility.FromJson<JSBridgeResponse>(jsonResponse);

            if (response.status == "success")
            {
                onLoadAppsInTossAdMobSuccess(response.message);
            }
            else
            {
                onLoadAppsInTossAdMobError(response.message);
            }
        }
        catch (Exception e)
        {
            onLoadAppsInTossAdMobError($"{e.Message}");
        }
    }

    #endregion
    
    #region showAppsInTossAdMob

    private Action<(AdMobEventType eventType, JSBridgeAdResponse adResponse)> onShowAppsInTossAdMobSuccess;
    private Action<string> onShowAppsInTossAdMobError;
    
    #if !UNITY_EDITOR && UNITY_WEBGL
    [DllImport("__Internal")]
    private static extern void RequesShowAD(string objName, string method,string AD_GROUP_ID);
    #endif  

    /// <summary>
    /// 광고를 보여줍니다.
    /// </summary>
    /// <param name="AD_GROUP_ID">토스에서 발급한 AD_GROUP_ID</param>
    /// <param name="onSuccess">성공 했을때 호출</param>
    /// <param name="onError">에러날때 호출</param>
    public void ShowAppsInTossAdMob(string AD_GROUP_ID,
                                    Action<(AdMobEventType eventType, JSBridgeAdResponse adResponse)> onSuccess,
                                    Action<string> onError)
    {
    #if UNITY_WEBGL && !UNITY_EDITOR
            this.onShowAppsInTossAdMobSuccess = onSuccess;
            this.onShowAppsInTossAdMobError = onError;
            RequesShowAD(gameObject.name, "OnShowAppsInTossAdMob",AD_GROUP_ID);
    #else
        Debug.LogWarning("showAppsInTossAdMob은 WebGL 빌드에서만 동작합니다.");
        onError("WebGL 환경이 아닙니다.");
    #endif
    }
    
    /// <summary> JS가 호출합니다. 절대 사용하지 마세요. </summary>
    public void OnShowAppsInTossAdMob(string jsonResponse)
    {
        try
        {
            JSBridgeAdResponse response =
                JsonUtility.FromJson<JSBridgeAdResponse>(jsonResponse);
            onShowAppsInTossAdMobSuccess
                ((ParseAdMobEventType(response.status),response));
        }
        catch (Exception e)
        {
            onShowAppsInTossAdMobError($"{e.Message}");
        }
    }

    
    private AdMobEventType ParseAdMobEventType(string status)
    {
        switch (status?.ToLowerInvariant())
        {
            case "requested":
                return AdMobEventType.REQUESTED;
            case "clicked":
                return AdMobEventType.CLICKED;
            case "dismissed":
                return AdMobEventType.DISMISSED;
            case "impression":
                return AdMobEventType.IMPRESSION;
            case "show":
                return AdMobEventType.SHOW;
            case "userearnedreward":
                return AdMobEventType.USER_EARNED_REWARD;
            case "failedtoshow":
                return AdMobEventType.FAILED_TO_SHOW;
            default:
                return AdMobEventType.ERROR;
        }
    }
    
    #endregion
}