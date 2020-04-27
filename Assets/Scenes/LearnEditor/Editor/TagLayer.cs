using System;
using UnityEngine;
using System.Collections;
using UnityEditor;

// ---------------------------【标签、层、对象选择】---------------------------
public class TagLayer : EditorWindow 
{

    [MenuItem("Editor基本组件/标签、层、对象选择")]
    public static void showWindow()
    {
        EditorWindow.GetWindow<TagLayer>().Show();
    }

    public String mTag;

    public int mLayer;

    public UnityEngine.Object mObject;

    public void OnGUI()
    {
        EditorGUILayout.LabelField("Tag");
        mTag = EditorGUILayout.TagField(this.mTag);
        EditorGUILayout.Space();
        
        EditorGUILayout.LabelField("Layer");
        mLayer = EditorGUILayout.LayerField(this.mLayer);
        EditorGUILayout.Space();

        EditorGUILayout.LabelField("GameObject");
        mObject = EditorGUILayout.ObjectField(this.mObject, typeof(Camera),true);
        EditorGUILayout.Space();
    }

    
}
