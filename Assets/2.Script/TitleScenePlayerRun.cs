using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleScenePlayerRun : MonoBehaviour
{
    private Rigidbody playerRb;

    //前に進むスピード
    private float moveSpeed = 15.0f;

    //移動の制限速度
    private float limitSpeed = 5.0f;
    //走る時のパーティクル
    [SerializeField] ParticleSystem runSmokeParticle;

    void Start() {

        playerRb = this.gameObject.GetComponent<Rigidbody>();

    }

    void Update() {

        //プレイヤーの移動処理
        var playerVelocity = playerRb.velocity.magnitude;

        if (playerVelocity <= limitSpeed) {

            playerRb.AddForce(new Vector3(0, 0, moveSpeed));

        }

    }
}
