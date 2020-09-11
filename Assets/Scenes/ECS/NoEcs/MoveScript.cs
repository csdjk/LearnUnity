using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {
    public float Speed = 0.01f;

    // Update is called once per frame
    void Update () {
        Vector3 pos = transform.position;
        transform.Translate (new Vector3 (0, 0, Speed * Time.deltaTime));
        if (transform.position.z > 20) {
            Speed = -Math.Abs (Speed);
        }
        if (transform.position.z < -20) {
            Speed = +Math.Abs (Speed);
        }
    }
}