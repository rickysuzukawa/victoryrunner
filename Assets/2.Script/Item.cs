using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アイテムを取得した時のサウンド、およびアイテムの回転
public class Item : MonoBehaviour
{

    [SerializeField] ParticleSystem itemGetParticle;

    private bool getItem = false;

    [SerializeField] float rotationPower = 70f;
    [SerializeField] float movePower = 0.05f;

    private bool isOnce = false;

    private void Update() {

        if (getItem) {

            transform.Rotate(new Vector3(0f, rotationPower, 0f), Space.World);
            transform.position += new Vector3(0f, movePower, 0f);

        }

    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "AttackHand" || other.gameObject.tag == "AttackBigHand") {

            if (isOnce == false) {

                isOnce = true;
                ItemGetParticle();
                getItem = true;
                Invoke("ItemDestroy", 0.5f);

            }

        }

    }

    void ItemDestroy() {

        Destroy(this.gameObject);

    }


    void ItemGetParticle() {

        ParticleSystem newParticle = Instantiate(itemGetParticle);
        newParticle.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        newParticle.Play();
        Destroy(newParticle.gameObject, 5.0f);

    }

}
