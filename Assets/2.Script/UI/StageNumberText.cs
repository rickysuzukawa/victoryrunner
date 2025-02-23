using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class StageNumberText : MonoBehaviour
{

    private TextMeshProUGUI stageText;

    private string nowSceneName;
    private string stageNumString;
    private int stageNum;

    private void Start() {

        GetStageNumber();

        stageText = gameObject.GetComponent<TextMeshProUGUI>();

        stageText.text = "STAGE " + stageNum;

    }

    //現在のステージ番号を取得する
    void GetStageNumber() {

        nowSceneName = SceneManager.GetActiveScene().name;
        stageNumString = nowSceneName.Substring(5);
        stageNum = int.Parse(stageNumString);

    }
}
