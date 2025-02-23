using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//ステージクリア数を保存するためのクラスです
public class StageClearManager : MonoBehaviour
{
    public static StageClearManager instance;

    //現在のステージ数を設定
    private int currentStage;

    private void Awake() {

        if (instance == null) {

            instance = this;
            DontDestroyOnLoad(this.gameObject);

        } else {

            Destroy(this.gameObject);

        }

    }

    //シーンがロードされた時に呼び出されます
    //現在のステージ数を取得するためです。ゴールした時にこのステージ数にプラス1をしてステージクリア数を保存しています。
    private void OnEnable() {

        SceneManager.sceneLoaded += OnSceneLoaded;

    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {

        CurrentStageNumberGet();

    }

    //PlayerMove.csからゴールした時に呼び出されています。現在のステージ数にプラス1しています
    public void CompleteStage() {

        int nextStage = currentStage + 1;
        PlayerPrefs.SetInt("ClearedStage", nextStage);
        PlayerPrefs.Save();

    }

    //プレイ中の現在のステージ数を取得
    void CurrentStageNumberGet() {

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;

        //ステージID(currentSceneIndex)が3以上ならステージ数を取得。タイトルシーンだと5文字以上無いからエラーになる。
        //シーンインデックスが変わるとここも変えなければなりません。
        if (currentSceneIndex >= 3) {
            string nowSceneName = SceneManager.GetActiveScene().name;
            string stageNumString = nowSceneName.Substring(5);
            currentStage = int.Parse(stageNumString);
        }

    }

}
