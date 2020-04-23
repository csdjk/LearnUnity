
using UnityEngine;
using System.Collections;
using UnityEditor;

// ---------------------------【下拉选择框】---------------------------
public class SelectWindow : EditorWindow
{

    public int index;

    public EnumTest mEnum;


    [MenuItem("Editor基本组件/下拉选择框")]
    public static void showWindow()
    {
        EditorWindow.GetWindow<SelectWindow>().Show();
    }



    public void OnGUI()
    {
        string[] strs = new[]
        {
            "数组下标0",
            "数组下标1",
            "数组下标2",
        };

        int[] intArr = new[]
        {
            1,
            2,
            3,
        };
        //字符选择，返回选择的字符数组下标
        this.index = EditorGUILayout.Popup(this.index, strs);

        //字符选择，返回对应的整数数组的整数值
        this.index = EditorGUILayout.IntPopup(this.index, strs, intArr);

        Debug.Log("index:" + index);

        //枚举选择
        this.mEnum = (EnumTest)EditorGUILayout.EnumPopup(this.mEnum);

    }


}


public enum EnumTest
{
    Int1,
    Str2,
    Float3,
    Color4
}
