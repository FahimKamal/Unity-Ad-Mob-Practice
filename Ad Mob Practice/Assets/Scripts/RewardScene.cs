using System;
using GoogleMobileAds.Api;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RewardScene : MonoBehaviour
{
    private int rewardCoin = 0;
    [SerializeField] private Text rewardCoinTxt;

    public int RewardCoin
    {
        get => rewardCoin;
        set
        {
            rewardCoin = value;
            rewardCoinTxt.text = rewardCoin.ToString();
        }
    }
    
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
        AdManager.Instance.OnRewardAdClosedEvent.RemoveAllListeners();
        AdManager.Instance.OnRewardAdClosedEvent.AddListener(For100Reward);
        AdManager.Instance.ShowRewardedAd();
    }
    
    public void OnReward200ButtonPressed()
    {
        AdManager.Instance.OnRewardAdClosedEvent.RemoveAllListeners();
        AdManager.Instance.OnRewardAdClosedEvent.AddListener(For200Reward);
        AdManager.Instance.ShowRewardedAd();
    }
    
    public void OnReward300ButtonPressed()
    {
        AdManager.Instance.OnRewardAdClosedEvent.RemoveAllListeners();
        AdManager.Instance.OnRewardAdClosedEvent.AddListener(For300Reward);
        AdManager.Instance.ShowRewardedAd();
    }
    
    public void OnRequestAdButtonPressed()
    {
        AdManager.Instance.RequestAndLoadRewardedAd();
    }

    public void OnShowPopupButtonPressed()
    {
        PopupManager.Instance.ShowPopup("Popup", "Here's your fucking Popup.");
    }
    
    private void For100Reward()
    {
        Debug.Log("Reward 100 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 100 button pressed");
        RewardCoin += 100;
    }

    private void For200Reward()
    {
        Debug.Log("Reward 200 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 200 button pressed");
        RewardCoin += 200;
    }

    private void For300Reward()
    {
        Debug.Log("Reward 300 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 300 button pressed");
        RewardCoin += 300;
    }
}
