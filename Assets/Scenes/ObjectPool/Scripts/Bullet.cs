using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class Bullet : MonoBehaviour, IResetable {
    public float speed = 50f;
    private Vector3 targetPos;
    public void OnReset () {
        targetPos =  transform.TransformPoint (0,0,50);
        Debug.Log(targetPos.ToString());
    }

    void Update () {
        transform.position = Vector3.MoveTowards (transform.position, targetPos, Time.deltaTime * speed);

        if (Vector3.Distance (transform.position, targetPos) < 0.1) {
            GameObejctPool.Instance.CollectObejct (gameObject);
        }
    }
}


public class User{
    public int id = 10;
    public string name = "zz";

    public int test{
        get{
            return 1;
        }
    }
}