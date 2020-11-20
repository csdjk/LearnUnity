using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System.Collections;
using CustomCoroutine;
using BezierUtils;

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
        SetWidth(BezierEditor.lineWidth);
        SetMaterial(BezierEditor.lineMat);
    }

    private void AddPoint(Vector3 pos, int index)
    {
        GameObject go = UnityEngine.Object.Instantiate(BezierEditor.point);
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
        sublineRenderer.startColor = BezierEditor.sublineColor;
        sublineRenderers.Add(sublineRenderer);
    }

    public void SetWidth(float lineWidth)
    {
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
    }
    public void SetMaterial(Material lineMat)
    {
        if (lineMat == null)
        {
            lineMat = new Material(Shader.Find("Particles/Standard Unlit"));
        }
        lineRenderer.material = lineMat;
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
}

//-----------------------------【贝塞尔曲线规划工具】-----------------------------
public class BezierEditor : EditorWindow
{
    // private GameObject line;
    private LineRenderer lineRenderer;
    public static GameObject point;
    // 线宽度
    public static float lineWidth = 0.05f;
    public static Material lineMat;
    public static Color sublineColor = Color.white;

    private BezierCurve curve;
    private List<Transform> pointArr;
    // 生命周期
    public float duration = 1.0f;
    public EaseType ease = EaseType.Linear;

    public string curPath = "Assets/Scenes/Bezier";

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
        ForeachCurve(curve, (v) =>
       {
           DestroyImmediate(v.line);
       });
        DestroyImmediate(player);
        EditorApplication.update -= EditorUpdate;
    }

    private void Init()
    {
        pointArr = new List<Transform>();

        AddCurve(
            new BezierCurve(new Vector3[]{
                new Vector3(1, 0, 0),
                new Vector3(0.9f, 1, 0),
                new Vector3(-0.3f, 1, 0),
                new Vector3(-1, 0, 0)
            }));
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
    // 曲线
    private void AddCurve(BezierCurve curve)
    {
        if (this.curve == null)
        {
            this.curve = curve;
        }
        else
        {
            this.curve.nextCurve = curve;
        }
    }

    private void EditorUpdate()
    {
        CoroutineManager.Instance.UpdateCoroutine();

        ForeachCurve(curve, (v) =>
        {
            v.Update();
        });
    }

    private void ForeachCurve(BezierCurve node, Action<BezierCurve> func)
    {
        var curNode = node;
        while (curNode != null)
        {
            func(curNode);
            curNode = curNode.nextCurve;
        }
    }

    // 设置线宽度
    private void SetLineWidth(float width)
    {
        ForeachCurve(curve, (v) =>
       {
           v.SetWidth(width);
       });
    }
    // 设置线材质
    private void SetLineMat(Material mat)
    {
        ForeachCurve(curve, (v) =>
        {
            v.SetMaterial(mat);
        });
    }

    private void SetSublineColor(Color color)
    {
        ForeachCurve(curve, (v) =>
        {
            v.SetSublineColor(color);
        });
    }

    private void OnGUI()
    {
        GUIStyle style = new GUIStyle();
        style.fontSize = 16;
        style.normal.textColor = new Color32(255, 255, 255, 255);
        GUILayout.Space(15);

        EditorGUILayoutTools.DrawSliderField("曲线宽度", ref lineWidth, 1, (v) =>
        {
            SetLineWidth(v);
        });
        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayoutTools.DrawObjectField("曲线材质", ref lineMat, typeof(Material), (v) =>
            {
                SetLineMat(v);
            });

            EditorGUILayoutTools.DrawColorField("辅助线颜色", ref sublineColor, (v) =>
            {
                SetSublineColor(v);
            });
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayoutTools.DrawFloatField("运动时长", ref duration, (v) => { });

            EditorGUILayoutTools.DrawEnumPopup("Ease", ref ease, (value) => { });
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        EditorGUILayoutTools.DrawTextField("曲线保存路径", ref curPath, (v) => { });

        EditorGUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            if (GUILayout.Button("保存曲线为预制体", GUILayout.Height(20)))
            {
                SaveCurvePrefab();
            }

            if (GUILayout.Button("播放", GUILayout.Height(20)))
            {
                Run();
            }
        }
        EditorGUILayout.EndHorizontal();

    }

    private void SaveCurvePrefab()
    {
        var curClone = Instantiate(curve.line);
        for (var i = curClone.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(curClone.transform.GetChild(i).gameObject);
        }

        PrefabUtility.SaveAsPrefabAsset(curClone, curPath + "/" + curClone.name + ".prefab");
        // var path = EditorUtility.SaveFilePanelInProject("Save Curve", "", curClone.name, "prefab");
        DestroyImmediate(curClone);
        AssetDatabase.Refresh();
    }

    // 
    private GameObject player;
    private void Run()
    {
        if (player == null)
        {
            player = Instantiate(point);
            player.name = "Player";
        }
        player.SetActive(true);
        player.transform.MovePath(curve.points, duration, ease);
    }

}
