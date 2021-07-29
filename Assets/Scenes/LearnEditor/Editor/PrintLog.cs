using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PrintLog : Editor
{
    [MenuItem("Editor教程/打印log")]
    static void ShowWindow()
    {
        Debug.Log("test");
    }
}
