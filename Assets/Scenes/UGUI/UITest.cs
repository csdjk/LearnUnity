using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UITest : MonoBehaviour {
    // Start is called before the first frame update
    void Start () {
        Stack<int> s = new Stack<int> ();
        s.Push (1);
        s.Push (2);
        s.Push (3);

        int[] a = s.ToArray ();
        foreach (var item in a) {
            Debug.Log (item);
        }
    }

}