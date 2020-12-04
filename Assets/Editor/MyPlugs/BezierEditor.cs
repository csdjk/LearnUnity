using System;
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using System.Collections;
using CustomCoroutine;
using BezierUtils;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


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

    public Gradient lineColor = new Gradient();

    private BezierCurve curve;
    private List<Transform> pointArr;
    // 生命周期
    public float duration = 1.0f;
    public EaseType ease = EaseType.Linear;

    public string curPath = "Assets/Scenes/Bezier/Prefabs";

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
        SetLineMat(lineMat);
        SetLineWidth(lineWidth);
        SetLineColor();
        SetSublineColor(sublineColor);
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

    private void SetLineColor()
    {
        ForeachCurve(curve, (v) =>
        {
            v.SetColor(lineColor);
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

            EditorGUILayoutTools.DrawGradientField("曲线颜色", ref lineColor, (v) =>
            {
                SetLineColor();
            });
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);
        EditorGUILayoutTools.DrawColorField("辅助线颜色", ref sublineColor, (v) =>
        {
            SetSublineColor(v);
        });
        EditorGUILayout.Space(10);
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayoutTools.DrawObjectField("执行对象", ref player, typeof(GameObject), (v) =>
            {
                // SetLineMat(player);
            });

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
    // 保存曲线
    private void SaveCurvePrefab()
    {
        var curClone = Instantiate(curve.line);
        for (var i = curClone.transform.childCount - 1; i >= 0; i--)
        {
            DestroyImmediate(curClone.transform.GetChild(i).gameObject);
        }

        PrefabUtility.SaveAsPrefabAsset(curClone, curPath + "/" + curClone.name + ".prefab");
        // var path = EditorUtility.SaveFilePanelInProject("Save Curve", "", curClone.name + ".prefab", "prefab");

        // if (path.Length != 0)
        // {
        //     byte[] goData = ConvetToObj(curClone);
        //     if (goData != null)
        //     {
        //         File.WriteAllBytes(path, goData);
        //         AssetDatabase.Refresh();
        //     }
        // }

        DestroyImmediate(curClone);
        AssetDatabase.Refresh();
    }


    private byte[] ConvetToObj(object obj)
    {
        BinaryFormatter se = new BinaryFormatter();
        MemoryStream memStream = new MemoryStream();
        se.Serialize(memStream, obj);
        byte[] bobj = memStream.ToArray();
        memStream.Close();
        return bobj;

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
