using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//このクラスは特定のオブジェクトのRigidbodyを取得して移動可能とするトリガーを実装しています
//プレイヤーがこのオブジェクトのColliderに衝突したら特定にオブジェクトのisKinematicをオフにして動かします
public class MoveTrigger : MonoBehaviour
{

    public List<Rigidbody> moveObjRb;

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Player") {

            foreach (Rigidbody enemy in moveObjRb) {

                enemy.isKinematic = false;

            }


        }

    }

}
