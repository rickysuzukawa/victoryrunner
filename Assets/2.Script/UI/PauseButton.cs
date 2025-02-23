using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseButton : MonoBehaviour
{

    public GameObject pausePanel;

    private void Start() {

        pausePanel.SetActive(false);

    }

    /// <summary>
    /// ステージ選択ボタン押下時処理
    /// </summary>
    /// <param name="sceneID">該当ボスID</param>
    public void OnStageSelectButtonPressed(int sceneID) {   //お邪魔しています
		// シーン切り替え
		SceneManager.LoadScene(sceneID + 3);
	}

    // 一時停止ボタンが押されたときに呼ばれる
    public void OnPauseButtonPressed() {

        this.gameObject.SetActive(false);
        // 一時停止パネルを表示
        pausePanel.SetActive(true);
        Time.timeScale = 0f;
        //ここにゲームステータスPauseへの変更処理を入れる
        var gameStatusManager = FindObjectOfType<GameStatusManager>();
        gameStatusManager.StagePauseAction();
    }

}
