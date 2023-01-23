using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScene : MonoBehaviour
{
    private void Start()
    {
        AdManager.Instance.GetfpsMeter();
    }
    public void OnShowBannerButtonPressed()
    {
        AdManager.Instance.ShowTopBanner();
    }
    
    public void OnHideBannerButtonPressed()
    {
        AdManager.Instance.HideTopBanner();
    }
    
    public void OnInterstitialButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Interstitial");
    }

    public void OnRewardedButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Rewarded");
    }

    public void OnQuitButtonPressed()
    {
        Application.Quit();
    }
}
