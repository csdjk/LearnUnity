using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GC_UnityFunCall : MonoBehaviour {

    //----------------------------未优化-----------------------------------
    public Mesh myMesh;
    void ExampleFunction () {
        for (int i = 0; i < myMesh.normals.Length; i++) {
            Vector3 normal = myMesh.normals[i];
        }
    }
    //----------------------------未优化-----------------------------------

    //----------------------------优化-----------------------------------
    void ExampleFunction_O () {
        Vector3[] meshNormals = myMesh.normals;
        for (int i = 0; i < meshNormals.Length; i++) {
            Vector3 normal = meshNormals[i];
        }
    }
    //----------------------------优化-----------------------------------

    private string playerTag = "Player";
    void OnTriggerStay (Collider other) {
        bool isPlayer = other.gameObject.tag == playerTag;
        Debug.Log(isPlayer);
    }

    private string playerTag_O = "Player";
    // void OnTriggerStay (Collider other) {
    //     bool isPlayer = other.gameObject.CompareTag (playerTag_O);
    //     Debug.Log(isPlayer);
    // }

    void Start () { }
    void Update () {
        // ExampleFunction ();
        // ExampleFunction_O ();
    }
}