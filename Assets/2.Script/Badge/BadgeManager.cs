using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class BadgeManager : MonoBehaviour
{
    public List<Badge> badges;              // バッジデータのリスト
    public TMP_Text coinText;               // 画面右上のコイン数表示
    private int totalCoin;                  // 現在のコイン数

    public List<GameObject> badgeImages;         // 各バッジのイメージ画像
    public List<Button> badgeButtons;       // 各バッジの購入ボタン
    public List<TMP_Text> badgePrices;      // 各バッジの価格表示テキスト
    public List<ParticleSystem> badgeShiningEffects;    //キラキラ光らせるエフェクト

    public GameObject badgeGetDisplayPanel; //バッジ取得時のパネル(背景黒くするため)
    public RectTransform badgeDisplay;      // 中央に表示するバッジImage用のRectTransform
    public Image badgeImage;                // 表示するバッジ画像
    public ParticleSystem confettiEffect;   // 紙吹雪のパーティクル

    public AudioClip badgeGetSound;            // バッジ取得時のサウンド
    private AudioSource audioSource;        // サウンド再生用

    void Start() {

        totalCoin = PlayerPrefs.GetInt("TotalCoin", 0);
        LoadBadgeGetData();
        UpdateCoinUI();
        UpdateBadgeUI();

        badgeGetDisplayPanel.gameObject.SetActive(false);
        badgeDisplay.gameObject.SetActive(false);           // 初期非表示
        confettiEffect.Stop();                              // パーティクル停止

        audioSource = GetComponent<AudioSource>(); // AudioSource を取得

    }

    // バッジのUIを更新
    public void UpdateBadgeUI() {
        //現在のコイン数を取得
        totalCoin = PlayerPrefs.GetInt("TotalCoin", 0);

        //バッジの種類の数だけ繰り返す
        for (int i = 0; i < badges.Count; i++) {
            Badge badge = badges[i];
            GameObject image = badgeImages[i];
            Button buyButton = badgeButtons[i];
            TMP_Text priceText = badgePrices[i];
            ParticleSystem badgeShiningEffect = badgeShiningEffects[i];

            //バッジがアンロックされているか順番に確認
            if (badge.isUnlocked) {

                //取得済の場合にボタンに表示させる文字
                image.GetComponent<Image>().color = new Color(1.0f, 1.0f, 1.0f, 1.0f);
                buyButton.interactable = false;
                priceText.text = "GET!!";
                //バッジゲットキラキラエフェクトは再生
                badgeShiningEffect.Play();

            } else {

                //未取得の場合コインの必要枚数を表示
                priceText.text = $"<sprite=0> x {badge.price}";
                //コインの取得枚数が必要枚数を上回っていたらボタンをアクティブにする
                buyButton.interactable = totalCoin >= badge.price;
                //バッジゲットキラキラエフェクトは停止
                badgeShiningEffect.Stop();
                int index = i; // 必要に応じてボタンのインデックスをキャプチャ
                buyButton.onClick.RemoveAllListeners();
                buyButton.onClick.AddListener(() => PurchaseBadge(index));

            }

            SaveBadgeGetData();

            //Debug.Log(badges[i].price + "　は　" + badges[i].isUnlocked);
        }
    }

    // バッジ購入処理
    void PurchaseBadge(int index) {
        Badge badge = badges[index];
        if (totalCoin >= badge.price && !badge.isUnlocked) {
            totalCoin -= badge.price;
            badge.isUnlocked = true;
            UpdateCoinUI();
            UpdateBadgeUI();

            ShowBadgeEffect(badge.badgeSprite);
            
            ShowBadgeShiningEffect();

            PlayBadgeSound();

        }
    }

    // コインUIを更新
    void UpdateCoinUI() {
        PlayerPrefs.SetInt("TotalCoin", totalCoin);
        totalCoin = PlayerPrefs.GetInt("TotalCoin", 0);
        coinText.text = $"<sprite=0> {totalCoin}";
    }

    //バッジの取得状態を記録
    void SaveBadgeGetData() {
        PlayerPrefs.SetInt("TotalCoin", totalCoin);
        for (int i = 0; i < badges.Count; i++) {

            PlayerPrefs.SetInt($"BadgeUnlocked_{i}", badges[i].isUnlocked ? 1 : 0);

        }
        PlayerPrefs.Save();

    }

    //バッジの取得状態を取得、ロード
    public void LoadBadgeGetData() {
        totalCoin = PlayerPrefs.GetInt("TotalCoin", 0);
        for (int i = 0; i < badges.Count; i++) {

            badges[i].isUnlocked = PlayerPrefs.GetInt($"BadgeUnlocked_{i}", 0) == 1;

        }

    }

    //ここにCanvas(BadgeGet)内のPanel表示非表示処理やバッジのくるくるゲットアニメーション、紙吹雪パーティクルの再生など
    void ShowBadgeEffect(Sprite badgeSprite) {
        //イメージ画像にbadgeSpriteを設定
        badgeImage.sprite = badgeSprite;
        //イメージのスケールを0に設定
        badgeDisplay.localScale = Vector3.zero;
        badgeGetDisplayPanel.gameObject.SetActive(true);
        badgeDisplay.gameObject.SetActive(true);

        // 紙吹雪のパーティクル再生
        confettiEffect.Play();

        // DOTweenで拡大＆回転アニメーション
        Sequence sequence = DOTween.Sequence();
        sequence.Append(badgeDisplay.DOScale(6.0f, 1.5f).SetEase(Ease.OutBack)); // 拡大
        sequence.Join(badgeDisplay.DORotate(new Vector3(0, 360f, 0), 0.2f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(5, LoopType.Restart)); // 回転を繰り返す

        sequence.AppendInterval(3.5f); // 表示時間
        sequence.Append(badgeDisplay.DOScale(0f, 0.5f).SetEase(Ease.InBack)); // 縮小

        sequence.OnComplete(() => {
            badgeDisplay.gameObject.SetActive(false);
            badgeGetDisplayPanel.gameObject.SetActive(false);
            UpdateBadgeUI();
        });
    }

    //バッジ取得した後のキラキラエフェクト。取得アニメーションのあとにキラキラさせたかったためコルーチンで処理しています。
    void ShowBadgeShiningEffect() {
        for (int i = 0; i < badges.Count; i++) {

            ParticleSystem badgeShiningEffect = badgeShiningEffects[i];

            badgeShiningEffect.Stop();

        }
        
    }

    // サウンドを再生するメソッド
    void PlayBadgeSound() {
        if (badgeGetSound != null && audioSource != null) {
            audioSource.PlayOneShot(badgeGetSound);
        }
    }

}
