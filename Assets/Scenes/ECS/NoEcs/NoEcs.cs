using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoEcs : MonoBehaviour {
    public float rangePos = 20;
    public int Count = 10000;
    public GameObject Prefab;
    // Start is called before the first frame update
    void Start () {
        for (int i = 0, len = Count; i < len; i++) {
            GameObject go = Instantiate (Prefab);
            go.transform.position = new Vector3(Random.Range (-rangePos, rangePos),0,Random.Range (-rangePos, rangePos));
            go.GetComponent<MoveScript>().Speed = Random.Range (-10, 10);
        }
    }

}