using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ProgressBarController : MonoBehaviour
{
    public Image fillBar; // FillBarのImageコンポーネントを割り当て

    private int clearedStage;
    private string nextStage;

    private void Awake() {

        //ClearedStageキーに何も入っていなければ1を設定
        clearedStage = PlayerPrefs.GetInt("ClearedStage", 1);
        nextStage = $"Stage{clearedStage}";

    }

    void Start() {

        StartCoroutine(SimulateProgressAndLoadScene(nextStage)); // 読み込みたいシーン名を指定

    }

    IEnumerator SimulateProgressAndLoadScene(string sceneName) {
        // プログレスバーを5秒間かけて進める
        float duration = 5f; // 進行にかける時間
        float elapsed = 0f;  // 経過時間

        while (elapsed < duration) {
            elapsed += Time.deltaTime;
            float progress = Mathf.Clamp01(elapsed / duration); // 経過時間に応じて進捗を計算
            fillBar.fillAmount = progress; // プログレスバーを更新
            yield return null;
        }

        // プログレスバーが満タンになったらシーンをロード
        SceneManager.LoadScene(nextStage);
    }
}
