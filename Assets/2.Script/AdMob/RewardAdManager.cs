using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleMobileAds.Api;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class RewardAdManager : MonoBehaviour
{
    public static RewardAdManager Instance { get; private set; }

    private RewardedAd rewardedAd;
    public int rewardAmount = 30; // リワードで取得できるジェム数

    private TextMeshProUGUI coinCounterText;
    public int totalCoin { get; private set; }
    private const string GEM_KEY = "TotalCoin";

    public Button rewardAdButton;
    private bool isEarnReward;
    public GameObject nextStageButton;

    public GameObject coinPrefab; // ジェムのプレハブ
    public GameObject rewardShinePrefab;
    public Transform coinParent; // UIのキャンバス内でジェムを配置する親オブジェクト

#if UNITY_IPHONE

    // 【本番用】AdMobのリワード広告本番用ID
    //private string adUnitId = "ca-app-pub-6297510004657467/9863660531";

    // 【テスト用】AdMobのリワード広告テストID
    private string adUnitId = "ca-app-pub-3940256099942544/1712485313";

#else
    private string adUnitId = "unused";
#endif


    //現在のシーン
    private string nowSceneName;
    //ステージ数を文字列で取得
    private string stageNumString;
    //ステージ数文字列をInt型に変換
    private int stageNum;

    //ステージを増やす際はかならずここの数字を変更すること！！
    private int finalStageNum = 30;

    private bool isBgmOn = true;

    private void Awake() {

        if (Instance == null) {

            Instance = this;

        } else {

            Destroy(gameObject);
            return;

        }

        RequestRewardAd();

    }

    void Start() {

        MobileAds.Initialize(initStatus => { });

        isEarnReward = false;

        FindGemText();
        LoadGems();

        rewardAdButton.onClick.AddListener(() => FindObjectOfType<RewardAdManager>().ShowRewardAd());

        //現在のステージ番号を取得する(左から5文字目)
        nowSceneName = SceneManager.GetActiveScene().name;

        stageNumString = nowSceneName.Substring(5);
        stageNum = int.Parse(stageNumString);

        // 初期状態をロード（保存されていれば）
        isBgmOn = PlayerPrefs.GetInt("BGM", 1) == 1;

    }

    private void FindGemText() {

        GameObject gemTextObj = GameObject.Find("CoinCounterText");

        if (gemTextObj != null) {

            coinCounterText = gemTextObj.GetComponent<TextMeshProUGUI>();

        } else {

            Debug.Log("gemTextObjが見つかりません。");
        }

    }

    //ジェム数読み込み
    private void LoadGems() {

        totalCoin = PlayerPrefs.GetInt(GEM_KEY, 0);

    }

    //ジェムに加算
    public void AddGems(int amount) {

        totalCoin += amount;

        // 上限チェック
        if (totalCoin > 9999) {
            totalCoin = 9999;
        }

        SaveGems();
        UpdateGemUI(); // UI更新

    }

    //ジェム数保存
    private void SaveGems() {

        PlayerPrefs.SetInt(GEM_KEY, totalCoin);
        PlayerPrefs.Save();

    }

    //ジェム数更新
    private void UpdateGemUI() {

        if (coinCounterText != null) {

            coinCounterText.text = $"<sprite=0> {totalCoin.ToString()}";

        }

    }

    void RequestRewardAd() {

        if (rewardedAd != null) {
            rewardedAd.Destroy();
            rewardedAd = null;
        }

        var adRequest = new AdRequest();

        RewardedAd.Load(adUnitId, adRequest,
          (RewardedAd ad, LoadAdError error) => {
              // if error is not null, the load request failed.
              if (error != null || ad == null) {
                  Debug.LogError("Rewarded ad failed to load an ad " +
                                 "with error : " + error);
                  return;
              }

              Debug.Log("Rewarded ad loaded with response : "
                        + ad.GetResponseInfo());

              rewardedAd = ad;

              RegisterEventHandlers(rewardedAd);
              RegisterReloadHandler(rewardedAd);

          });

    }

    public void ShowRewardAd() {

        LoadGems();

        isBgmOn = PlayerPrefs.GetInt("BGM", 1) == 1;

        const string rewardMsg =
        "Rewarded ad rewarded the user. Type: {0}, amount: {1}.";

        if (Application.internetReachability == NetworkReachability.NotReachable) {
            Debug.LogWarning("インターネットに接続されていません。広告をスキップして次のステージへ進みます。");
            LoadNextStageScene();
            return;
        }

        if (rewardedAd == null) {

            Debug.Log("リワード広告が準備できていません");
            LoadNextStageScene();
            return;

        }

        if (rewardedAd != null && rewardedAd.CanShowAd()) {
            rewardedAd.Show((Reward reward) => {
                // TODO: Reward the user.
                Debug.Log(System.String.Format(rewardMsg, reward.Type, reward.Amount));

                isEarnReward = true;

                rewardAdButton.gameObject.transform.localScale = Vector3.zero;
                nextStageButton.SetActive(false);
                Debug.Log("リワード広告視聴完了！ジェム + " + rewardAmount);

            });
        } else {

            Debug.Log("リワード広告が準備できていません");

        }
    }

    private void RegisterEventHandlers(RewardedAd ad) {
        // Raised when the ad is estimated to have earned money.
        ad.OnAdPaid += (AdValue adValue) => {
            Debug.Log(System.String.Format("Rewarded ad paid {0} {1}.",
                adValue.Value,
                adValue.CurrencyCode));
        };
        // Raised when an impression is recorded for an ad.
        ad.OnAdImpressionRecorded += () => {
            Debug.Log("Rewarded ad recorded an impression.");
        };
        // Raised when a click is recorded for an ad.
        ad.OnAdClicked += () => {
            Debug.Log("Rewarded ad was clicked.");
        };
        // Raised when an ad opened full screen content.
        ad.OnAdFullScreenContentOpened += () => {
            Debug.Log("Rewarded ad full screen content opened.");

            if (AudioManager.Instance != null) {
                AudioManager.Instance.MuteAll();
            }
        };
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () => {
            Debug.Log("Rewarded ad full screen content closed.");

            if (isBgmOn == true) {

                if (AudioManager.Instance != null) {
                    AudioManager.Instance.UnmuteAll();
                }

            }

            //報酬実行
            EarnReward();

        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) => {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            if (isBgmOn == true) {

                if (AudioManager.Instance != null) {
                    AudioManager.Instance.UnmuteAll();
                }

            }

            LoadNextStageScene();

        };
    }

    private void RegisterReloadHandler(RewardedAd ad) {
        // Raised when the ad closed full screen content.
        ad.OnAdFullScreenContentClosed += () => {
            Debug.Log("Rewarded Ad full screen content closed.");

            // Reload the ad so that we can show another as soon as possible.
            RequestRewardAd();
        };
        // Raised when the ad failed to open full screen content.
        ad.OnAdFullScreenContentFailed += (AdError error) => {
            Debug.LogError("Rewarded ad failed to open full screen content " +
                           "with error : " + error);

            // Reload the ad so that we can show another as soon as possible.
            RequestRewardAd();
        };
    }

    void EarnReward() {

        if (isEarnReward) {

            StartCoroutine(AnimateGems());

        }

    }

    IEnumerator AnimateGems() {

        GameObject shine = Instantiate(rewardShinePrefab, coinParent);
        shine.transform.SetParent(coinParent, false);
        shine.transform.localPosition = new Vector3(0, 0, 0);
        shine.transform.DORotate(new Vector3(0, 0, 360), 2f, RotateMode.FastBeyond360) // 2秒で1回転
            .SetEase(Ease.Linear) // 一定速度
            .SetLoops(-1, LoopType.Restart); // 無限ループ

        int gemCount = 6; // 表示するジェムの個数
        Vector3 targetPos = coinCounterText.transform.position; // ジェムのUI位置
        Vector2 startOffsetRange = new Vector2(100f, 100f); // ランダム配置の範囲

        List<GameObject> gems = new List<GameObject>();

        // ジェムを生成
        for (int i = 0; i < gemCount; i++) {

            GameObject gem = Instantiate(coinPrefab, coinParent);
            gem.transform.SetParent(coinParent, false);

            float randomX = Random.Range(-startOffsetRange.x, startOffsetRange.x);
            float randomY = Random.Range(-startOffsetRange.y, startOffsetRange.y);
            gem.transform.localPosition = new Vector3(randomX, randomY, 0);
            gems.Add(gem);

        }

        yield return new WaitForSeconds(1.2f);

        shine.SetActive(false);

        // DOTweenでアニメーションさせる
        for (int i = 0; i < gems.Count; i++) {

            GameObject gem = gems[i];

            gems[i].transform.DOMove(targetPos, 0.8f)
                .SetEase(Ease.InOutQuad)
                .OnComplete(() => {
                    Destroy(gem); // 到達したら削除
                    Vibration();

                });

            yield return new WaitForSeconds(0.1f); // 少し遅れて次のジェムを動かす
        }

        AddGems(rewardAmount);
        SaveGems();
        UpdateGemUI();

        yield return new WaitForSeconds(2.0f);

        LoadNextStageScene();

    }

    public void LoadNextStageScene() {

        if (stageNum != 30) {

            //今のステージ数にプラス1をしてシーン遷移
            SceneManager.LoadScene("Stage" + (stageNum + 1));

        } else {

            Debug.Log("現在の最終ステージ(finalStageNum)は" + finalStageNum + "です。エンディングシーンに遷移します。");
            SceneManager.LoadScene("Ending");

        }

    }

    public void Vibration() {

        //バイブ振動
        HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Light);

    }
}
