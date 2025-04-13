using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinGetCounter : MonoBehaviour
{

    public static CoinGetCounter instance;
    public TMP_Text coinCounterText;
    private int totalCoin = 0;   //初期値は必ず0です
    private int maxCoinValue = 9999;

    void Awake() {

        if (instance == null) {

            instance = this;

        } else {

            Destroy(gameObject);

        }

        //テスト用の2行。totalCoinが最大値になった時の挙動確認用です。必ずコメントアウトすること！！
        //PlayerPrefs.SetInt("TotalCoin", totalCoin);
        //totalCoin = PlayerPrefs.GetInt("TotalCoin");
    }

    public void UpdateCoinDisplay() {

        if (totalCoin >= maxCoinValue) return;

        totalCoin = PlayerPrefs.GetInt("TotalCoin");
        totalCoin += 1;
        coinCounterText.text = $"<sprite=0> {totalCoin.ToString()}";
        PlayerPrefs.SetInt("TotalCoin", totalCoin);

    }

}
