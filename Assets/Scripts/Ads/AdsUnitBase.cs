using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;

public abstract class AdsUnitBase : MonoBehaviour
{
    private void Awake()
    {
        // Use the AppStateEventNotifier to listen to application open/close events.
        // This is used to launch the loaded ad when we open the app.
        AppStateEventNotifier.AppStateChanged += OnAppStateChangedBase;
    }

    private void OnDestroy()
    {
        // Always unlisten to events when complete.
        AppStateEventNotifier.AppStateChanged -= OnAppStateChangedBase;
    }

    private void OnAppStateChangedBase(AppState state)
    {
        Debug.Log("App State changed to : " + state);
        OnAppStateChanged(state);
    }

    private IEnumerator Start()
    {
        while (AdsManager.Instance.IsReady == false)
        {
            yield return 0;
        }
        Initialize();
    }
    protected virtual void Initialize()
    {
        // AdsManagerの初期化が終わったあとに呼ばれる
    }
    protected virtual void OnAppStateChanged(AppState state)
    {

    }

}
