using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveClamp : MonoBehaviour
{

    //移動範囲の制限値
    public float moveX_min_value;
    public float moveX_max_value;
    public float moveZ_min_value;
    public float moveZ_max_value;

    private void Update() {

        //移動範囲を制限
        Vector3 newPosition = transform.localPosition;
        newPosition.x = Mathf.Clamp(newPosition.x, moveX_min_value, moveX_max_value);
        newPosition.z = Mathf.Clamp(newPosition.z, -moveZ_min_value, moveZ_max_value);
        transform.localPosition = newPosition;

    }
}
