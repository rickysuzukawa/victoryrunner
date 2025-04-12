using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TapStartPanel : MonoBehaviour, IPointerClickHandler {

    private bool isOnce;

    [SerializeField]
    private GameObject pauseBtn;

    public GameObject dashBoardButton;

    void Start() {

        isOnce = false;

        pauseBtn.SetActive(false);

    }


    public void OnPointerClick(PointerEventData eventData) {

        if (isOnce) return; // すでにスタートしていたら無視

        GameObject clickedObj = eventData.pointerEnter;

        // このパネル自身 またはその子要素がタッチされた場合のみスタート
        if (clickedObj == null || (!clickedObj.transform.IsChildOf(transform) && clickedObj != gameObject)) {
            return;
        }

        var gameStatusManager = FindObjectOfType<GameStatusManager>();
        gameStatusManager.StagePlayAction();

        isOnce = true;
        pauseBtn.SetActive(true);
        gameObject.SetActive(false); // パネルを非表示
    }
}
