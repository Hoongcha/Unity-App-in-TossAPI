using System;
using TMPro;
using UnityEngine;

// 무조건 로드 -> 출력 해야합니다.
// 광고 하나가 끝날때 마다 해당 사이클을 반복해야합니다.
// 로드 -> 출력 -> 로드 -> 출력 -> ........
public class TossLoadAppInAdMobExample : MonoBehaviour
{
    [SerializeField]
    private TMP_Text debugText;
    [SerializeField]
    private TossAdsConfig tossAdsConfig;
    [SerializeField]
    private TossAdMobJSBridge tossAdMob;
    
    public void LoadAppInTossAdMob()
    {
        tossAdMob.LoadAppsInTossAdMob(
            tossAdsConfig.AD_GROUP_ID,
            OnSuccess,OnError );
    }
    private void OnSuccess(string msg)
    {
        debugText.text = msg;
    }
    
    void OnError(string msg)
    {
        if (msg.Contains("AdMob Not Supported"))
        {
            // 현재 앱이 애드몹을 지원하지 않음 ( 설정 확인 요망 )
        }
        debugText.text = msg;
    }
}