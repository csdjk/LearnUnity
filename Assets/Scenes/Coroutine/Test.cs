using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试 自定义的协程
/// </summary>
public class Test : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        var t1 = Test01 ();
        var t2 = Test02 ();
        CoroutineManager.Instance.StartCoroutine (t1);
        CoroutineManager.Instance.StartCoroutine (t2);
    }

    void Update()
    {
        CoroutineManager.Instance.UpdateCoroutine ();
    }

    IEnumerator Test01 () {
        Debug.Log ("start test 01");
        yield return new WaitForSeconds (5);
        Debug.Log ("after 5 seconds");
        yield return new WaitForSeconds (5);
        Debug.Log ("after 10 seconds");
    }

    IEnumerator Test02 () {
        Debug.Log ("start test 02");
        yield return new WaitForFrames (500);
        Debug.Log ("after 500 frames");
    }
}