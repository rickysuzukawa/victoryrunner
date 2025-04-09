using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKeyControllerManager : MonoBehaviour
{

    private void Awake() {

        DontDestroyOnLoad(gameObject);

    }

#if UNITY_EDITOR
    void Update() {

        if (Input.GetKeyDown(KeyCode.Z)) {
            Time.timeScale /= 10;
        }

        if (Input.GetKeyUp(KeyCode.C)) {
            Time.timeScale *= 2;
        }

        if (Input.GetKeyDown(KeyCode.X)) {
            Time.timeScale = 1;
        }

        if (Input.GetKeyDown(KeyCode.Space)) {
            Time.timeScale = 0;
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            ReloadScene();
        }

    }

#endif

    public void ReloadScene() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
