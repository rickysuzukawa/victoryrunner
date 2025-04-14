using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds;
using GoogleMobileAds.Api;
using UnityEngine.SceneManagement;

public class InterstitialAdManager : MonoBehaviour
{
    private InterstitialAd _interstitialAd;

    private bool isBgmOn = true;

#if UNITY_IPHONE

    // 【本番用】AdMobのインタースティシャル広告ID
    //private string adUnitId = "ca-app-pub-6297510004657467/8500082753";

    // 【テスト用】AdMobのインタースティシャル広告ID
    private string adUnitId = "ca-app-pub-3940256099942544/4411468910";


#else
    private string adUnitId = "unused";
#endif

    private int adCounter = 0;              // 広告表示のカウント
    private int showAdIntervalCount = 2;    // 何回に1回広告を表示させるか

    private void Awake() {

        DontDestroyOnLoad(gameObject);

    }

    void Start() {

        MobileAds.Initialize(initStatus => { });
        LoadInterstitialAd();

        // 初期状態をロード（保存されていれば）
        isBgmOn = PlayerPrefs.GetInt("BGM", 1) == 1;

    }

    public void LoadInterstitialAd() {

        if (_interstitialAd != null) {

            _interstitialAd.Destroy();
            _interstitialAd = null;

        }

        Debug.Log("インタースティシャル広告を読み込み");

        var adRequest = new AdRequest();

        InterstitialAd.Load(adUnitId, adRequest,
            (InterstitialAd ad, LoadAdError error) => {
                if (error != null || ad == null) {

                    Debug.LogError("interstitial ad failed to load an ad " +
                                   "with error : " + error);
                    return;

                }

                Debug.Log("Interstitial ad loaded with response : "
                          + ad.GetResponseInfo());

                _interstitialAd = ad;

                RegisterEventHandlers(_interstitialAd);
                RegisterReloadHandler(_interstitialAd);

            });
    }

    public void ShowInterstitialAd() {

        adCounter++;

        isBgmOn = PlayerPrefs.GetInt("BGM", 1) == 1;

        if (_interstitialAd == null) {

            Debug.LogError("Interstitial ad is not ready yet.");
            LoadInterstitialAd(); // 再度広告をロード
            ReloadScene();
            return;

        }

        if (adCounter % showAdIntervalCount == 0 && _interstitialAd.CanShowAd()) {

            Debug.Log("インタースティシャル広告表示");
            _interstitialAd.Show();

        } else {

            ReloadScene();
            Debug.LogError("Interstitial ad cannot be shown.");

        }

    }

    private void RegisterEventHandlers(InterstitialAd interstitialAd) {
        // Raised when the ad is estimated to have earned money.
        interstitialAd.OnAdPaid += (AdValue adValue) => {
            Debug.Log(System.String.Format("Interstitial ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        interstitialAd.OnAdImpressionRecorded += () => {
            Debug.Log("Interstitial ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        interstitialAd.OnAdClicked += () => {
            Debug.Log("Interstitial ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        interstitialAd.OnAdFullScreenContentOpened += () => {

            Debug.Log("インタースティシャル広告が開きました");

            if (AudioManager.Instance != null) {
                AudioManager.Instance.MuteAll();
            }

        };
        // Raised when the ad closed full screen content.
        interstitialAd.OnAdFullScreenContentClosed += () => {

            Debug.Log("インタースティシャル広告が閉じられました");

            if (isBgmOn == true) {

                if (AudioManager.Instance != null) {
                    AudioManager.Instance.UnmuteAll();
                }

            }
            

            ReloadScene();

        };
        // Raised when the ad failed to open full screen content.
        interstitialAd.OnAdFullScreenContentFailed += (AdError error) => {
            Debug.LogError("Interstitial ad failed to open full screen content " +
                           "with error : " + error);

            if (isBgmOn == true) {

                if (AudioManager.Instance != null) {
                    AudioManager.Instance.UnmuteAll();
                }

            }

            ReloadScene();
        };

    }

    private void RegisterReloadHandler(InterstitialAd ad) {

        // 広告が全画面コンテンツを閉じたときに発生します。

        ad.OnAdFullScreenContentClosed += () => {

            Debug.Log("インタースティシャル広告の全画面コンテンツが閉じられました。");

            LoadInterstitialAd();

            ReloadScene();

        };

        // 広告が全画面コンテンツを開けなかった場合に発生します。

        ad.OnAdFullScreenContentFailed += (AdError error) => {

            Debug.LogError("インタースティシャル広告が全画面コンテンツを開けませんでした " +

                           "with error : " + error);

            // できるだけ早く別の広告を表示できるよう、広告をリロードしてください。

            LoadInterstitialAd();

        };

    }

    void ReloadScene() {

        Time.timeScale = 1.0f;
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);

    }
}
