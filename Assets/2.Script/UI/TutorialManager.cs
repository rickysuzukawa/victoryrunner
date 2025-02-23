using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public GameObject tutorialPanel;

    //まず最初は画面をストップします
    private void Awake() {

        Scene thisScene = SceneManager.GetActiveScene();

        if (thisScene.name == "Stage1") {

            Time.timeScale = 0f;

        }

    }

    private void Start() {

        Scene thisScene = SceneManager.GetActiveScene();

        if (thisScene.name == "Stage1") {
            //ここにゲームステータスPauseへの変更処理を入れる
            var gameStatusManager = FindObjectOfType<GameStatusManager>();
            gameStatusManager.StagePauseAction();

        } else {

            tutorialPanel.SetActive(false);

        }

    }

    //ボタンを押すとチュートリアル終了です
    public void TutorialStartAction() {

        tutorialPanel.SetActive(false);
        var gameStatusManager = FindObjectOfType<GameStatusManager>();
        gameStatusManager.StagePlayAction();
        Time.timeScale = 1.0f;

    }
}
