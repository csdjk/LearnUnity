using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GUISkin : EditorWindow
{
    [MenuItem("Editor教程/常用小功能/内置GUI皮肤的使用")]
    public static void showWindow()
    {
        EditorWindow.CreateInstance<GUISkin>().Show();
    }


    private void OnGUI() {
        // 方法一
        GUILayout.Button("方法一",GUI.skin.GetStyle("flow node 4"),GUILayout.Width(150));
        // 方法二
        GUILayout.Button("方法二",GUI.skin.button,GUILayout.Width(150));
        // 方法三
        GUILayout.Button("方法三",EditorStyles.miniButton,GUILayout.Width(150));
        // 方法四 - (推荐这种)
        GUILayout.Button("方法四",new GUIStyle("flow node 5"),GUILayout.Width(150));
    }
}
