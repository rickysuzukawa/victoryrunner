using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CoinGetCounter : MonoBehaviour
{

    public static CoinGetCounter instance;
    public TMP_Text coinCounterText;
    private int totalCoin = 0;
    private int maxCoinValue = 999;

    void Awake() {

        if (instance == null) {

            instance = this;

        } else {

            Destroy(gameObject);

        }

    }

    //void Start() {

    //    totalCoin = PlayerPrefs.GetInt("TotalCoin");
    //    coinCounterText.text = $"<sprite=0> {totalCoin.ToString()}";

    //}

    public void UpdateCoinDisplay() {

        if (totalCoin >= maxCoinValue) return;

        totalCoin = PlayerPrefs.GetInt("TotalCoin");
        totalCoin += 1;
        coinCounterText.text = $"<sprite=0> {totalCoin.ToString()}";
        PlayerPrefs.SetInt("TotalCoin", totalCoin);

    }

}
