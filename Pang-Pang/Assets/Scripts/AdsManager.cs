using UnityEngine;
using GoogleMobileAds.Api;
using System;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

#if UNITY_ANDROID
    private string rewardedAdUnitId = "ca-app-pub-9548284037151614/3825356361";
    private string interstitialAdUnitId = "ca-app-pub-9548284037151614/9176604010";
#elif UNITY_IOS
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/1712485313";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
    private string rewardedAdUnitId = "ca-app-pub-3940256099942544/5224354917";
    private string interstitialAdUnitId = "ca-app-pub-3940256099942544/1033173712";
#endif

    private bool adsInitialized = false;

    private RewardedAd rewardedAd;
    private InterstitialAd interstitialAd;

    #region Unity

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

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
        InitializeAds();
    }

    #endregion

    #region Initialization

    private void InitializeAds()
    {
        MobileAds.Initialize(_ =>
        {
            adsInitialized = true;
            LoadRewardedAd();
            LoadInterstitialAd();
        });
    }

    #endregion

    // =========================
    // Rewarded
    // =========================

    private void LoadRewardedAd()
    {
        if (!adsInitialized) return;

        RewardedAd.Load(rewardedAdUnitId, new AdRequest(), (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Rewarded Load Failed");
                return;
            }

            rewardedAd = ad;

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                rewardedAd = null;
                LoadRewardedAd(); // 광고 끝나면 다음 광고 미리 준비
            };

            rewardedAd.OnAdFullScreenContentFailed += (err) =>
            {
                rewardedAd = null;
                LoadRewardedAd();
            };

            Debug.Log("Rewarded Loaded");
        });
    }

    public void ShowRewardedAd(Action onCompleted = null, Action onFailed = null)
    {
        if (rewardedAd != null && rewardedAd.CanShowAd())
        {
            rewardedAd.Show(reward =>
            {
                onCompleted?.Invoke();
            });
        }
        else
        {
            Debug.Log("Rewarded Not Ready");
            onFailed?.Invoke();

            if (rewardedAd == null)
                LoadRewardedAd();
        }
    }

    // =========================
    // Interstitial
    // =========================

    private void LoadInterstitialAd()
    {
        if (!adsInitialized) return;

        InterstitialAd.Load(interstitialAdUnitId, new AdRequest(), (ad, error) =>
        {
            if (error != null || ad == null)
            {
                Debug.Log("Interstitial Load Failed");
                return;
            }

            interstitialAd = ad;

            interstitialAd.OnAdFullScreenContentClosed += () =>
            {
                interstitialAd = null;
                LoadInterstitialAd(); // 닫히면 다음 광고 준비
            };

            interstitialAd.OnAdFullScreenContentFailed += (err) =>
            {
                interstitialAd = null;
                LoadInterstitialAd();
            };

            Debug.Log("Interstitial Loaded");
        });
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.CanShowAd())
        {
            interstitialAd.Show();
        }
        else
        {
            Debug.Log("Interstitial Not Ready");

            if (interstitialAd == null)
                LoadInterstitialAd();
        }
    }
}
