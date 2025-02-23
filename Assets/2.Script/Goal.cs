using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //パートナーのAnimatorを取得
    private GameObject partner;
    private Animator partnerAnimator;

    private GameObject wowParticle;

    public GameObject handObject;

    public GameStatusManager gameStatusManager;

    [SerializeField] ParticleSystem confettiParticle;

    public bool isGoal = false;

    private AudioSource audioGoalSource;

    [SerializeField] AudioClip goalSound;

    private void Start() {

        if (GameObject.Find("PartnerBoy(Clone)") != null) {

            partner = GameObject.Find("PartnerBoy(Clone)");

        }
        if (GameObject.Find("PartnerGirl(Clone)") != null) {

            partner = GameObject.Find("PartnerGirl(Clone)");

        }

        partnerAnimator = partner.GetComponent<Animator>();
        wowParticle = GameObject.Find("WOWparticle");
        wowParticle.SetActive(false);

        audioGoalSource = gameObject.GetComponent<AudioSource>();

    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "Player") {

            gameStatusManager.StageClearAction();
            confettiParticle.Play();
            isGoal = true;
            wowParticle.SetActive(true);
            handObject.SetActive(false);
            partnerAnimator.SetBool("isVictoryJump", true);

            audioGoalSource.PlayOneShot(goalSound);

        }

    }

}
