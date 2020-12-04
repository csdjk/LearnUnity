using System.Collections;
using System.Collections.Generic;
using System.Linq;
using BezierUtils;
using UnityEngine;
public class BezierCurve
{
    public Transform[] pointsTransform;
    public Vector3[] points;
    public GameObject line;
    public BezierCurve nextCurve;
    private LineRenderer lineRenderer;
    private float sublineWidth = 0.03f;
    private List<LineRenderer> sublineRenderers = new List<LineRenderer>();
    public BezierCurve(Vector3[] points)
    {
        this.points = points;
        pointsTransform = new Transform[points.Length];

        line = new GameObject("line");
        lineRenderer = line.AddComponent<LineRenderer>();
        // 添加控制点
        for (int i = 0; i < points.Length; i++)
        {
            AddPoint(points[i], i);
        }
        // 添加辅助线
        AddSubLine(points[0], points[1]);
        AddSubLine(points[2], points[3]);
        // 
        UpdatePoints();
        SetWidth(0.1f);
        // SetMaterial(BezierEditor.lineMat);
    }

    private void AddPoint(Vector3 pos, int index)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.transform.localScale = new Vector3(0.1f,0.1f,0.1f);
        go.transform.position = pos;
        go.transform.parent = line.transform;
        pointsTransform[index] = go.transform;
    }

    private void AddSubLine(Vector3 start, Vector3 end)
    {
        var subline = new GameObject("subline");
        subline.transform.parent = line.transform;

        var sublineRenderer = subline.AddComponent<LineRenderer>();

        sublineRenderer.positionCount = 2;
        sublineRenderer.SetPositions(new Vector3[] { start, end });
        sublineRenderer.startWidth = sublineRenderer.endWidth = sublineWidth;
        sublineRenderer.material = new Material(Shader.Find("Particles/Standard Unlit"));
        sublineRenderer.startColor = sublineRenderer.endColor = Color.white;
        sublineRenderers.Add(sublineRenderer);
    }

    public void SetWidth(float lineWidth)
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        // lineRenderer.widthCurve = 
    }
    public void SetMaterial(Material lineMat)
    {
        if (lineMat == null)
        {
            lineMat = new Material(Shader.Find("Particles/Standard Unlit"));
        }
        lineRenderer.material = lineMat;
    }

    public void SetColor(Gradient startColor)
    {
        lineRenderer.colorGradient = startColor;
    }

    public void SetSublineColor(Color color)
    {
        foreach (var subline in sublineRenderers)
        {
            subline.startColor = color;
        }
    }

    public void Update()
    {
        if (!CheckPositionChanged())
        {
            return;
        }
        UpdatePoints();
        UpdateSubLine();
    }

    private void UpdatePoints()
    {
        List<BVector3> posArr = Bezier.Instance.CreateBezierList(points, 1, 20);
        if (lineRenderer)
        {
            lineRenderer.positionCount = posArr.Count;
            lineRenderer.SetPositions(posArr.Select((v) => { return v.pos; }).ToArray());
        }
    }

    private void UpdateSubLine()
    {
        sublineRenderers[0].SetPositions(new Vector3[] { points[0], points[1] });
        sublineRenderers[1].SetPositions(new Vector3[] { points[2], points[3] });
    }

    // 检测坐标是否变化
    private bool CheckPositionChanged()
    {
        bool isChange = false;
        for (var i = 0; i < points.Length; i++)
        {
            var newPos = pointsTransform[i].position;
            if (!points[i].Equals(newPos))
            {
                isChange = true;
                points[i] = newPos;
            }
        }
        return isChange;
    }

    public string Method(string parameter)
    {
        var list = new Stack()
    }
}
