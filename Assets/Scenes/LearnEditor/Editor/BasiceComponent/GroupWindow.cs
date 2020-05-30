using UnityEngine;
using System.Collections;
using UnityEditor;

// ---------------------------【Group - 分组】---------------------------
public class GroupWindow : EditorWindow
{

    [MenuItem("Editor教程/基本组件/字段分组")]
    public static void showWindow()
    {
        EditorWindow.GetWindow<GroupWindow>().Show();

    }


    public bool mSelect;

    public Vector2 mPos;


    public void OnGUI()
    {
        //滚动分组----Begin
        this.mPos = EditorGUILayout.BeginScrollView(this.mPos);
        Debug.Log("滚动分组测试位置：" + this.mPos);

        // ---------------------------【Toggle分组----Begin】---------------------------
        // 创建一个通过toogle来控制的垂直分组
        this.mSelect = EditorGUILayout.BeginToggleGroup("选择分组", this.mSelect);

        EditorGUILayout.LabelField("1");
        EditorGUILayout.TextField("sdk");
        EditorGUILayout.LabelField("2");
        EditorGUILayout.TextField("sdk");
        EditorGUILayout.LabelField("3");
        EditorGUILayout.TextField("sdk");

        EditorGUILayout.EndToggleGroup();
        // ---------------------------【Toggle分组-----End】---------------------------

        EditorGUILayout.Space();

        //水平分组----Begin
        Rect rect1 = EditorGUILayout.BeginHorizontal();

        EditorGUILayout.LabelField("5");
        EditorGUILayout.TextField("sdk");
        EditorGUILayout.LabelField("6");
        EditorGUILayout.TextField("sdk");
        EditorGUILayout.LabelField("7");
        EditorGUILayout.TextField("sdk");
        EditorGUILayout.LabelField("8");
        EditorGUILayout.TextField("sdk");
        Debug.Log("水平分组测试数据：" + rect1);

        EditorGUILayout.EndHorizontal();
        //水平分组----End


        //垂直分组----Bengin
        Rect rect2 = EditorGUILayout.BeginVertical();

        EditorGUILayout.LabelField("5");
        EditorGUILayout.TextField("sdk");
        EditorGUILayout.LabelField("6");
        EditorGUILayout.TextField("sdk");
        EditorGUILayout.LabelField("7");
        EditorGUILayout.TextField("sdk");
        EditorGUILayout.LabelField("8");
        EditorGUILayout.TextField("sdk");

        EditorGUILayout.EndVertical();
        //垂直分组----End


        //滚动分组-----End
        EditorGUILayout.EndScrollView();

    }
}
