using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStatusManager : MonoBehaviour
{

    public enum GameStatus {

        Ready,
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

    private void Awake() {

        ChangeStatus(GameStatus.Ready);

    }

    private void Start() {

        Application.targetFrameRate = 60;


        //チュートリアルを表示させるためステージ1だけすぐにプレイにしてない
        //if (currentStagenameScene != "Stage1") {

        //    ChangeStatus(GameStatus.Play);

        //}

        string currentStagenameScene = SceneManager.GetActiveScene().name;
        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlayBGMForStage(currentStagenameScene);
        }

    }

    private void Update() {

        Debug.Log(CurrentStatus);

    }

    public void StageReadyAction() {

        ChangeStatus(GameStatus.Ready);
        Time.timeScale = 1.0f;

    }

    public void StagePlayAction() {

        ChangeStatus(GameStatus.Play);
        Time.timeScale = 1.0f;

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

        ChangeStatus(GameStatus.Pause);
        Time.timeScale = 0f;

    }

    public void RestartScene() {

        Scene thisScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(thisScene.name);
        Time.timeScale = 1.0f;

    }

    void StageClearPanelShow() {

        stageClearPanel.SetActive(true);

    }

    // ステータスを変更するメソッド
    public void ChangeStatus(GameStatus newStatus) {
        CurrentStatus = newStatus;
    }

}
