using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DrawGUIInScene : Editor
{
    [MenuItem("Editor教程/常用小功能/在Scene绘制GUI")]
    private static void ShowGUI()
    {
        SceneView.duringSceneGui += OnScene;
    }

    private static void OnScene(SceneView sceneview)
    {
        // new GUIStyle(EditorStyles.label)
        Handles.BeginGUI();
        GUILayout.BeginArea(new Rect(0, 0, 200, 220),"Box",GUI.skin.box);
        GUILayout.Space(50);
        if (GUILayout.Button("Test",EditorStyles.miniButtonMid))
        {
            Debug.Log("Button Test!");
        }
        GUILayout.EndArea();
        Handles.EndGUI();
    }
}
