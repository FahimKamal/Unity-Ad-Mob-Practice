using System.Collections;
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
        if (AdManager.Instance.rewardedAd.IsLoaded())
        {
            AdManager.Instance.ShowRewardedAd();
            StartCoroutine(For100Reward());
        }
        
    }
    
    public void OnReward200ButtonPressed()
    {
        if (AdManager.Instance.rewardedAd.IsLoaded())
        {
            AdManager.Instance.ShowRewardedAd();
            StartCoroutine(For200Reward());
        }
    }
    
    public void OnReward300ButtonPressed()
    {
        if (AdManager.Instance.rewardedAd.IsLoaded())
        {
            AdManager.Instance.ShowRewardedAd();
            StartCoroutine(For300Reward());
        }
    }
    
    public void OnRequestAdButtonPressed()
    {
        AdManager.Instance.RequestAndLoadRewardedAd();
    }

    public void OnShowPopupButtonPressed()
    {
        PopupManager.Instance.ShowPopup("Popup", "Here's your fucking Popup.");
    }
    
    private IEnumerator For100Reward()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Reward 100 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 100 button pressed");
        RewardCoin += 100;
    }

    private IEnumerator For200Reward()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Reward 200 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 200 button pressed");
        RewardCoin += 200;
    }

    private IEnumerator For300Reward()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Reward 300 button pressed");
        PopupManager.Instance.ShowPopup("Ad Showed", "Reward 300 button pressed");
        RewardCoin += 300;
    }
}
