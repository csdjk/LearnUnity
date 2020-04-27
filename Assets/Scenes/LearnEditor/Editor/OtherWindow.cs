using UnityEngine;
using System.Collections;
using UnityEditor;

public class OtherFiledWindow : EditorWindow 
{
    [MenuItem("Editor基本组件/曲线")]
    public static void showWindow()
    {
        EditorWindow.GetWindow<OtherFiledWindow>().Show();
    }

    public AnimationCurve mAC = new AnimationCurve();


    public bool mSelect;

    public Object mObj;

    public void OnGUI()
    {
        //动画字段
        this.mAC = EditorGUILayout.CurveField("动画片段",this.mAC);

        //获得选择的物体
        this.mObj = EditorGUILayout.ObjectField(this.mObj, typeof (Transform),true);

        //将选择的物体放在面板上
        this.mSelect = EditorGUILayout.InspectorTitlebar(this.mSelect,this.mObj);
    }
}
