using UnityEngine;

/// <summary>
/// 광고 설정값만 따로 관리하는 ScriptableObject
/// </summary>
[CreateAssetMenu(menuName = "Configs/TossAdsConfig")]
public class TossAdsConfig : ScriptableObject
{
    /// <summary>
    /// 광고 ID <br></br>
    /// 테스트 id <br></br>
    /// 전면형 광고 : ait-ad-test-interstitial-id <br></br>
    /// 리워드 광고 : ait-ad-test-rewarded-id
    /// </summary>
    public string AD_GROUP_ID = "ait-ad-test-interstitial-id";
}