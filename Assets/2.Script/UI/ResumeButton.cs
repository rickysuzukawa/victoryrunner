using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject pauseButton;

    // 再開ボタンが押されたときに呼ばれる
    public void OnResumeButtonPressed() {

        // 一時停止パネルを非表示
        pausePanel.SetActive(false);
        pauseButton.SetActive(true);
        // ゲームの時間を再開
        Time.timeScale = 1f; // ゲームを再開
        var gameStatusManager = FindObjectOfType<GameStatusManager>();
        gameStatusManager.StagePlayAction();
    }


}
