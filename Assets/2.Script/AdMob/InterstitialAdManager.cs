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
    private string adUnitId = "ca-app-pub-6297510004657467/8500082753";

    // 【テスト用】AdMobのインタースティシャル広告ID
    // private string adUnitId = "ca-app-pub-3940256099942544/4411468910";


#else
    private string adUnitId = "unused";
#endif

    // 広告表示のカウンター（リスタート用と次ステージ用を分ける）
    private int restartAdCounter = 0;
    private int nextStageAdCounter = 0;
    private int showAdIntervalCount = 2;    // 何回に1回広告を表示させるか

    // 現在表示中の広告の種類を追跡
    private enum AdType { None, Restart, NextStage }
    private AdType currentAdType = AdType.None;

    // 広告が閉じられた時のコールバック
    public delegate void AdClosedCallback();
    private AdClosedCallback onAdClosed;

    // 次ステージボタンの参照を保持
    private NextStageButton nextStageButton;
    private bool isWaitingForNextStageButton = false;

    private void Awake() {

        DontDestroyOnLoad(gameObject);

    }

    void Start() {

        MobileAds.Initialize(initStatus => { });
        LoadInterstitialAd();

        // 初期状態をロード（保存されていれば）
        isBgmOn = PlayerPrefs.GetInt("BGM", 1) == 1;

        // 初回のNextStageButtonの参照を取得
        StartCoroutine(FindNextStageButton());

    }

    private IEnumerator FindNextStageButton()
    {
        // 最大5回まで試行
        int attempts = 0;
        while (nextStageButton == null && attempts < 5)
        {
            // 非アクティブなオブジェクトも含めて検索
            var allNextStageButtons = FindObjectsOfType<NextStageButton>(true);
            if (allNextStageButtons.Length > 0)
            {
                nextStageButton = allNextStageButtons[0];
                Debug.Log($"NextStageButton found in Start - Active: {nextStageButton.gameObject.activeInHierarchy}, " +
                         $"ActiveSelf: {nextStageButton.gameObject.activeSelf}, " +
                         $"Scene: {nextStageButton.gameObject.scene.name}");
                break;
            }
            attempts++;
            Debug.Log($"Attempt {attempts} to find NextStageButton... No buttons found in scene {SceneManager.GetActiveScene().name}");
            yield return new WaitForSeconds(0.5f);
        }

        if (nextStageButton == null)
        {
            Debug.LogWarning($"NextStageButton not found after multiple attempts in scene {SceneManager.GetActiveScene().name}");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // シーンが読み込まれたら、NextStageButtonの参照を更新
        StartCoroutine(UpdateNextStageButtonReference());
    }

    private IEnumerator UpdateNextStageButtonReference()
    {
        // 少し待ってからNextStageButtonを探す（シーンの初期化が完了するのを待つ）
        yield return new WaitForSeconds(0.1f);

        // 非アクティブなオブジェクトも含めて検索
        var allNextStageButtons = FindObjectsOfType<NextStageButton>(true);
        if (allNextStageButtons.Length > 0)
        {
            nextStageButton = allNextStageButtons[0];
            Debug.Log($"NextStageButton reference updated after scene load: Found in scene {SceneManager.GetActiveScene().name} - " +
                     $"Active: {nextStageButton.gameObject.activeInHierarchy}, " +
                     $"ActiveSelf: {nextStageButton.gameObject.activeSelf}");
        }
        else
        {
            Debug.Log($"No NextStageButton found in scene {SceneManager.GetActiveScene().name} after first attempt");
            // 少し待ってから再試行
            yield return new WaitForSeconds(0.5f);
            allNextStageButtons = FindObjectsOfType<NextStageButton>(true);
            if (allNextStageButtons.Length > 0)
            {
                nextStageButton = allNextStageButtons[0];
                Debug.Log($"NextStageButton found in second attempt - Active: {nextStageButton.gameObject.activeInHierarchy}, " +
                         $"ActiveSelf: {nextStageButton.gameObject.activeSelf}");
            }
            else
            {
                Debug.LogWarning($"No NextStageButton found in scene {SceneManager.GetActiveScene().name} after second attempt");
            }
        }
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

            });
    }

    // リスタート時の広告表示
    public void ShowRestartInterstitialAd()
    {
        restartAdCounter++;
        isBgmOn = PlayerPrefs.GetInt("BGM", 1) == 1;
        currentAdType = AdType.Restart;

        if (_interstitialAd == null)
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            LoadInterstitialAd();
            ReloadScene();
            return;
        }

        if (restartAdCounter % showAdIntervalCount == 0 && _interstitialAd.CanShowAd())
        {
            Debug.Log("インタースティシャル広告表示（リスタート）");
            _interstitialAd.Show();
        }
        else
        {
            ReloadScene();
            Debug.Log("リスタート時の広告表示をスキップ");
        }
    }

    // 次ステージへの遷移時の広告表示
    public void ShowNextStageInterstitialAd()
    {
        // NextStageButtonの参照を再確認（非アクティブなオブジェクトも含めて検索）
        if (nextStageButton == null)
        {
            var allNextStageButtons = FindObjectsOfType<NextStageButton>(true);
            if (allNextStageButtons.Length > 0)
            {
                nextStageButton = allNextStageButtons[0];
                Debug.Log($"NextStageButton reference checked in ShowNextStageInterstitialAd: Found - " +
                         $"Active: {nextStageButton.gameObject.activeInHierarchy}, " +
                         $"ActiveSelf: {nextStageButton.gameObject.activeSelf}");
            }
            else
            {
                Debug.LogError($"No NextStageButton found in scene {SceneManager.GetActiveScene().name} when trying to show ad");
            }
        }

        nextStageAdCounter++;
        isBgmOn = PlayerPrefs.GetInt("BGM", 1) == 1;
        currentAdType = AdType.NextStage;

        if (_interstitialAd == null)
        {
            Debug.LogError("Interstitial ad is not ready yet.");
            LoadInterstitialAd();
            if (nextStageButton != null)
            {
                nextStageButton.OnAdClosed();
            }
            else
            {
                Debug.LogError("NextStageButton reference is null, cannot proceed to next stage");
            }
            return;
        }

        if (nextStageAdCounter % showAdIntervalCount == 0 && _interstitialAd.CanShowAd())
        {
            Debug.Log("インタースティシャル広告表示（次ステージ）");
            _interstitialAd.Show();
        }
        else
        {
            Debug.Log("次ステージへの広告表示をスキップ");
            if (nextStageButton != null)
            {
                nextStageButton.OnAdClosed();
            }
            else
            {
                Debug.LogError("NextStageButton reference is null, cannot proceed to next stage");
            }
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
            
            // 広告が閉じられた後の処理を、現在の広告タイプに応じて分岐
            switch (currentAdType)
            {
                case AdType.Restart:
                    Debug.Log("リスタート用広告が閉じられたため、シーンをリロードします");
                    ReloadScene();
                    break;
                case AdType.NextStage:
                    Debug.Log("次ステージ用広告が閉じられたため、次のステージに遷移します");
                    if (nextStageButton != null)
                    {
                        nextStageButton.OnAdClosed();
                    }
                    else
                    {
                        Debug.LogError("NextStageButton reference is null, cannot proceed to next stage");
                    }
                    break;
                default:
                    Debug.LogWarning("不明な広告タイプです");
                    break;
            }

            // 広告をリロード
            LoadInterstitialAd();
            // 広告タイプをリセット
            currentAdType = AdType.None;
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

            // エラー時の処理も同様に分岐
            switch (currentAdType)
            {
                case AdType.Restart:
                    Debug.Log("リスタート用広告の表示に失敗したため、シーンをリロードします");
                    ReloadScene();
                    break;
                case AdType.NextStage:
                    Debug.Log("次ステージ用広告の表示に失敗したため、次のステージに遷移します");
                    if (nextStageButton != null)
                    {
                        nextStageButton.OnAdClosed();
                    }
                    else
                    {
                        Debug.LogError("NextStageButton reference is null, cannot proceed to next stage");
                    }
                    break;
                default:
                    Debug.LogWarning("不明な広告タイプです");
                    break;
            }

            // 広告をリロード
            LoadInterstitialAd();
            // 広告タイプをリセット
            currentAdType = AdType.None;
        };
    }

    void ReloadScene() {

        Time.timeScale = 1.0f;
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);

    }
}
