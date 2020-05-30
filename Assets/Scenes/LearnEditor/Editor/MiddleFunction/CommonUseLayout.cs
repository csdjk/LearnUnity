using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class CommonUseLayout : EditorWindow
{
    [MenuItem("Editor教程/常用小功能/常用布局方式")]
    public static void showWindow()
    {
        EditorWindow.CreateInstance<CommonUseLayout>().Show();
    }

    private void OnGUI()
    {

        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            GUILayout.Label("水平居中");
            GUILayout.Label("水平居中");
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        // 行
        GUILayout.BeginHorizontal();
        {
            // 第一列
            GUILayout.BeginVertical();
            {
                GUILayout.Label("垂直居中开始");
                GUILayout.FlexibleSpace();
                GUILayout.Label("垂直居中");
                GUILayout.Label("垂直居中");
                GUILayout.FlexibleSpace();
                GUILayout.Label("垂直居中结束");

            }
            GUILayout.EndVertical();
            // 第二列
            GUILayout.BeginVertical();
            {
                GUILayout.Label("垂直居中开始");
                GUILayout.BeginVertical();
                {
                    GUILayout.FlexibleSpace();
                    GUILayout.Label("垂直居中");
                    GUILayout.Label("垂直居中");
                    GUILayout.FlexibleSpace();
                }
                GUILayout.EndVertical();
                GUILayout.Label("垂直居中结束");

            }
            GUILayout.EndVertical();
        }
        GUILayout.EndHorizontal();
    }

}
