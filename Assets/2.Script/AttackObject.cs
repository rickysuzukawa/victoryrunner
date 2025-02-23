using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Handオブジェクトにアタッチしています
public class AttackObject : MonoBehaviour {

    //UnityEditor上のマウス移動感度
    public float mouse_sensitivity = 1300;

    //スマートフォン上のタッチ移動感度
    public float touch_sensitivity = 15;

    //移動範囲の制限値
    private float moveX_clamp_value = 1.5f;
    private float moveZ_min_value = 0.7f;
    private float moveZ_max_value = 13.0f;

    //回転の強さ
    private float rotationPower;
    // y軸の角度範囲制限の最小値
    private float minAngleX = -20;
    // y軸の角度範囲制限の最大値
    private float maxAngleX = 20;
    //回転の範囲制限のため
    private float currentAngleY;

    //自身のRigidbody
    private Rigidbody rb;

    private PlayerMove playerMove;

    private void Start() {

        rb = GetComponent<Rigidbody>();

        if (GameObject.Find("PlayerBoy(Clone)") != null) {

            playerMove = GameObject.Find("PlayerBoy(Clone)").GetComponent<PlayerMove>();

        }

        if (GameObject.Find("PlayerGirl(Clone)") != null) {

            playerMove = GameObject.Find("PlayerGirl(Clone)").GetComponent<PlayerMove>();

        }

    }

    private void Update() {

#if UNITY_EDITOR

        if (Input.GetMouseButton(0)) {

            //キーボード用
            float dx = Input.GetAxis("Mouse X");
            float dy = Input.GetAxis("Mouse Y");

            Vector3 force = new Vector3(dx, 0, dy) * mouse_sensitivity;

            rb.AddForce(force, ForceMode.Force);

            rotationPower = 50f;
            //ドラッグした分だけ回転反映
            this.transform.Rotate(new Vector3(0f, dx * rotationPower, 0f));

        }

#endif


#if UNITY_IOS

        //スマホタッチの場合
        if (Input.touchCount > 0) {

            Touch touchPos = Input.GetTouch(0);

            // 移動していたら移動量を取得してオブジェクトを移動
            if (TouchPhase.Moved == touchPos.phase) {

                float deltaX, deltaY;

                deltaX = Input.touches[0].deltaPosition.x;
                deltaY = Input.touches[0].deltaPosition.y;

                //ドラッグした分だけ移動反映
                Vector3 force = new Vector3(deltaX, 0, deltaY) * touch_sensitivity;
                rb.AddForce(force, ForceMode.Force);

                rotationPower = 0.4f;

                //ドラッグした分だけ回転反映
                this.transform.Rotate(new Vector3(0f, deltaX * rotationPower, 0f));

            }

        }

#endif

        //移動範囲を制限
        Vector3 newPosition = transform.localPosition;
        newPosition.x = Mathf.Clamp(newPosition.x, -moveX_clamp_value, moveX_clamp_value);
        newPosition.z = Mathf.Clamp(newPosition.z, -moveZ_min_value, moveZ_max_value);
        transform.localPosition = newPosition;

        ////手の回転角度範囲をClamp制限
        currentAngleY = this.transform.localEulerAngles.y;

        if (currentAngleY > 180) {

            currentAngleY = currentAngleY - 360;

        }

        currentAngleY = Mathf.Clamp(currentAngleY, minAngleX, maxAngleX);
        this.transform.localEulerAngles = new Vector3(0f, currentAngleY, 70f);

    }

}
