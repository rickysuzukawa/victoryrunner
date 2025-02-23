using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraMove : MonoBehaviour
{

    //プレイヤー情報格納
    private GameObject player;
    [SerializeField] private PlayerSelectManager playerSelectManager;

    [SerializeField]
    private Goal goalScript;

    // ゴール時のカメラ目標角度
    private Vector3 targetRotationEuler = new Vector3(9.0f, 0f, 0f);
    // 補間速度
    private float rotationSpeed = 2.0f;
    // 目標の回転（クォータニオン）
    private Quaternion targetRotation;

    void Start() {

        player = playerSelectManager.player;

        //初期のカメラ角度を設定
        transform.eulerAngles = new Vector3(35f, 0f, 0f);

        // 目標の回転をクォータニオンに変換
        targetRotation = Quaternion.Euler(targetRotationEuler);

    }

    void LateUpdate() {

        //ゴール時のカメラ移動およびアングル変更処理
        if (goalScript.isGoal == true) {

            //カメラをスムーズにプレイヤーに近づける
            transform.position = Vector3.Lerp(transform.position, player.transform.position + new Vector3(0f, 1.0f,-2.5f), Time.deltaTime * 2.0f);
            //カメラをスムーズに回転(上に向かせる)
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);


        }

    }

}
