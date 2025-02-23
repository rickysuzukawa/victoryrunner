using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//このクラスはプレイヤーを生成するためにあります
//キャラクター選択によりプレイヤーのキャラが変わります

public class PlayerSelectManager : MonoBehaviour
{

    [SerializeField] GameObject[] playerPrefabs;
    [SerializeField] GameObject[] partnerPrefabs;

    //playerオブジェクトはMainCameraMove.csからプレイヤー追従するためにpublicでなければいけません
    public GameObject player;
    private Transform playerTrans;

    public GameObject partner;
    private Transform partnerTrans;

    [SerializeField] Vector3 playerFirstPosition;
    private Vector3 partnerFirstPosition;

    [SerializeField] Transform goalPosition;

    private void Awake() {

        int playerSelectNum = PlayerPrefs.GetInt("PlayerSelectNumber", 0);
        //プレイヤーを生成。開始位置、回転を設定
        player = Instantiate(playerPrefabs[playerSelectNum]);
        playerTrans = player.transform;
        playerTrans.position = playerFirstPosition;
        Vector3 angle = playerTrans.eulerAngles;
        angle.x = 0f;
        angle.y = 0f;
        angle.z = 0f;
        playerTrans.eulerAngles = angle;

        if (playerSelectNum == 0) {

            //パートナーは女の子を生成
            partner = Instantiate(partnerPrefabs[1]);


        } else if (playerSelectNum == 1) {

            //パートナーは男の子を生成
            partner = Instantiate(partnerPrefabs[0]);

        }

        partnerFirstPosition = new Vector3(0.5f, 0f, goalPosition.position.z + 5.0f);

        //パートナーの立ち位置、回転を設定
        partnerTrans = partner.transform;
        partnerTrans.position = partnerFirstPosition;
        Vector3 partnerAngle = partnerTrans.eulerAngles;
        partnerAngle.x = 0f;
        partnerAngle.y = -180f;
        partnerAngle.z = 0f;
        partnerTrans.eulerAngles = partnerAngle;

    }

}
