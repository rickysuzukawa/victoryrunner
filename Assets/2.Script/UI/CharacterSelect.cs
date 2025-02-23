using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{

    private void Start() {

        //デフォルトは0(プレイヤーは男の子)
        PlayerPrefs.GetInt("PlayerSelectNumber", 0);

    }

    public void LeftButtonAction() {

        //男の子キャラを保存
        PlayerPrefs.SetInt("PlayerSelectNumber", 0);

    }

    public void RightButtonAction() {

        //女の子キャラを保存
        PlayerPrefs.SetInt("PlayerSelectNumber", 1);

    }

    public void StartGameButtonAction() {

        //ステージセレクトシーンに遷移したい場合
        //SceneManager.LoadScene("StageSelect");

        int clearedStage = PlayerPrefs.GetInt("ClearedStage", 1);

        string nextStage = $"Stage{clearedStage}";

        SceneManager.LoadScene(nextStage);

    }

}
