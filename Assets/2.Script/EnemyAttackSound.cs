using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttackSound : MonoBehaviour
{

    private AudioSource enemyAudioSource;

    [SerializeField] AudioClip[] enemyAttackSound;

    void Start()
    {

        enemyAudioSource = GetComponent<AudioSource>();

    }

    //手が敵などに当たった時のサウンドはPlayerAudioSound.csに処理を移行しました。
    //採用の場合はこのクラスは必要なくなります。その際は敵などのオブジェクトからAudio Sourceを取り除いてください
    private void OnCollisionEnter(Collision collision) {

        //if (collision.gameObject.tag == "AttackHand") {

        //    //enemyAttackSoundの配列0〜2を取り出したい
        //    if (enemyAttackSound.Length > 1) {

        //        //enemyAttackSound.Lengthが1以上の時
        //        int rndEnemyAttackSoundNumber = Random.Range(0, enemyAttackSound.Length);
        //        enemyAudioSource.PlayOneShot(enemyAttackSound[rndEnemyAttackSoundNumber]);

        //    } else if (enemyAttackSound.Length <= 1) {

        //        enemyAudioSource.PlayOneShot(enemyAttackSound[0]);

        //    }

        //}

    }
}
