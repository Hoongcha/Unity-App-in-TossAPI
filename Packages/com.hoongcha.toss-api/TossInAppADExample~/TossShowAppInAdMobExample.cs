using System;
using TMPro;
using UnityEngine;

// 무조건 로드 -> 출력 해야합니다.
// 광고 하나가 끝날때 마다 해당 사이클을 반복해야합니다.
// 로드 -> 출력 -> 로드 -> 출력 -> ........
public class TossShowAppInAdMobExample: MonoBehaviour
{
    [SerializeField]
    private TMP_Text debugText;
    [SerializeField]
    private TossAdsConfig tossAdsConfig;
    [SerializeField]
    private TossAdMobJSBridge tossAdMob;
    
    public void ShowAppInAdMob()
    {
        tossAdMob.ShowAppsInTossAdMob(
            tossAdsConfig.AD_GROUP_ID,
            OnSuccess,OnError );
    }
    private void OnSuccess((AdMobEventType eventType, JSBridgeAdResponse adResponse) adEventCallback)
    {
        debugText.text = adEventCallback.adResponse.message;
        switch (adEventCallback.eventType)
        {
            case AdMobEventType.REQUESTED:
                // 광고 보여주기 요청 완료
                break;
            case AdMobEventType.CLICKED:
                // 광고 클릭
                break;
            case AdMobEventType.DISMISSED:
                AudioListener.pause = true; // 광고 끌때 다시 소리 킴 ( 토스 정책 )
                // 광고 닫힘
                break;
            case AdMobEventType.IMPRESSION:
                // 광고 노출
                break;
            case AdMobEventType.SHOW:
                AudioListener.pause = false; // 광고 킬때 음소거 ( 토스 정책 )
                // 광고 컨텐츠 보여졌음
                break;
            case AdMobEventType.USER_EARNED_REWARD:
                var unitType = adEventCallback.adResponse.unitType;
                var unitAmount = adEventCallback.adResponse.unitAmount;
                break;
            case AdMobEventType.FAILED_TO_SHOW:
                // 광고 보여주기 실패
                break;
            case AdMobEventType.ERROR:
                // 알수 없는 에러
                break;
        }
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