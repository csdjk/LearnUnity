using UnityEngine;
using System.Collections;
using UnityEditor;

// ---------------------------【各种类型的显示框】---------------------------
public class TextColorWindow : EditorWindow
{

    [MenuItem("Editor教程/基本组件/各种类型的显示框")]
    public static void showWindow()
    {
        EditorWindow.GetWindow<TextColorWindow>().Show();
    }

    public string mText = "please input a string";
    public Color mColor = Color.black;


    public int mInt;
    public float mFloat;

    public float mMinVal;
    public float mMaxVal;


    public Vector2 mPos2;

    public Vector3 mPos3;

    public Vector4 mPos4;

    public Rect mRect;

    public Bounds mBounds;


    public void OnGUI()
    {
        Debug.Log("OnGUI");
        //空一行
        EditorGUILayout.Space();
        //文本标签
        EditorGUILayout.LabelField("输入文本");
        this.mText = EditorGUILayout.TextField(this.mText);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("输入文本");
        this.mText = EditorGUILayout.TextArea(this.mText);
        EditorGUILayout.Space();

        EditorGUILayout.SelectableLabel("输入密码");
        this.mText = EditorGUILayout.PasswordField(this.mText);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("选择颜色");
        this.mColor = EditorGUILayout.ColorField(this.mColor);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("输入整数");
        this.mInt = EditorGUILayout.IntField(this.mInt);
        // 滑动条
        this.mInt = EditorGUILayout.IntSlider(this.mInt, 0, 100);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("输入浮点数");
        this.mFloat = EditorGUILayout.FloatField(this.mFloat);
        // 滑动条
        this.mFloat = EditorGUILayout.Slider(this.mFloat, 0, 100);
        EditorGUILayout.Space();

        // 范围滑动条
        EditorGUILayout.LabelField("范围滑动条");
        this.mMinVal = EditorGUILayout.Slider(this.mMinVal, 0, 100);
        this.mMaxVal = EditorGUILayout.Slider(this.mMaxVal, 0, 100);
        EditorGUILayout.MinMaxSlider(ref this.mMinVal, ref this.mMaxVal, 0, 100);
        EditorGUILayout.Space();

        this.mPos2 = EditorGUILayout.Vector2Field("二维坐标", this.mPos2);
        EditorGUILayout.Space();

        this.mPos3 = EditorGUILayout.Vector3Field("三维坐标", this.mPos3);
        EditorGUILayout.Space();

        this.mPos4 = EditorGUILayout.Vector4Field("四维坐标", this.mPos4);
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("矩阵");
        this.mRect = EditorGUILayout.RectField(this.mRect);
        EditorGUILayout.Space();


        EditorGUILayout.LabelField("距离");
        this.mBounds = EditorGUILayout.BoundsField(this.mBounds);
        EditorGUILayout.Space();

    }



}
