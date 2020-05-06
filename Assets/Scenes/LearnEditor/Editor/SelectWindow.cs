
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
        // DisplayShaderContext(EditorGUILayout.GetControlRect());
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

        // shader 下拉列表
        EditorGUILayout.LabelField("Shader下拉列表");
        ShaderInfo[] shaderLists = ShaderUtil.GetAllShaderInfo();
        string[] nameLists = new string[shaderLists.Length];
        for (int i = 0; i < shaderLists.Length; i++)
        {
            nameLists[i] = shaderLists[i].name;
        }
        EditorGUILayout.Popup(this.index, nameLists);

    }


}


public enum EnumTest
{
    Int1,
    Str2,
    Float3,
    Color4
}
