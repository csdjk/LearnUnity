using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BezierTest : MonoBehaviour
{
    public GameObject dot;
    // Start is called before the first frame update
    void Start()
    {
        Vector3 pos1 = new Vector3(1,0,0);
        Vector3 pos2 = new Vector3(0,1,0);
        Vector3 pos3 = new Vector3(-1,0,0);
        List<BVector3> posArr = Bezier.Instance.CreateBezierList(new Vector3[]{pos1,pos2,pos3},1,20);
        Vector3[] points = posArr.Select((v)=>{ return v.pos; }).ToArray();



        LineRenderer line = transform.GetComponent<LineRenderer>();
        line.startColor = Color.red;
        line.endColor = Color.blue;
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    void Update()
    {
        
    }
}
