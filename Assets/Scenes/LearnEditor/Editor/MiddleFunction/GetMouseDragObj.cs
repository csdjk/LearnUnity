using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class GetMouseDragObj : EditorWindow
{
    [MenuItem("Editor教程/常用小功能/捕获鼠标拖拽到场景或者编辑器窗口的对象")]
    public static void showWindow()
    {
        EditorWindow.CreateInstance<GetMouseDragObj>().Show();
        SceneView.duringSceneGui += OnScene;
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        GUILayout.FlexibleSpace();
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.Label("请分别拖拽文件到 场景 和 当前窗口 测试!!");
        GUILayout.FlexibleSpace();
        GUILayout.EndHorizontal();
        GUILayout.FlexibleSpace();
        GUILayout.EndVertical();

        if (EventType.DragUpdated == Event.current.type)
        {
            foreach (var item in DragAndDrop.objectReferences)
            {
                Debug.Log("检查到拖拽到编辑器窗口的对象: " + item.name);
            }
        }
    }

    private static void OnScene(SceneView sceneview)
    {
        if (EventType.DragUpdated == Event.current.type)
        {
            foreach (var item in DragAndDrop.objectReferences)
            {
                Debug.Log("检查到拖拽到场景的对象: " + item.name);
            }
        }
    }

    //当层级视图中的对象或对象组发生更改时引发的事件
    private static void OnHierarchyChanged()
    {
        if (Selection.activeGameObject != null && PrefabUtility.GetPrefabType(Selection.activeGameObject) == PrefabType.PrefabInstance)
        {
            if (Selection.activeGameObject)
                DestroyImmediate(Selection.activeGameObject);
            Debug.LogError("非法操作! 禁止拖拽物体到场景! 请通过编辑器添加物体!");
        }
    }


    private void OnDestroy() {
        SceneView.duringSceneGui -= OnScene;
    }
}
