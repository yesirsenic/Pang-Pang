using UnityEngine;
using GoogleMobileAds.Api;
using System;
using UnityEngine.Rendering.Universal;
using GoogleMobileAds.Ump.Api;

public class AdsManager : MonoBehaviour
{
    public static AdsManager Instance;

    private Action onRewardCompleted;
    private Action onRewardFailed;
    private bool rewardGranted;

#if UNITY_ANDROID
    private string rewardedAdUnitId = "ca-app-pub-9548284037151614/3825356361";
    private string interstitialAdUnitId = "ca-app-pub-9548284037151614/9176604010";
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

        MobileAds.RaiseAdEventsOnUnityMainThread = true;
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

            rewardedAd.OnAdFullScreenContentClosed += () =>
            {
                if (!rewardGranted)
                    onRewardFailed?.Invoke();

                ResetRewardState();
                LoadRewardedAd();
            };

            rewardedAd.OnAdFullScreenContentFailed += error =>
            {
                onRewardFailed?.Invoke();
                ResetRewardState();
                LoadRewardedAd();
            };
        });
    }

    public void ShowRewardedAd(Action onCompleted, Action onFailed)
    {
        if (rewardedAd == null || !rewardedAd.CanShowAd())
        {
            onFailed?.Invoke();
            return;
        }

        onRewardCompleted = onCompleted;
        onRewardFailed = onFailed;
        rewardGranted = false;

        rewardedAd.Show(reward =>
        {
            rewardGranted = true;
            onRewardCompleted?.Invoke();   // ⭐ 여기서 즉시 Resume
        });
    }

    private void ResetRewardState()
    {
        rewardGranted = false;
        onRewardCompleted = null;
        onRewardFailed = null;
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