using UnityEditor;
using System;
using UnityEngine;
using UnityEngine;

public class EditorGUILayoutTools {
     public static float RowSpace = 5;
    public static float HeadSpace = 10;

    // 绘制 float 输入框
    public static void DrawFloatField(string title, ref float value, Action<float> changeHandler)
    {
        GUILayout.Space(RowSpace);
        EditorGUILayout.BeginHorizontal("box");
        GUILayout.Space(HeadSpace);
        float preValue = value;
        value = EditorGUILayout.FloatField(title, value);
        GUILayout.FlexibleSpace();
        GUILayout.Space(HeadSpace);
        EditorGUILayout.EndHorizontal();
        if (preValue != value)
        {
            changeHandler(value);
        }
    }
    // 
    public static void DrawSliderField(string title, ref float value, float max, Action<float> changeHandler)
    {
        GUILayout.Space(RowSpace);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(HeadSpace);
        GUILayout.Label(title);
        float preValue = value;
        value = EditorGUILayout.Slider(value, 0, max);
        GUILayout.Space(HeadSpace);
        EditorGUILayout.EndHorizontal();
        if (preValue != value)
        {
            changeHandler(value);
        }
    }


      // 
    public static void DrawObjectField<T>(string title, ref T value,Type type, Action<UnityEngine.Object> changeHandler) where T:UnityEngine.Object
    {
        GUILayout.Space(RowSpace);
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(HeadSpace);
        GUILayout.Label(title);
        T preValue = value;
        value = EditorGUILayout.ObjectField(value as UnityEngine.Object, type, true) as T;
        GUILayout.Space(HeadSpace);
        EditorGUILayout.EndHorizontal();
        if (preValue != value)
        {
            changeHandler(value);
        }
    }
}