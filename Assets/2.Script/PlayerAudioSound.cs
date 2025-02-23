using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//手が敵などに当たった時のサウンドとバイブレーションを管理しています
public class PlayerAudioSound : MonoBehaviour
{
    private AudioSource playerAudioSource;

    [SerializeField] AudioClip[] audioClip;

    void Start()
    {

        playerAudioSource = GetComponent<AudioSource>();
        
    }

    private void OnCollisionEnter(Collision collision) {

        if (collision.gameObject.tag == "Enemy") {

            playerAudioSource.PlayOneShot(audioClip[0]);
            HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Medium);

        } else if (collision.gameObject.tag == "Obstacles") {

            playerAudioSource.PlayOneShot(audioClip[2]);
            HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Medium);

        }

    }

    private void OnTriggerEnter(Collider other) {

        if (other.gameObject.tag == "GoldCoin") {

            playerAudioSource.PlayOneShot(audioClip[1]);
            HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Medium);

        } else if (other.gameObject.tag == "ItemBigHand") {

            playerAudioSource.PlayOneShot(audioClip[3]);
            HapticFeedback.ImpactOccurred(ImpactFeedbackStyle.Medium);

        }
    }
}
