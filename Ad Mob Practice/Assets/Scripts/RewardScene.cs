using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardScene : MonoBehaviour
{
    private void Start()
    {
        AdManager.Instance.GetfpsMeter();
    }

    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Home");
    }
    
    public void OnReward100ButtonPressed()
    {
        AdManager.Instance.TestFunction = For100Reward;
        AdManager.Instance.ShowRewardedAd();
    }
    
    public void OnReward200ButtonPressed()
    {
        AdManager.Instance.TestFunction = For200Reward;
        AdManager.Instance.ShowRewardedAd();
    }
    
    public void OnReward300ButtonPressed()
    {
        AdManager.Instance.TestFunction = For300Reward;
        AdManager.Instance.ShowRewardedAd();
    }
    
    public void OnRequestAdButtonPressed()
    {
        AdManager.Instance.RequestAndLoadRewardedAd();
    }


    void For100Reward()
    {
        Debug.Log("Reward 100 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 100 button pressed");
    }
    
    void For200Reward()
    {
        Debug.Log("Reward 200 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 200 button pressed");
    }
    
    void For300Reward()
    {
        Debug.Log("Reward 300 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 300 button pressed");
    }
}
