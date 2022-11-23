using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterstitialScene : MonoBehaviour
{
    public void OnInterstitialButtonPressed()
    {
        AdManager.Instance.ShowAdmobInterstitial();
        Debug.Log("Interstitial button pressed");
    }
    
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Home");
    }
}
