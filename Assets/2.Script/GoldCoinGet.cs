using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoldCoinGet : MonoBehaviour {

    [SerializeField]
    [Tooltip("発生させるエフェクト(パーティクル)")]
    private ParticleSystem particle;

    private int coinPoint = 1;

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "AttackHand") {

            CoinGetCounter.instance.UpdateCoinDisplay();

            ParticleSystem newParticle = Instantiate(particle);
            newParticle.transform.position = this.transform.position;
            newParticle.Play();

            Destroy(newParticle.gameObject, 5.0f);

            Destroy(this.gameObject);

        }
    }

}
