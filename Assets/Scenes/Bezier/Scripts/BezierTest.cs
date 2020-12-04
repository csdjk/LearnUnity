using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using BezierUtils;

public class BezierTest : MonoBehaviour
{
    public GameObject dot;
    public GameObject player;
    public GameObject player1;
    public Vector3[] cotrollerPoints;

    float duration = 2f;

    // Start is called before the first frame update
    void Start()
    {
        // Vector3 pos1 = new Vector3(-1, 0, 0);
        // Vector3 pos2 = new Vector3(-0.5f, 1, 0);
        // Vector3 pos3 = new Vector3(0.5f, 1, 0);
        // Vector3 pos4 = new Vector3(1, 0, 0);

        cotrollerPoints = new Vector3[] {
            new Vector3(-1, 0, 0),
            new Vector3(-1, 0.5f, 0),
            new Vector3(-0.5f, 1, 0),
            new Vector3(0f, 1, 0),
            new Vector3(0.5f, 1, 0),
            new Vector3(1, 0, 0)};

        List<BVector3> posArr = Bezier.Instance.CreateBezierList(cotrollerPoints, 1, 20);
        Vector3[] points = posArr.Select((v) => { return v.pos; }).ToArray();



        LineRenderer line = transform.GetComponent<LineRenderer>();
        line.startColor = Color.red;
        line.endColor = Color.blue;
        line.positionCount = points.Length;
        line.SetPositions(points);
    }

    private void OnGUI()
    {
        if (GUILayout.Button("播放", GUILayout.Height(20)))
        {
            StartCoroutine(MovePath());
            player1.transform.position = cotrollerPoints[3];
            player1.transform.DOPath(cotrollerPoints, duration, PathType.CubicBezier).SetEase(Ease.Linear);
        }
    }

    private IEnumerator MovePath()
    {
        for (float timeCount = 0; timeCount < duration; timeCount += Time.deltaTime)
        {
            player.transform.position = Bezier.Instance.ComputeBezierPoint(timeCount, duration).pos;
            Debug.Log("当前时间：" + timeCount);
            yield return 0;
        }
    }
}
