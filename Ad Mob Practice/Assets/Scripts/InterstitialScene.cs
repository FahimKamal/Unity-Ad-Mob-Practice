﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterstitialScene : MonoBehaviour
{
    private void Start()
    {
        AdManager.Instance.GetfpsMeter();
    }

    public void OnInterstitialButtonPressed()
    {
        // AdManager.Instance.OnAdClosedEvent.AddListener(() =>
        // {
        //     Debug.Log("Interstitial successfully showed.");
        //     PopupManager.Instance.ShowPopup("Ad Showed", "Interstitial successfully showed.");
        // });
        AdManager.Instance.ShowInterstitialAd();
        Debug.Log("Interstitial button pressed");
    }
    
    public void OnBackButtonPressed()
    {
        SceneManager.LoadScene("Scenes/Home");
    }
}
