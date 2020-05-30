using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// ---------------------------【自定义属性面板 - 选择不同的值绘制不同的属性】---------------------------
public class InspectorCustom : MonoBehaviour
{

    public MyEnum mEnum;

    public int mInt;

    public float FlatVal;

    public string StrVal;

    public Color mColor;
}


public enum MyEnum
{
    None,
    IntVal,
    FloatVal,
    StrVal,
    CocolVal
}
