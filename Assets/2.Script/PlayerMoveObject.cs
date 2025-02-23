using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//PlayerMoveObjectにアタッチしています。メインカメラとハンドの移動に関する親オブジェクトになります。
public class PlayerMoveObject : MonoBehaviour
{

    //起動時の位置
    private Vector3 launchPos;

    //プレイヤー情報格納用
    private GameObject player;
    [SerializeField] private PlayerSelectManager playerSelectManager;

    //相対距離取得用
    private Vector3 offset;

    [SerializeField]
    private Goal goalScript;

    void Start() {

        player = playerSelectManager.player;

        //初期の位置を設定
        launchPos = new Vector3(0f, 0f, 0f);
        transform.position = launchPos;

        //自分自身とplayerとの相対距離を求める
        offset = transform.position - player.transform.position;

    }

    void Update() {

        if (goalScript.isGoal == false) {

            //新しいトランスフォームの値を代入する
            transform.position = player.transform.position + offset;

        }

    }
}
