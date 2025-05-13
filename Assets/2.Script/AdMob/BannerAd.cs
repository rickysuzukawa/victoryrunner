using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;

public class BannerAd : MonoBehaviour
{
    private static BannerAd instance;
    private BannerView bannerView;

    //【本番用】
    private string adUnitId = "ca-app-pub-6297510004657467/4962687641";

    //【テスト用】
    // private string adUnitId = "ca-app-pub-3940256099942544/2934735716";

    void Awake() {
        // すでにインスタンスが存在する場合は破棄
        if (instance != null && instance != this) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // シーン遷移時に破棄されないようにする
    }

    void Start() {

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize((InitializationStatus initStatus) => { });

        if (bannerView == null) {

            RequestBanner();

        }

    }

    void RequestBanner() {

        if (bannerView != null) return; // すでにある場合は作らない

        AdSize adaptiveSize = AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSizeWithWidth(AdSize.FullWidth);

        bannerView = new BannerView(adUnitId, adaptiveSize, AdPosition.Bottom);
        var adRequest = new AdRequest();
        bannerView.LoadAd(adRequest);

    }

    void OnDestroy() {

        // インスタンスが削除されたらバナーも解放
        if (bannerView != null) {

            bannerView.Destroy();
            bannerView = null;

        }

    }

    public void HideBanner() {
        if (bannerView != null) {
            bannerView.Hide();
        }
    }

    public void ShowBanner() {
        if (bannerView != null) {
            bannerView.Show();
        }
    }
}
