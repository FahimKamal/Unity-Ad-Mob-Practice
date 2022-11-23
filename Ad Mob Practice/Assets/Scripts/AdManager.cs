using System;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

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
    
    [Header("App ID's for Admob")]
    public string androidAppId = "ca-app-pub-3940256099942544/3419835294";
    public string iosAppId = "ca-app-pub-3940256099942544/5662855259";
    [Space(10)]
    
    [Header("Banner ID's for Admob")]
    public string androidBannerId = "ca-app-pub-3940256099942544/6300978111";
    public string iosBannerId = "ca-app-pub-3940256099942544/2934735716";
    [HideInInspector] public string bannerId;
    [HideInInspector] public bool isBannerLoaded = false;
    private bool isBannerVisible = false;
    [Space(10)]
    
    [Header("Interstitial ID's for Admob")]
    public string androidInterstitialId = "ca-app-pub-3940256099942544/1033173712";
    public string iosInterstitialId = "ca-app-pub-3940256099942544/4411468910";
    [HideInInspector] public string interstitialId;
    
    [Space(10)]
    
    [Header("Rewarded ID's for Admob")]
    public string androidRewardedVideoId = "ca-app-pub-3940256099942544/5224354917";
    public string iosRewardedVideoId = "ca-app-pub-3940256099942544/1712485313";
    [HideInInspector] public string rewardedVideoId;
    
     public BannerView bannerView;
     public InterstitialAd interstitial;
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
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion
    
    // Start is called before the first frame update
    private void Start()
    {
        #if UNITY_ANDROID
                bannerId = androidBannerId;
                interstitialId = androidInterstitialId;
                rewardedVideoId = androidRewardedVideoId;
        #elif UNITY_IOS
                bannerId = iosBannerId;
                interstitialId = iosInterstitialId;
                rewardedVideoId = iosRewardedVideoId;
        #endif
        
        MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);
    }

    private void HandleInitCompleteAction(InitializationStatus initStatus)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            RequestBanner();
            RequestInterstitial();
            RequestAdmobRewarded();
            // RequestNativeBanner();
        });
    }
    
     #region Request Ads
    public AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    public void RequestInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            // Clean up interstitial ad before creating a new one.
            // We might have to shift this if statement to the place where we are calling this function.
            // if (interstitial != null)
            // {
            //     interstitial.Destroy();
            // }

            // Create an interstitial.
            interstitial = new InterstitialAd(interstitialId);

            // Register for ad events.
            // interstitial.OnAdLoaded += this.HandleOnAdLoaded;
            // interstitial.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
            // interstitial.OnAdOpening += this.HandleOnAdOpened;
             interstitial.OnAdClosed += this.HandleOnAdClosed;
            // this.interstitial. += this.HandleOnAdLeavingApplication;

            // Load an interstitial ad.
            interstitial.LoadAd(this.CreateAdRequest());

        }
        
    }
    public void RequestBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            // Clean up banner ad before creating a new one.
            // We might have to shift this if statement to the place where we are calling this function.
            if (bannerView != null)
            {
                bannerView.Destroy();
            }
            // Create a 320x50 banner at the top of the screen.
            bannerView = new BannerView(bannerId, AdSize.Banner, AdPosition.Top);

            // Register for ad events.
            bannerView.OnAdLoaded += this.HandleAdLoaded;
            bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
            bannerView.OnAdOpening += this.HandleAdOpened;
            bannerView.OnAdClosed += this.HandleAdClosed;
            // bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

            // Load a banner ad.
            bannerView.LoadAd(this.CreateAdRequest());
            bannerView.Hide();
            print("Show Top Banner");
        }
    }
    
    private void RequestAdmobRewarded()
    {

        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            rewardedAd = new RewardedAd(rewardedVideoId);

            // Called when an ad request has successfully loaded.
            // rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            
            // Called when an ad request failed to load.
            // this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

            // Called when an ad is shown.
            // rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            
            // Called when an ad request failed to show.
            // rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            
            // Called when the user should be rewarded for interacting with the ad.
            // rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            
            // Called when the ad is closed.
            rewardedAd.OnAdClosed += HandleRewardedAdClosed;


            // Load the rewarded ad with the request.
            rewardedAd.LoadAd(this.CreateAdRequest());
        }
    }
    #endregion
    
    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received");
        isBannerLoaded = true;
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: " + args.LoadAdError);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
        isBannerLoaded = false;
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        print("HandleAdLeftApplication event received");
    }

    #endregion
    
    
    #region Interstial callback handlers 
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        print("HandleFailedToReceiveAd event received with message: "
                            + args);
        RequestInterstitial();
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        print("HandleAdOpened event received");

    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        print("HandleAdClosed event received");
        RequestInterstitial();

    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        print("HandleAdLeavingApplication event received");
    }
    #endregion
    
    
    #region AdmobRewarded_Ad callback handlers

    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        print("HandleRewardedAdFailedToLoad event received with message: "
              + args.AdError.GetMessage());
        // RequestAdmobRewarded();
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        print("HandleRewardedAdOpening event received");
        // RequestAdmobRewarded();
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        print("HandleRewardedAdFailedToShow event received with message: "
              + args.AdError.GetMessage());

        //  RequestAdmobRewarded();
    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        print("HandleRewardedAdClosed event received");
        RequestAdmobRewarded();

    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        // Todo: After successfully watching rewarded video, give reward to user.
    }

    #endregion
    
    
    
    #region  Ads Call Function
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

    public void ShowAdmobInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (interstitial.IsLoaded())
            {
               interstitial.Show();
            }
            else
            {
              RequestInterstitial();
            }
        }
    }
    
    public void ShowAdmobRewardedVideo()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (rewardedAd.IsLoaded())
            {
                rewardedAd.Show();
            }
            else
            {    
                RequestAdmobRewarded();
            }
        }
    }
    #endregion
    
}
