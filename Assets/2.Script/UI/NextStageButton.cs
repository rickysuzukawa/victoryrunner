using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageButton : MonoBehaviour
{
    private string nowSceneName;
    private string stageNumString;
    //現在のステージの数値
    private int currentStage;

    //最終ステージ数を設定
    //NextStageButtonを押した際にステージ1に戻るためのものです
    private int finalStageNum = 30;

    void Start()
    {
        GetStageNumber();

    }

    //NextStageButtonを押した際のアクション
    public void LoadNextScene() {

        Time.timeScale = 1.0f;

        //現在のステージ数と最終ステージを確認
        if (currentStage != finalStageNum) {

            //今のステージ数にプラス1をしてシーン遷移
            SceneManager.LoadScene("Stage" + (currentStage + 1));

        } else if (currentStage == finalStageNum) {

            //ClearedStageキーを1に戻す
            PlayerPrefs.SetInt("ClearedStage", 1);
            //ステージが最終ならばEndingシーンに遷移
            SceneManager.LoadScene("Ending");

        }

    }

    //現在のステージ番号を取得する
    void GetStageNumber() {

        nowSceneName = SceneManager.GetActiveScene().name;
        stageNumString = nowSceneName.Substring(5);
        currentStage = int.Parse(stageNumString);

    }
}
