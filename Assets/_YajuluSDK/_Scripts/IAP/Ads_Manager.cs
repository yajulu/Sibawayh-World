using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyMobile;

namespace _YajuluSDK._Scripts.Ads
{
public class Ads_Manager : MonoBehaviour
{
    

    public void ShowAdIntersential()
    {
        // Check if interstitial ad is ready
        bool isReady = Advertising.IsInterstitialAdReady();

        // Show it if it's ready
        if (isReady)
        {
            Advertising.ShowInterstitialAd();
            Debug.Log("Ad showed");
        }
        else
        {
            Debug.Log("Ad failed to show");
        }
    }

    // Subscribe to the event
    void OnEnable()
    {
        Advertising.InterstitialAdCompleted += InterstitialAdCompletedHandler;
    }  

    // The event handler
    void InterstitialAdCompletedHandler(InterstitialAdNetwork network, AdPlacement placement)
    {
        Debug.Log("Interstitial ad has been closed.");
    }

    // Unsubscribe
    void OnDisable()
    {
        Advertising.InterstitialAdCompleted -= InterstitialAdCompletedHandler;
    }

}
}

