using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

//キャラクター選択シーンを開いた時に最初にセレクトされているボタンを指定するためのクラス
public class CharacterSelectSceneManager : MonoBehaviour
{

    [SerializeField] GameObject boyButton;

    [SerializeField] GameObject girlButton;

    private void Awake() {

        int playerSelectNum = PlayerPrefs.GetInt("PlayerSelectNumber", 0);

        if (playerSelectNum == 0) {

            //プレイヤーは男の子。男の子ボタンを選択状態
            EventSystem.current.SetSelectedGameObject(boyButton);


        } else if (playerSelectNum == 1) {

            //プレイヤーは女の子。女の子ボタンを選択状態
            EventSystem.current.SetSelectedGameObject(girlButton);


        }

    }


}
