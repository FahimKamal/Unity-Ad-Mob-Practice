using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;
using System.Collections.Generic;
using UnityEngine;

public class MediationManager : MonoBehaviour
{
    #region Ad_IDs
    public string app_Id;
    public string banner_Id;
    public string nativeBanner_Id;
    public string interstitial_Id;
    public string rewardedVideo_Id;

    public static MediationManager Instance;
    private BannerView bannerView;
    private BannerView nativebannerView;
    private InterstitialAd interstitial;
    private RewardedAd rewardedAd;
    #endregion


  
    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
            print("again");
        }
        else
        {
            print("not again");

            Destroy(this.gameObject);
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        MobileAds.SetiOSAppPauseOnBackground(true);

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(HandleInitCompleteAction);
    }

    private void HandleInitCompleteAction(InitializationStatus initstatus)
    {
        MobileAdsEventExecutor.ExecuteInUpdate(() =>
        {
            RequestBanner();
            RequestInterstitial();
            RequestAdmobRewarded();
           // RequestNativeBanner();
            ShowTopBanner();
        });
    }


    #region Request Ads
    public AdRequest CreateAdRequest()
    {
        return new AdRequest.Builder().Build();
    }
    private void RequestInterstitial()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            // Clean up interstitial ad before creating a new one.
            if (this.interstitial != null)
            {
                this.interstitial.Destroy();
            }

            // Create an interstitial.
            this.interstitial = new InterstitialAd(interstitial_Id);

            // Register for ad events.
            this.interstitial.OnAdLoaded += this.HandleOnAdLoaded;
            this.interstitial.OnAdFailedToLoad += this.HandleOnAdFailedToLoad;
            this.interstitial.OnAdOpening += this.HandleOnAdOpened;
            this.interstitial.OnAdClosed += this.HandleOnAdClosed;
            // this.interstitial. += this.HandleOnAdLeavingApplication;

            // Load an interstitial ad.
            this.interstitial.LoadAd(this.CreateAdRequest());
        }

    }
    public void RequestBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            // Clean up banner ad before creating a new one.
            if (this.bannerView != null)
            {
                this.bannerView.Destroy();
            }
            // Create a 320x50 banner at the top of the screen.
            this.bannerView = new BannerView(banner_Id, AdSize.Banner, AdPosition.Top);

            // Register for ad events.
            this.bannerView.OnAdLoaded += this.HandleAdLoaded;
            this.bannerView.OnAdFailedToLoad += this.HandleAdFailedToLoad;
            this.bannerView.OnAdOpening += this.HandleAdOpened;
            this.bannerView.OnAdClosed += this.HandleAdClosed;
            //this.bannerView.OnAdLeavingApplication += this.HandleAdLeftApplication;

            // Load a banner ad.
            this.bannerView.LoadAd(this.CreateAdRequest());
            this.bannerView.Hide();
            print("Show Top Banner");
        }
    }
    public void RequestNativeBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (this.nativebannerView != null)
            {
                this.nativebannerView.Destroy();
            }

            this.nativebannerView = new BannerView(banner_Id, AdSize.MediumRectangle, 0, 54);


            this.nativebannerView.LoadAd(this.CreateAdRequest());
            this.nativebannerView.Hide();
        }
    }
    private void RequestAdmobRewarded()
    {

        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            this.rewardedAd = new RewardedAd(rewardedVideo_Id);

            // Called when an ad request has successfully loaded.
            this.rewardedAd.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            //      this.rewardedAd.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;

            // Called when an ad is shown.
            this.rewardedAd.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            this.rewardedAd.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this.rewardedAd.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            this.rewardedAd.OnAdClosed += HandleRewardedAdClosed;


            // Load the rewarded ad with the request.
            this.rewardedAd.LoadAd(this.CreateAdRequest());
        }
    }
    #endregion



    #region Banner callback handlers

    public void HandleAdLoaded(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: " + args.LoadAdError);
    }

    public void HandleAdOpened(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");
    }

    public void HandleAdClosed(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
    }

    public void HandleAdLeftApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeftApplication event received");
    }

    #endregion

    #region Interstial callback handlers 
    public void HandleOnAdLoaded(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleAdLoaded event received");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        MonoBehaviour.print("HandleFailedToReceiveAd event received with message: "
                            + args.ToString());
        RequestInterstitial();
    }

    public void HandleOnAdOpened(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleAdOpened event received");

    }

    public void HandleOnAdClosed(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleAdClosed event received");
        RequestInterstitial();

    }

    public void HandleOnAdLeavingApplication(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
    }
    #endregion
    #region AdmobRewarded_Ad callback handlers

    public void HandleRewardedAdLoaded(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdLoaded event received");
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToLoad event received with message: "
                             + args.AdError.GetMessage());
       // RequestAdmobRewarded();
    }

    public void HandleRewardedAdOpening(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdOpening event received");
       // RequestAdmobRewarded();
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.AdError.GetMessage());

      //  RequestAdmobRewarded();
    }

    public void HandleRewardedAdClosed(object sender, System.EventArgs args)
    {
        MonoBehaviour.print("HandleRewardedAdClosed event received");
        RequestAdmobRewarded();

    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {
        string type = args.Type;
        double amount = args.Amount;
     
           //PlayerPrefs.SetInt("WatchYes", 1);
           //ShowFreeCoins();
       
    }

    #endregion

    #region  Ads Call Function
    public void ShowTopBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (this.bannerView != null)
            {
                this.bannerView.Show();
            }
            // else
            // {
            //     AdViewScene.instance.ShowFbBanner();
            // }
        }
    }
    public void HideTopBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (this.bannerView != null)
            {
                this.bannerView.Hide();
            }
        }
    }
    public void ShowNativeBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (this.nativebannerView != null)
            {
                this.nativebannerView.Show();
            }
            //HideTopBanner();
        }
    }
    public void HideNativeBanner()
    {
        if (PlayerPrefs.GetInt("RemoveAds") == 0)
        {
            if (this.nativebannerView != null)
            {
                this.nativebannerView.Hide();
            }
        }
        //ShowTopBanner();
    }

    public void ShowAdmobInterstial()
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
            if (this.rewardedAd.IsLoaded())
            {
                this.rewardedAd.Show();
            }
            else
            {    
                RequestAdmobRewarded();
            }
        }
    }
    #endregion
}

