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
        AdManager.Instance.rewardedAd.OnUserEarnedReward += For100Reward;
        AdManager.Instance.ShowRewardedAd();
    }
    
    public void OnReward200ButtonPressed()
    {
        AdManager.Instance.rewardedAd.OnUserEarnedReward += For200Reward;
        AdManager.Instance.ShowRewardedAd();
    }
    
    public void OnReward300ButtonPressed()
    {
        AdManager.Instance.rewardedAd.OnUserEarnedReward += For300Reward;
        AdManager.Instance.ShowRewardedAd();
    }


    void For100Reward(object sender, Reward args)
    {
        Debug.Log("Reward 100 button pressed");
    }
    
    void For200Reward(object sender, Reward args)
    {
        Debug.Log("Reward 200 button pressed");
    }
    
    void For300Reward(object sender, Reward args)
    {
        Debug.Log("Reward 300 button pressed");
    }
}
