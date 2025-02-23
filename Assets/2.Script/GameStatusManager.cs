using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatusManager : MonoBehaviour
{

    public enum GameStatus {
        Play,
        GameOver,
        Goal,
        Pause,
    }

    // 現在のゲームステータスを格納
    public GameStatus CurrentStatus { get; private set; } = GameStatus.Play;

    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject stageClearPanel;
    [SerializeField] GameObject pauseButton;

    public float clearPanelShowDelayTime = 4.0f;

    private void Start() {

        Application.targetFrameRate = 60;
        Scene thisScene = SceneManager.GetActiveScene();
        if (thisScene.name != "Stage1") {

            ChangeStatus(GameStatus.Play);

        }

    }

    public void StagePlayAction() {

        //pauseButton.SetActive(true);
        ChangeStatus(GameStatus.Play);

    }

    public void GameOverAction() {

        pauseButton.SetActive(false);
        gameOverPanel.SetActive(true);
        ChangeStatus(GameStatus.GameOver);

    }

    //GoalオブジェクトのGoal.csから呼び出しています
    public void StageClearAction() {

        pauseButton.SetActive(false);
        Invoke("StageClearPanelShow", clearPanelShowDelayTime);
        //ChangeStatus(GameStatus.Goal);

    }

    public void StagePauseAction() {

        //pauseButton.SetActive(false);
        ChangeStatus(GameStatus.Pause);

    }

    public void RestartScene() {

        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);

    }

    void StageClearPanelShow() {

        stageClearPanel.SetActive(true);

    }

    // ステータスを変更するメソッド
    public void ChangeStatus(GameStatus newStatus) {
        CurrentStatus = newStatus;
    }

}
