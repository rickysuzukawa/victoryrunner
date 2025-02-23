using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardParticleBlink : MonoBehaviour {

    //public float blinkSpeed = 1f; // 点滅速度
    //private Renderer particleRenderer; // パーティクルのレンダラー
    //private float targetAlpha = 1f; // 目標のアルファ値
    //private float currentAlpha = 1f; // 現在のアルファ値

    //void Start() {
    //    particleRenderer = GetComponent<Renderer>(); // パーティクルのレンダラーを取得
    //}

    //void Update() {
    //    // アルファ値を更新して点滅させる
    //    currentAlpha = Mathf.Lerp(currentAlpha, targetAlpha, Time.deltaTime * blinkSpeed);
    //    Color color = particleRenderer.material.color;
    //    color.a = currentAlpha;
    //    particleRenderer.material.color = color;

    //    // アルファ値が1または0に近づいたら反転させる
    //    if (currentAlpha >= 0.95f) targetAlpha = 0f;
    //    else if (currentAlpha <= 0.05f) targetAlpha = 1f;
    //}

}
