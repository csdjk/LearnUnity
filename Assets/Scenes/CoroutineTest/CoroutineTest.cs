using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试 协程 调用顺序 : 
/// fixedUpdate -> update -> coroutine -> lateUpdate
/// </summary>
public class CoroutineTest : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        StartCoroutine ("CoroutineFun");
    }

    void FixedUpdate () {
        Debug.Log ("FixedUpdate");
    }
    void Update () {
        Debug.Log ("Update");
    }

    void LateUpdate () {
        Debug.Log ("LateUpdate");
    }

    IEnumerator CoroutineFun () {
        Debug.Log ("This is Update Coroutine Call Before");
        yield return 1;
        Debug.Log ("This is Update Coroutine Call After");
    }
}