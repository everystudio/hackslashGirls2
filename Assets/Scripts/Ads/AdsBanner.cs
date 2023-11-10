using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using GoogleMobileAds.Common;
using System;

public class AdsBanner : AdsUnitBase
{
    private BannerView bannerView;

    protected override void Initialize()
    {
        CreateBannerView();
    }

    public void CreateBannerView()
    {
        Debug.Log("Creating banner view");

        // If we already have a banner, destroy the old one.
        if (bannerView != null)
        {
            DestroyBannerView();
        }
        AdSize adaptiveSize =
                AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerView = new BannerView(
            Defines.AD_BOTTOM_BANNER_ID,
            adaptiveSize,
            AdPosition.Bottom);

        bannerView.OnBannerAdLoaded += OnBannerAdLoaded;//bannerViewの状態が バナー表示完了 となった時に起動する関数(関数名HandleAdLoaded)を登録
        bannerView.OnBannerAdLoadFailed += OnBannerAdLoadFailed;//bannerViewの状態が バナー読み込み失敗 となった時に起動する関数(関数名HandleAdFailedToLoad)を登録

        //リクエストを生成
        var adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);

    }

    private void OnBannerAdLoaded()
    {
        Debug.Log("Banner view loaded an ad with response : "
                 + bannerView.GetResponseInfo());
        Debug.Log(string.Format("Ad Height: {0}, width: {1}",
                bannerView.GetHeightInPixels(),
                bannerView.GetWidthInPixels()));
    }

    private void OnBannerAdLoadFailed(LoadAdError error)
    {
        Debug.LogError("Banner view failed to load an ad with error : "
                + error);
    }


    public void DestroyBannerView()
    {
        if (bannerView != null)
        {
            Debug.Log("Destroying banner view.");
            bannerView.Destroy();
            bannerView = null;
        }
    }
}
