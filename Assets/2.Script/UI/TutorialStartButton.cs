using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TutorialStartButton : MonoBehaviour
{

    public Button buttonObject;

    private void Start() {

        if (buttonObject != null ) {

            RectTransform buttonTransform = buttonObject.GetComponent<RectTransform>();// 拡大縮小アニメーション
            buttonTransform.DOScale(new Vector3(1.1f, 1.1f, 1.1f), 0.5f) // 拡大
                .SetEase(Ease.InOutSine) // 緩やかなアニメーション
                .SetLoops(-1, LoopType.Yoyo) // 無限ループで拡大・縮小を繰り返す
                .SetUpdate(true); // タイムスケールの影響を受けない

        } else {

            Debug.Log("ボタンが設定されていません");
        }

        

    }
}
