using UnityEditor;
using System;
using UnityEngine;

public class EditorGUILayoutTools
{
    public static float RowSpace = 15;
    public static float HeadSpace = 20;
    // 水平布局
    public static void Horizontal(Action func)
    {
        GUILayout.Space(RowSpace);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(HeadSpace);
            func();
            GUILayout.Space(HeadSpace);

        }
        EditorGUILayout.EndHorizontal();
    }
    /// 绘制领域 
    public static void DrawField<T>(string title, ref T value, Func<T, T> func, Action<T> changeHandler)
    {
        GUILayout.Space(RowSpace);
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(HeadSpace);
            GUILayout.Label(title);
            
            T preValue = value;

            value = func(value);

            GUILayout.Space(HeadSpace);
            if (value != null && !value.Equals(preValue))
            {
                changeHandler(value);
            }
        }
        EditorGUILayout.EndHorizontal();
    }

    // 绘制 float 输入框
    public static void DrawFloatField(string title, ref float value, Action<float> changeHandler)
    {

        DrawField(title, ref value, (v) =>
        {
            return EditorGUILayout.FloatField(v);
        }, (v) =>
        {
            changeHandler(v);
        });
    }
    // 
    public static void DrawSliderField(string title, ref float value, float max, Action<float> changeHandler)
    {
        DrawField(title, ref value, (v) =>
        {
            return EditorGUILayout.Slider(v, 0, max);
        }, (v) =>
        {
            changeHandler(v);
        });
    }


    // 
    public static void DrawObjectField<T>(string title, ref T value, Type type, Action<T> changeHandler) where T : UnityEngine.Object
    {
        DrawField(title, ref value, (v) =>
        {
            return EditorGUILayout.ObjectField(v, type, true) as T;
        }, (v) =>
        {
            changeHandler(v);
        });
    }

    // 绘制 下拉列表 UI
    public static void DrawEnumPopup<T>(string title, ref T value, Action<T> changeHandler) where T : Enum
    {
        DrawField(title, ref value, (v) =>
       {
           return (T)EditorGUILayout.EnumPopup(v);
       }, (v) =>
       {
           changeHandler(v);
       });
    }

    // 绘制 下拉列表 UI
    public static void DrawPopup(string title, ref int selectIndex, string[] list, Action<string> changeHandler)
    {
        DrawField(title, ref selectIndex, (v) =>
        {
            return EditorGUILayout.Popup(v, list, "ToolbarPopup");
        }, (v) =>
        {
            changeHandler(list[v]);
        });
    }

    public static void DrawColorField(string title, ref Color value, Action<Color> changeHandler)
    {
        DrawField(title, ref value, (v) =>
        {
            return EditorGUILayout.ColorField(v);
        }, (v) =>
        {
            changeHandler(v);
        });
    }
    // 
    public static void DrawGradientField(string title, ref Gradient value, Action<Gradient> changeHandler)
    {
        DrawField(title, ref value, (v) =>
        {
            return EditorGUILayout.GradientField(v);
        }, (v) => { });
        // 因为Gradient 不好比较 就直接执行了
        changeHandler(value);
    }

    public static void DrawTextField(string title, ref string value, Action<string> changeHandler)
    {
        DrawField(title, ref value, (v) =>
        {
            return EditorGUILayout.TextField(v);
        }, (v) =>
        {
            changeHandler(v);
        });
    }
}