using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Rendering.Universal;
using GoogleMobileAds.Ump.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

#if UNITY_ANDROID
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
#elif UNITY_IOS
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    // 👇 에디터용 
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
#endif

    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void Start()
    {
        MobileAds.Initialize(_ =>
        {
            LoadRewardedAd();
            LoadInterstitialAd();
        });
    }

    // =========================
    // Rewarded
    // =========================
    public void LoadRewardedAd()
    {
        RewardedAd.Load(rewardedAdUnitId, new AdRequest(), (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Rewarded load failed");
                return;
            }

            rewardedAd = ad;
        });
    }

    public void ShowRewardedAd(Action onCompleted, Action onFailed = null)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(reward =>
            {
                onCompleted?.Invoke();
                LoadRewardedAd(); // 다음 광고 미리 로드
            });
        }
        else
        {
            onFailed?.Invoke();
        }
    }

    // =========================
    // Interstitial
    // =========================
    public void LoadInterstitialAd()
    {
        InterstitialAd.Load(interstitialAdUnitId, new AdRequest(), (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Interstitial load failed");
                return;
            }

            interstitialAd = ad;
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
            LoadInterstitialAd();
        }
    }

}