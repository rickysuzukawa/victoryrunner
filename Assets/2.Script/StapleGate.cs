using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StapleGate : MonoBehaviour
{
    private Rigidbody rb;

    private void Start() {

        rb = this.gameObject.GetComponent<Rigidbody>();

    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.tag == "AttackBigHand") {

            rb.isKinematic = false;

        }

    }
}
