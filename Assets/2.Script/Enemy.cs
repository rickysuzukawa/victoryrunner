using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Rigidbody enemyRg;

    //移動の制限速度
    public float limitSpeed = 2.0f;

    public Vector3 moveVector;

    public float knockBackPower;

    //ここからジグザグ移動関連
    public bool zigzagOn;

    public bool lookAtAlwaysForward = false;
    // 移動速度
    public float speed = 5f;
    // ジグザグの距離（方向を反転するまでの移動距離）
    public float zigzagDistance = 3f;

    // 内部変数
    private Vector3 direction = Vector3.right; // 初期方向は右
    private float distanceTravelled = 0f; // 移動した距離

    [SerializeField] ParticleSystem attackParticle;

    private ParticleSystem attackHitParticle;

    private GameStatusManager gameStatusManager;

    void Start()
    {

        enemyRg = this.gameObject.GetComponent<Rigidbody>();

        // GameStatusManager を取得
        gameStatusManager = FindObjectOfType<GameStatusManager>();

        if (gameStatusManager == null) {
            Debug.LogError("GameStatusManager が見つかりません！");
        }

    }

    void Update()
    {

        var enemyVelocity = enemyRg.velocity.magnitude;

        //速度制限をかけます
        if (enemyVelocity <= limitSpeed && gameStatusManager.CurrentStatus == GameStatusManager.GameStatus.Play) {

            enemyRg.AddForce(moveVector);

        }

        if (zigzagOn == true && enemyRg.isKinematic == false) {

            // オブジェクトを移動させる
            Vector3 newPosition = enemyRg.position + direction * speed * Time.deltaTime;
            enemyRg.MovePosition(newPosition);

            // 移動した距離を累積する
            distanceTravelled += speed * Time.deltaTime;

            if (!lookAtAlwaysForward) {
                // オブジェクトの向きを進行方向に向かせる
                enemyRg.MoveRotation(Quaternion.LookRotation(direction));
            }

            // 移動した距離がジグザグの距離に達した場合、方向を反転させる
            if (distanceTravelled >= zigzagDistance) {
                // 方向を反転させる
                direction = -direction;
                // 移動した距離をリセットする
                distanceTravelled = 0f;
            }


            //// Transform移動なので一旦コメントアウト。上のRigidbody移動に変更しました。オブジェクトを移動させる
            //transform.Translate(direction * speed * Time.deltaTime, Space.World);
            //// 移動した距離を累積する
            //distanceTravelled += speed * Time.deltaTime;

            //if (lookAtAlwaysForward == false) {

            //    // オブジェクトの向きを進行方向に向かせる
            //    transform.rotation = Quaternion.LookRotation(direction);

            //}

        }

        if (attackHitParticle != null) {

            attackHitParticle.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        }


    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.tag == "AttackHand") {

            //手にアタックされたら敵をノックバック
            Vector3 distination = (transform.position - collision.transform.position).normalized;
            enemyRg.AddForce(distination * knockBackPower, ForceMode.VelocityChange);
            //AttackParticle();

        }

    }

    void AttackParticle() {

        attackHitParticle = Instantiate(attackParticle);
        attackHitParticle.Play();
        Destroy(attackHitParticle.gameObject, 5.0f);


    }

}
