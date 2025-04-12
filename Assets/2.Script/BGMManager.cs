using UnityEngine;
using UnityEngine.UI;

public class BGMManager : MonoBehaviour {
    public Sprite bgmOnSprite;
    public Sprite bgmOffSprite;
    public Image buttonImage;

    private bool isBgmOn = true;

    void Start() {
        // 初期状態をロード（保存されていれば）
        isBgmOn = PlayerPrefs.GetInt("BGM", 1) == 1;
        UpdateBGMState();
    }

    public void ToggleBGM() {
        isBgmOn = !isBgmOn;
        UpdateBGMState();

        // 設定を保存
        PlayerPrefs.SetInt("BGM", isBgmOn ? 1 : 0);
        PlayerPrefs.Save();
    }

    void UpdateBGMState() {
        buttonImage.sprite = isBgmOn ? bgmOnSprite : bgmOffSprite;

        if (isBgmOn == true) {

            if (AudioManager.Instance != null) {
                AudioManager.Instance.UnmuteAll();
            }

        } else {

            if (AudioManager.Instance != null) {
                AudioManager.Instance.MuteAll();
            }

        }
    }


    public void Vibration() {

        //バイブ振動
        HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Light);

    }
}

