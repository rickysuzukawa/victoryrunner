using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class NextStageButton : MonoBehaviour
{
    private string nowSceneName;
    private string stageNumString;
    //現在のステージの数値
    private int currentStage;

    private InterstitialAdManager interstitialAdManager;

    //最終ステージ数を設定
    //NextStageButtonを押した際にステージ1に戻るためのものです
    private int finalStageNum = 30;
    private bool isAdShowing = false;

    void Start()
    {
        GetStageNumber();

        if (interstitialAdManager == null) {
            interstitialAdManager = FindObjectOfType<InterstitialAdManager>();
        }

        if (interstitialAdManager == null) {
            Debug.LogError("InterstitialAdManager が見つかりません。シーンに配置されていますか？");
        }

    }

    //NextStageButtonを押した際のアクション
    public void LoadNextScene()
    {
        Debug.Log("NextStageButton: LoadNextScene called");
        if (isAdShowing)
        {
            Debug.Log("NextStageButton: Ad is already showing, skipping");
            return;
        }

        //現在のステージ数と最終ステージを確認
        if (currentStage != finalStageNum)
        {
            Debug.Log($"NextStageButton: Current stage {currentStage}, showing ad");
            isAdShowing = true;
            interstitialAdManager.ShowNextStageInterstitialAd();
            // 広告表示後のシーン遷移は、広告が閉じられた後に処理される
        }
        else if (currentStage == finalStageNum)
        {
            Debug.Log("NextStageButton: Final stage reached, loading ending scene");
            //ClearedStageキーを1に戻す
            PlayerPrefs.SetInt("ClearedStage", 1);
            //ステージが最終ならばEndingシーンに遷移
            SceneManager.LoadScene("Ending");
        }
    }

    // 広告が閉じられた後に呼び出されるメソッド
    public void OnAdClosed()
    {
        Debug.Log("NextStageButton: OnAdClosed called");
        isAdShowing = false;
        if (currentStage != finalStageNum)
        {
            Debug.Log($"NextStageButton: Loading next stage {currentStage + 1}");
            //今のステージ数にプラス1をしてシーン遷移
            SceneManager.LoadScene("Stage" + (currentStage + 1));
        }
    }

    //現在のステージ番号を取得する
    void GetStageNumber()
    {
        nowSceneName = SceneManager.GetActiveScene().name;
        stageNumString = nowSceneName.Substring(5);
        currentStage = int.Parse(stageNumString);
        Debug.Log($"NextStageButton: Current stage number set to {currentStage}");
    }

    public void Vibration() {

        //バイブ振動
        HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Light);

    }
}
