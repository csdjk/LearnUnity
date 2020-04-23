using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

// ---------------------------【自定义属性面板 - 选择不同的值绘制不同的属性】---------------------------
[CustomEditor(typeof(InspectorCustom))]
public class InspectorCustomEditor : Editor
{
   
    public SerializedObject mObject;

    public SerializedProperty mEnum;

    public SerializedProperty mInt;

    public SerializedProperty mFloat;

    public SerializedProperty mStr;

    public SerializedProperty mColor;

    public void OnEnable()
    {
        this.mObject = new SerializedObject(target);
        this.mEnum = this.mObject.FindProperty("mEnum");
        this.mInt = this.mObject.FindProperty("mInt");
        this.mFloat = this.mObject.FindProperty("FlatVal");
        this.mStr = this.mObject.FindProperty("StrVal");
        this.mColor = this.mObject.FindProperty("mColor");
    }


    public override void OnInspectorGUI()
    {
        this.mObject.Update();

        EditorGUILayout.PropertyField(this.mEnum);

        switch (this.mEnum.enumValueIndex)
        {
            case 1:
                EditorGUILayout.PropertyField(this.mInt);
                break;
            case 2:
                EditorGUILayout.PropertyField(this.mFloat);
                break;
            case 3:
                EditorGUILayout.PropertyField(this.mStr);
                break;
            case 4:
                EditorGUILayout.PropertyField(this.mColor);
                break;
        }
        
        this.mObject.ApplyModifiedProperties();

    }
}
