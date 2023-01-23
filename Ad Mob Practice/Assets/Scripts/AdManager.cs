using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using UnityEngine.Events;
using UnityEngine.UI;

public class AdManager : MonoBehaviour
{
    /// <summary>
    /// ID's for android
    /// App open	    ca-app-pub-3940256099942544/3419835294
    /// Banner	        ca-app-pub-3940256099942544/6300978111
    /// Interstitial	ca-app-pub-3940256099942544/1033173712
    /// Rewarded	    ca-app-pub-3940256099942544/5224354917
    ///
    /// ID's for iOS
    /// App open	    ca-app-pub-3940256099942544/5662855259
    /// Banner	        ca-app-pub-3940256099942544/2934735716
    /// Interstitial	ca-app-pub-3940256099942544/4411468910
    /// Rewarded	    ca-app-pub-3940256099942544/1712485313
    /// </summary>
    
    public static AdManager Instance;

    [Header("Banner ID's for Admob")] 
    [SerializeField]
    private bool useBanner;
    
    [SerializeField, ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(useBanner) )]
    private string androidBannerId = "ca-app-pub-3940256099942544/6300978111";
    [SerializeField, ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(useBanner) )]
    private string iosBannerId = "ca-app-pub-3940256099942544/2934735716";
    
    [HideInInspector] public string bannerId;
    [HideInInspector] public bool isBannerLoaded = false;
    private bool isBannerVisible = false;
    [Space(10)]
    
    [Header("Interstitial ID's for Admob")]
    [SerializeField]
    private bool useInterstitialAd;
    [SerializeField, ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(useInterstitialAd) )]
    private string androidInterstitialId = "ca-app-pub-3940256099942544/1033173712";
    [SerializeField, ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(useInterstitialAd) )]
    private string iosInterstitialId = "ca-app-pub-3940256099942544/4411468910";
    
    [HideInInspector] public string interstitialId;
    
    [Space(10)]
    
    [Header("Rewarded ID's for Admob")]
    [SerializeField]
    private bool useRewardedAd;
    [SerializeField, ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(useRewardedAd) )]
    private string androidRewardedVideoId = "ca-app-pub-3940256099942544/5224354917";
    [SerializeField, ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(useRewardedAd) )]
    private string iosRewardedVideoId = "ca-app-pub-3940256099942544/1712485313";
    
    [HideInInspector] public string rewardedVideoId;
    
    [Space(10)]
    [Header("FPS Meter")]
    public bool showFpsMeter = true;
    private float deltaTime;
    [SerializeField, ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(showFpsMeter) )]
    private Text fpsMeter;
    [SerializeField, ShowIf(ActionOnConditionFail.DONT_DRAW, ConditionOperator.AND, nameof(showFpsMeter) )]
    private Text statusText;
    
    public UnityEvent OnAdLoadedEvent;
    public UnityEvent OnAdFailedToLoadEvent;
    public UnityEvent OnAdOpeningEvent;
    public UnityEvent OnAdFailedToShowEvent;
    public UnityEvent OnUserEarnedRewardEvent;
    public UnityEvent OnAdClosedEvent;
    
    public BannerView bannerView;
    public InterstitialAd interstitialAd;
    public RewardedAd rewardedAd;

    #region Singleton

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            GetfpsMeter();
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void GetfpsMeter()
    {
        fpsMeter = GameObject.Find("fpsMeter").GetComponent<Text>();
        statusText = GameObject.Find("txtEvents").GetComponent<Text>();
    }

    #endregion
    
    // Start is called before the first frame update
    private void Start()
    {
        #if UNITY_EDITOR
                bannerId = "unused";
                interstitialId = "unused";
                rewardedVideoId = "unused";
        #elif UNITY_ANDROID
                bannerId = androidBannerId;
                interstitialId = androidInterstitialId;
                rewardedVideoId = androidRewardedVideoId;
        #elif UNITY_IOS
                bannerId = iosBannerId;
                interstitialId = iosInterstitialId;
                rewardedVideoId = iosRewardedVideoId;
        #else
                bannerId = "unexpected_platform";
                interstitialId = "unexpected_platform";
                rewardedVideoId = "unexpected_platform";
        #endif
        
        MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);
    }

    private void HandleInitCompleteAction(InitializationStatus initStatus)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            if (useBanner)
                RequestBannerAd();
            if (useInterstitialAd)
                RequestAndLoadInterstitialAd();
            if (useRewardedAd)
                RequestAndLoadRewardedAd();
            
            // RequestNativeBanner();
        });
    }
    
    private void Update()
    {
        if (showFpsMeter)
        {
            fpsMeter.gameObject.SetActive(true);
            deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
            float fps = 1.0f / deltaTime;
            fpsMeter.text = string.Format("{0:0.} fps", fps);
        }
        else
        {
            fpsMeter.gameObject.SetActive(false);
        }
    }
    
    public AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    
    #region BANNER ADS

    public void RequestBannerAd()
    {
        PrintStatus("Requesting Banner ad.");
        // Clean up banner before reusing
        if (bannerView != null)
        {
            bannerView.Destroy();
        }

        // Create a 320x50 banner at top of the screen
        bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);

        // Add Event Handlers
        bannerView.OnAdLoaded += (sender, args) =>
        {
            PrintStatus("Banner ad loaded.");
            isBannerLoaded = true;
            OnAdLoadedEvent.Invoke();
        };
        bannerView.OnAdFailedToLoad += (sender, args) =>
        {
            PrintStatus("Banner ad failed to load with error: "+args.LoadAdError.GetMessage());
            OnAdFailedToLoadEvent.Invoke();
        };
        bannerView.OnAdOpening += (sender, args) =>
        {
            PrintStatus("Banner ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        bannerView.OnAdClosed += (sender, args) =>
        {
            isBannerLoaded = false;
            PrintStatus("Banner ad closed.");
            RequestBannerAd();
            OnAdClosedEvent.Invoke();
        };
        bannerView.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                "Banner ad received a paid event.",
                args.AdValue.CurrencyCode,
                args.AdValue.Value);
            PrintStatus(msg);
        };

        // Load a banner ad
        bannerView.LoadAd(CreateAdRequest());
        bannerView.Hide();
    }
    
    public void DestroyBannerAd()
    {
        if (bannerView != null)
        {
            bannerView.Destroy();
        }
    }
    
    public void ShowTopBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (isBannerLoaded && bannerView != null && !isBannerVisible)
            {
                bannerView.Show();
                isBannerVisible = true;
            }
        }
    }
    public void HideTopBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (isBannerLoaded && isBannerVisible && bannerView != null)
            {
                bannerView.Hide();
                isBannerVisible = false;
            }
        }
    }

    #endregion

    #region INTERSTITIAL ADS

    public void RequestAndLoadInterstitialAd()
    {
        PrintStatus("Requesting Interstitial ad.");

        // Clean up interstitial before using it
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }

        interstitialAd = new InterstitialAd(interstitialId);

        // Add Event Handlers
        interstitialAd.OnAdLoaded += (sender, args) =>
        {
            PrintStatus("Interstitial ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        interstitialAd.OnAdFailedToLoad += (sender, args) =>
        {
            PrintStatus("Interstitial ad failed to load with error: "+args.LoadAdError.GetMessage());
            RequestAndLoadInterstitialAd();
            OnAdFailedToLoadEvent.Invoke();
        };
        interstitialAd.OnAdOpening += (sender, args) =>
        {
            PrintStatus("Interstitial ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        interstitialAd.OnAdClosed += (sender, args) =>
        {
            PrintStatus("Interstitial ad closed.");
            RequestAndLoadInterstitialAd();
            OnAdClosedEvent.Invoke();
        };
        interstitialAd.OnAdDidRecordImpression += (sender, args) =>
        {
            PrintStatus("Interstitial ad recorded an impression.");
        };
        interstitialAd.OnAdFailedToShow += (sender, args) =>
        {
            PrintStatus("Interstitial ad failed to show.");
        };
        interstitialAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Interstitial ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            PrintStatus(msg);
        };

        // Load an interstitial ad
        interstitialAd.LoadAd(CreateAdRequest());
    }

    public void ShowInterstitialAd()
    {
        if (interstitialAd != null && interstitialAd.IsLoaded())
        {
            interstitialAd.Show();
        }
        else
        {
            PrintStatus("Interstitial ad is not ready yet.");
        }
    }

    public void DestroyInterstitialAd()
    {
        if (interstitialAd != null)
        {
            interstitialAd.Destroy();
        }
    }

    #endregion
    
    #region REWARDED ADS

    public void RequestAndLoadRewardedAd()
    {
        PrintStatus("Requesting Rewarded ad.");

        // create new rewarded ad instance
        rewardedAd = new RewardedAd(rewardedVideoId);

        // Add Event Handlers
        rewardedAd.OnAdLoaded += (sender, args) =>
        {
            PrintStatus("Reward ad loaded.");
            OnAdLoadedEvent.Invoke();
        };
        rewardedAd.OnAdFailedToLoad += (sender, args) =>
        {
            PrintStatus("Reward ad failed to load.");
            RequestAndLoadRewardedAd();
            OnAdFailedToLoadEvent.Invoke();
        };
        rewardedAd.OnAdOpening += (sender, args) =>
        {
            PrintStatus("Reward ad opening.");
            OnAdOpeningEvent.Invoke();
        };
        rewardedAd.OnAdFailedToShow += (sender, args) =>
        {
            PrintStatus("Reward ad failed to show with error: "+args.AdError.GetMessage());
            RequestAndLoadRewardedAd();
            OnAdFailedToShowEvent.Invoke();
        };
        rewardedAd.OnAdClosed += (sender, args) =>
        {
            PrintStatus("Reward ad closed.");
            OnAdClosedEvent.Invoke();
        };
        rewardedAd.OnUserEarnedReward += (sender, args) =>
        {
            PrintStatus("User earned Reward ad reward: "+args.Amount);
            OnUserEarnedRewardEvent.Invoke();
            RequestAndLoadRewardedAd();
        };
        rewardedAd.OnAdDidRecordImpression += (sender, args) =>
        {
            PrintStatus("Reward ad recorded an impression.");
        };
        rewardedAd.OnPaidEvent += (sender, args) =>
        {
            string msg = string.Format("{0} (currency: {1}, value: {2}",
                                        "Rewarded ad received a paid event.",
                                        args.AdValue.CurrencyCode,
                                        args.AdValue.Value);
            PrintStatus(msg);
        };

        // Create empty ad request
        rewardedAd.LoadAd(CreateAdRequest());
    }

    public void ShowRewardedAd()
    {
        if (rewardedAd != null)
        {
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
            }
            else
            {
                PrintStatus("Reward ad is not ready yet.");
            }
        }
        else
        {
            RequestAndLoadRewardedAd();
        }
    }

    #endregion
    
  
    
    #region  Ads Call Function
    

    // public void ShowAdmobInterstitial()
    // {
    //     if (PlayerPrefs.GetInt("RemoveAds") == 0)
    //     {
    //         if (interstitial.IsLoaded())
    //         {
    //            interstitial.Show();
    //         }
    //         else
    //         {
    //           RequestInterstitial();
    //         }
    //     }
    // }
    
    // public void ShowAdmobRewardedVideo()
    // {
    //     if (PlayerPrefs.GetInt("RemoveAds") == 0)
    //     {
    //         if (rewardedAd.IsLoaded())
    //         {
    //             rewardedAd.Show();
    //         }
    //         else
    //         {    
    //             RequestAdmobRewarded();
    //         }
    //     }
    // }
    #endregion

    #region Utility

    ///<summary>
    /// Log the message and update the status text on the main thread.
    ///</summary>
    private void PrintStatus(string message)
    {
        Debug.Log(message);
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            statusText.text = message;
        });
    }

    #endregion
    
}
