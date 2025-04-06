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


    void ShowInterstitialAd() {

        interstitialAdManager.ShowInterstitialAd();

    }

    public void RestartScene() {

        ShowInterstitialAd();

        Time.timeScale = 1.0f;
        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);

    }

    public void Vibration() {

        //バイブ振動
        HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Light);

    }

}
