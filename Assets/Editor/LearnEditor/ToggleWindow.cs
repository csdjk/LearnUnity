using UnityEngine;
using System.Collections;
using UnityEditor;

public class ToggleWindow : EditorWindow 
{

    [MenuItem("Editor基本组件/单选字段选择")]
    public static void showWindow()
    {
        EditorWindow.GetWindow<ToggleWindow>().Show();
    }

    public bool mIsSelect;



    public void OnGUI()
    {

        this.mIsSelect = EditorGUILayout.Toggle("选择", this.mIsSelect);


        this.mIsSelect = EditorGUILayout.Foldout(this.mIsSelect, "折叠");

        if(this.mIsSelect)
        {
            EditorGUILayout.LabelField("you and me");
            EditorGUILayout.LabelField("you and me");
            EditorGUILayout.LabelField("you and me");
            EditorGUILayout.LabelField("you and me");
            EditorGUILayout.LabelField("you and me");
        }

    }

}
