using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{

    private void Start() {
        Invoke("LoadStage1Scene", 25.0f);
    }

    void LoadStage1Scene() {
        SceneManager.LoadScene("Stage1");
    }

}
