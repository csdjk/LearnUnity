using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;

//-----------------------------【贝塞尔曲线规划工具】-----------------------------
public class BezierEditor : EditorWindow
{
    private GameObject line;
    private LineRenderer lineRenderer;
    private GameObject point;
    // 线宽度
    private float lineWidth = 0.1f;
    private Material lineMat;

    private List<Transform> pointArr;

    private Vector3 startPoint;
    private Vector3 centerPoint;
    private Vector3 endPoint;
    private List<BVector3> posArr;

    public float runTime = 5;
    // public float HeadSpace = 10;


    // 菜单选项目录
    [MenuItem("长生但酒狂的插件/贝塞尔曲线工具")]
    static public void OpenBezierEditor()
    {
        EditorWindow.CreateInstance<BezierEditor>().Show();
    }



    private void OnEnable()
    {
        FindPoint();
        Init();
        EditorApplication.update += EditorUpdate;
    }

    private void OnDisable()
    {
        DestroyImmediate(line);
        EditorApplication.update -= EditorUpdate;
    }

    private void Init()
    {
        pointArr = new List<Transform>();
        line = new GameObject("line");
        lineRenderer = line.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.generateLightingData = true;
        SetLineMat(lineMat);
        AddPoint(new Vector3(1, 0, 0));
        AddPoint(new Vector3(0.9f, 1, 0));
        // AddPoint(new Vector3(-0.3f, 1, 0));
        AddPoint(new Vector3(-1, 0, 0));

        UpdatePoints();
    }

    private GameObject FindPoint()
    {
        var currentScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
        GameObject[] gos = currentScene.GetRootGameObjects();
        foreach (var go in gos)
        {
            if (go.name == "Point")
            {
                point = go;
                break;
            }
        }

        return point;
    }


    private void AddPoint(Vector3 pos)
    {
        GameObject go = Instantiate(point);
        go.transform.position = pos;
        go.transform.parent = line.transform;
        pointArr.Add(go.transform);
    }

    private void EditorUpdate()
    {
        UpdatePoints();
    }
    // 更新点
    private void UpdatePoints()
    {
        posArr = Bezier.Instance.CreateBezierList(pointArr.Select((v) => { return v.position; }).ToArray(), 1, 20);
        Vector3[] points = posArr.Select((v) => { return v.pos; }).ToArray();
        if (lineRenderer)
        {
            lineRenderer.positionCount = points.Length;
            lineRenderer.SetPositions(points);
        }
    }

    // 设置线宽度
    private void SetLineWidth(float width)
    {
        lineRenderer.startWidth = width;
        lineRenderer.endWidth = width;
    }
    // 设置线材质
    private void SetLineMat(Material mat)
    {
        lineRenderer.material = mat;
    }


    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = new Color32(255, 255, 255, 255);
        GUILayout.Space(15);

        EditorGUILayoutTools.DrawSliderField("线宽度", ref lineWidth, 1, (v) =>
        {
            SetLineWidth(v);
        });

        EditorGUILayoutTools.DrawObjectField("线材质", ref lineMat, typeof(Material), (v) =>
        {
            SetLineMat(v as Material);
        });

        EditorGUILayoutTools.DrawFloatField("运动时长", ref runTime, (v) => { });

        EditorGUILayout.Space(10);

        if (GUILayout.Button("添加控制点", GUILayout.Height(20)))
        {

        }

        if (GUILayout.Button("播放", GUILayout.Height(20)))
        {
            Run();
        }
    }

    // 匀速运行
    private GameObject player;
    private void Run()
    {
        if (player == null)
        {
            player = Instantiate(point);
            player.transform.parent = line.transform;
        }
        player.SetActive(true);
        // player.transform.position = posArr[0].pos;
        // Sequence quence = DOTween.Sequence();

        var tweener = player.transform.DOPath(pointArr.Select((v) => { return v.position; }).ToArray(), runTime,PathType.CubicBezier);
        tweener.onComplete = () =>
        {
            player.SetActive(false);
            Debug.Log("运动完成");
        };
    }
}
