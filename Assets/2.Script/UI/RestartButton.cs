using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RestartButton : MonoBehaviour
{

    private InterstitialAdManager interstitialAdManager;

    private void Start() {

        // InterstitialAdManager の参照を取得
        if (interstitialAdManager == null) {
            interstitialAdManager = FindObjectOfType<InterstitialAdManager>();
        }

        if (interstitialAdManager == null) {
            Debug.LogError("InterstitialAdManager が見つかりません。シーンに配置されていますか？");
        }

    }

    public void RestartScene() {

        ShowInterstitialAd();

    }

    void ShowInterstitialAd() {

        interstitialAdManager.ShowRestartInterstitialAd();

    }

    public void Vibration() {

        //バイブ振動
        HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Light);

    }

}
