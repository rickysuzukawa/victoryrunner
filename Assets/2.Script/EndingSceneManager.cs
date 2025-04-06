using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingSceneManager : MonoBehaviour
{

    private void Start() {
        Invoke("LoadStage1Scene", 25.0f);

        string currentStageName = SceneManager.GetActiveScene().name;

        if (AudioManager.Instance != null) {
            AudioManager.Instance.PlayBGMForStage(currentStageName);
        }
    }

    void LoadStage1Scene() {
        SceneManager.LoadScene("Stage1");
    }

}
