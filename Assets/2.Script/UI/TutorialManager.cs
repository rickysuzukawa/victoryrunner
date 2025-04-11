using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{

    public GameObject tutorialPanel;

    private void Start() {

        Scene thisScene = SceneManager.GetActiveScene();

        if (thisScene.name == "Stage1") {
            //ここにゲームステータスPauseへの変更処理を入れる
            var gameStatusManager = FindObjectOfType<GameStatusManager>();
            gameStatusManager.StageReadyAction();

        } else {

            tutorialPanel.SetActive(false);

        }

    }

    //ボタンを押すとチュートリアル終了です
    public void TutorialStartAction() {

        tutorialPanel.SetActive(false);

    }
}
