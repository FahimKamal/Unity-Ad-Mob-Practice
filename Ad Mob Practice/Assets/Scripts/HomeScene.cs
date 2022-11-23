using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScene : MonoBehaviour
{
    public void OnBannerButtonPressed()
    {
        
    }
    
    public void OnInterstitialButtonPressed()
    {
        // Open the Interstitial scene
        SceneManager.LoadScene("Scenes/Interstitial");
    }

    public void OnRewardedButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Rewarded");
    }
}
