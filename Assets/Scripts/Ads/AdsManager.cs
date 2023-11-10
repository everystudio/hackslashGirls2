using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public class AdsManager : Singleton<AdsManager>
{
    private bool isReady = false;
    public bool IsReady
    {
        get
        {
            return isReady;
        }
    }
    public override void Initialize()
    {
        base.Initialize();
        dontDestroyOnLoad = true;

    }

    private void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) =>
        {
            // This callback is called once the MobileAds SDK is initialized.
            isReady = true;
        });
    }

}
