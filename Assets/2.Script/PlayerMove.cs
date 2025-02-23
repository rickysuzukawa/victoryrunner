using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//少年少女(PlayerBoy,PlayerGirl)にアタッチしています。
public class PlayerMove : MonoBehaviour
{

    private Rigidbody playerRb;

    //前に進むスピード
    private float moveSpeed = 15.0f;

    //移動の制限速度
    private float limitSpeed = 5.0f;

    private GameObject gameStatusManager;
    private GameStatusManager gameStatusManagerScript;

    private GameObject attackHand;

    private bool isDead = false;

    //敵に当たった時のプレイヤーのくるくる回る強さ
    private float deadRotationPower = 2000f;

    //プレイヤーが死んだ時のパーティクル
    [SerializeField] ParticleSystem deadParticle;
    //走る時のパーティクル
    [SerializeField] ParticleSystem runSmokeParticle;
    //スピードアップのパーティクル
    [SerializeField] public ParticleSystem speedUpParticle;

    private Animator playerAnimator;

    private AudioSource playerAudioSource;
    [SerializeField] AudioClip gameOverSound;

    private bool isOnce= false;

    void Start() {

        playerRb = this.gameObject.GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();
        attackHand = GameObject.Find("BlackHand");
        gameStatusManager = GameObject.Find("GameStatusManager");
        gameStatusManagerScript = gameStatusManager.GetComponent<GameStatusManager>();

        playerAudioSource = gameObject.GetComponent<AudioSource>();

    }

    void Update() {

        
        
        //プレイヤーの移動処理
        var playerVelocity = playerRb.velocity.magnitude;
        
        if (playerVelocity <= limitSpeed && gameStatusManagerScript.CurrentStatus == GameStatusManager.GameStatus.Play) {

            //速度制限をかけます。通常時の動作
            playerRb.AddForce(new Vector3(0, 0, moveSpeed));

        }

        if (isDead) {

            // transformを取得
            Transform myTransform = this.transform;

            float deadRotationValue = deadRotationPower * Time.deltaTime;
            myTransform.Rotate(0f, deadRotationValue, 0f);

        }

    }


    private void OnCollisionEnter(Collision other) {

        if (other.gameObject.tag == "Enemy") {

            GameOverAction();

            if (runSmokeParticle != null) {

                Destroy(runSmokeParticle.gameObject);

            }

        }

    }


    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Partner") {

            //プレイヤーがゴールにいるパートナーに触れたらアニメーション実行
            PlayerGoalAnimation();


        } else if (other.gameObject.tag == "Goal") {

            HapticFeedback.NotificationOccurred(NotificationFeedbackStyle.Success);
            runSmokeParticle.Stop();

            //ステージクリア数のセーブ
            StageClearManager.instance.CompleteStage();

        } else if (other.gameObject.tag == "Enemy") {

            GameOverAction();

            if (runSmokeParticle != null) {

                Destroy(runSmokeParticle.gameObject);

            }

        }

    }

    void PlayerDeadAction() {

        this.gameObject.SetActive(false);

    }

    void GameOverAction() {

        //当たるたびに何度も実行されるのを防ぐためisOnceで切り分けています
        if (!isOnce) {

            Time.timeScale = 0.2f;

            isOnce = true;
            HapticFeedback.NotificationOccurred(NotificationFeedbackStyle.Error);
            playerAudioSource.PlayOneShot(gameOverSound);
            attackHand.SetActive(false);
            playerRb.isKinematic = true;
            isDead = true;

            gameStatusManagerScript.GameOverAction();

            DeadParticle();
            Invoke("PlayerDeadAction", 1.0f);

        }

        Collider col = GetComponent<Collider>();
        if (col != null) {

            col.enabled = false;

        }

    }

    void DeadParticle() {

        ParticleSystem newParticle = Instantiate(deadParticle);
        newParticle.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        newParticle.Play();
        Destroy(newParticle.gameObject, 5.0f);

    }

    void PlayerGoalAnimation() {

        moveSpeed = 0f;
        playerRb.isKinematic = true;
        playerAnimator.SetBool("isVictoryJump", true);

        // transformを取得
        Vector3 playerRotation = gameObject.transform.localEulerAngles;
        playerRotation.y = 180f;
        gameObject.transform.localEulerAngles = playerRotation;

    }

}
