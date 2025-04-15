using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DebugKeyControllerManager : MonoBehaviour
{

    public int num = 1;

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

        if (Input.GetKeyDown(KeyCode.Q)) {

            SceneManager.LoadScene("Stage" + (1 + num));

        }

        if (Input.GetKeyDown(KeyCode.W)) {

            SceneManager.LoadScene("Stage" + (2 + num));

        }

        if (Input.GetKeyDown(KeyCode.E)) {

            SceneManager.LoadScene("Stage" + (3 + num));

        }

        if (Input.GetKeyDown(KeyCode.R)) {

            SceneManager.LoadScene("Stage" + (4 + num));

        }

        if (Input.GetKeyDown(KeyCode.T)) {

            SceneManager.LoadScene("Stage" + (5 + num));

        }

        if (Input.GetKeyDown(KeyCode.Y)) {

            SceneManager.LoadScene("Stage" + (6 + num));

        }

        if (Input.GetKeyDown(KeyCode.U)) {

            SceneManager.LoadScene("Stage" + (7 + num));

        }

        if (Input.GetKeyDown(KeyCode.I)) {

            SceneManager.LoadScene("Stage" + (8 + num));

        }

        if (Input.GetKeyDown(KeyCode.O)) {

            SceneManager.LoadScene("Stage" + (9 + num));

        }

    }

#endif

    public void ReloadScene() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

}
