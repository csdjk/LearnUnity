using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(OwnerUITest))]
public class OwnerUIInspector : Editor
{

    public SerializedObject mObj;

    public SerializedProperty mInt;

    public SerializedProperty mFloat;

    public SerializedProperty mStr;

    public SerializedProperty mtype;




    /// <summary>
    /// 选择当前的游戏对象时执行
    /// </summary>
    public void OnEnable()
    {
        // Debug.Log("OnEnable()------");
        this.mObj = new SerializedObject(target);
        this.mInt = this.mObj.FindProperty("IntVal");
        this.mFloat = this.mObj.FindProperty("FlatVal");
        this.mStr = this.mObj.FindProperty("StrVal");
        this.mtype = this.mObj.FindProperty("mType");
    }



    /// <summary>
    /// 绘制
    /// </summary>
    public override void OnInspectorGUI()
    {
        // Debug.Log("OnInspectorGUI()------");
        this.mObj.Update();

        EditorGUILayout.PropertyField(this.mInt);
        EditorGUILayout.PropertyField(this.mFloat);
        // EditorGUILayout.PropertyField(this.mStr);
        //true,表示显示出类的子节点
        // EditorGUILayout.PropertyField(this.mtype, true);

    }



}
