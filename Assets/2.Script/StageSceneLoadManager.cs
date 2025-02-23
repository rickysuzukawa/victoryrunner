using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSceneLoadManager : MonoBehaviour
{

    void Start()
    {

        Invoke("StageSceneAutoLoad", 5.0f);
        
    }

    public void StageSceneAutoLoad() {

        //ステージセレクトシーンに遷移したい場合
        //SceneManager.LoadScene("StageSelect");

        //ClearedStageキーに何も入っていなければ1を設定
        int clearedStage = PlayerPrefs.GetInt("ClearedStage", 1);

        string nextStage = $"Stage{clearedStage}";

        SceneManager.LoadScene(nextStage);

    }
}
