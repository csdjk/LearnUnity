using System.Collections;
using System.Collections.Generic;
using Common;
using UnityEngine;

public class Gun : MonoBehaviour {

    public GameObject bulletPrefab;

    void Start () {
        // 
        // User go = new User ();
        // go.id = 111;
        // go.name = "zz";
        // Debug.Log (JsonHelper.ObjectToJson (go));
        // var a = JsonHelper.JsonToObject<User> ("{\"test\":\"1,\"id\":\"1}");
        // 
        Vector3[] arr = new Vector3[] { new Vector3(0,0,50), new Vector3(0,0,24), new Vector3(0,0,20),new Vector3(0,0,123) };
        var min = arr.GetMin (t => Vector3.Distance(Vector3.zero,t));
        Debug.Log ("min " + min.ToString());

    }
    public void Fire () {
        GameObejctPool.Instance.CreateObject ("bullet", bulletPrefab, transform.position, transform.rotation);
    }

    private void OnGUI () {
        if (GUILayout.Button("发射")) {
            Fire ();
        }
    }
}