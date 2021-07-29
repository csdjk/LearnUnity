using System.IO;
using System.Text.RegularExpressions;
using System;
using System.Reflection;
using MyTools;
using UnityEditor;
using UnityEngine;

public class AttributeCallBack : Editor
{
    public static GameObject assetObj;


    [MenuItem("MyTools/打印log")]
    static void ShowWindow()
    {
        Debug.Log("test");
    }

    // 处理asset打开的callback函数
    [UnityEditor.Callbacks.OnOpenAssetAttribute(0)]
    static bool OnOpenAsset(int instance, int line)
    {
        string name = EditorUtility.InstanceIDToObject(instance).name;

        string stack_trace = GetStackTrace();
        // if (!string.IsNullOrEmpty(stack_trace))
        // {
        //     // 跳转到lua脚本
        //     if (stack_trace.Contains("stack traceback:"))
        //     {
        //         GotoLuaFile(stack_trace);
        //         return true;
        //     }
        //     // 选中预制体
        //     else if (stack_trace.Contains(FindPrefabByText.LogHead))
        //     {
        //         PingObject(stack_trace);
        //         return true;
        //     }
        //     return true;
        // }
        return false;
    }

    static void GotoLuaFile(string stack_trace)
    {
        string[] arr = Regex.Split(stack_trace, "stack traceback:");
        if (arr == null || arr.Length < 2)
        {
            Debug.LogError("未检查到调用栈log！");
            return;
        }
        stack_trace = arr[1];
        string[] lines = Regex.Split(stack_trace, "\n");
        foreach (var line in lines)
        {
            string lineTemp = line.Trim();
            if (!lineTemp.Equals(String.Empty) && !lineTemp.Contains("Error") && !lineTemp.Contains("Warning") && !lineTemp.Contains("log"))
            {
                string[] strArr = Regex.Split(lineTemp.Trim(), ":");
                if (strArr == null || strArr.Length < 2) return;

                string path = "Assets/ToLua/Lua/" + strArr[0] + ".lua";
                // 打开脚本
                if (File.Exists(path))
                    // UnityEditorInternal.InternalEditorUtility.OpenFileAtLineExternal(path, int.Parse(strArr[1]));
                    OpenFile(path, int.Parse(strArr[1]));
                else
                    Debug.LogError("脚本不存在:" + path);
                return;
            }
        }
    }
    // 选中节点
    static void PingObject(string stack_trace)
    {
        string path = stack_trace.Replace(FindPrefabByText.LogHead, String.Empty);
        string[] arr = Regex.Split(path, FindPrefabByText.LogHead2);

        if (arr != null && arr.Length > 0)
        {
            path = Regex.Replace(arr[0], "<[^>]+>", String.Empty);
            assetObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (assetObj)
            {
                EditorGUIUtility.PingObject(assetObj);
                Transform trs = FindPrefabByText.UIRoot.Find(assetObj.name);
                // 如果不存在
                if (trs == null)
                {
                    trs = (PrefabUtility.InstantiatePrefab(assetObj) as GameObject).transform;
                    trs.parent = FindPrefabByText.UIRoot;
                    trs.localScale = Vector3.one;
                }
                // 查找子节点
                string nodePath = Regex.Replace(arr[1], "<[^>]+>", String.Empty);
                string[] nodePathArr = nodePath.Split('\n');
                if (nodePathArr.Length > 0)
                {
                    nodePath = nodePathArr[0].Replace(trs.name + "/", String.Empty);
                    Transform node = trs.Find(nodePath);
                    if (node)
                    {
                        Selection.activeGameObject = node.gameObject;
                    }
                }
            }
        }
    }

    static void OpenFile(string path, int line)
    {
        System.Diagnostics.Process process = new System.Diagnostics.Process();
        System.Diagnostics.ProcessStartInfo startInfo = new System.Diagnostics.ProcessStartInfo();
        startInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
        startInfo.FileName = "D:/Software/Microsoft VS Code/Code.exe";
        startInfo.Arguments = string.Format("-g {0}:{1}", path, line);
        process.StartInfo = startInfo;
        process.Start();
    }


    static string GetStackTrace()
    {
        // 找到UnityEditor.EditorWindow的assembly
        var assembly_unity_editor = Assembly.GetAssembly(typeof(UnityEditor.EditorWindow));
        if (assembly_unity_editor == null) return null;

        // 找到类UnityEditor.ConsoleWindow
        var type_console_window = assembly_unity_editor.GetType("UnityEditor.ConsoleWindow");

        // type_console_window.getme
        if (type_console_window == null) return null;
        // 找到UnityEditor.ConsoleWindow中的成员ms_ConsoleWindow
        var field_console_window = type_console_window.GetField("ms_ConsoleWindow", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic);
        if (field_console_window == null) return null;
        // 获取ms_ConsoleWindow的值
        var instance_console_window = field_console_window.GetValue(null);
        if (instance_console_window == null) return null;

        // 如果console窗口时焦点窗口的话，获取stacktrace
        if ((object)UnityEditor.EditorWindow.focusedWindow == instance_console_window)
        {
            // 通过assembly获取类ListViewState
            var type_list_view_state = assembly_unity_editor.GetType("UnityEditor.ListViewState");
            if (type_list_view_state == null) return null;

            // 找到类UnityEditor.ConsoleWindow中的成员m_ListView
            var field_list_view = type_console_window.GetField("m_ListView", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field_list_view == null) return null;

            // 获取m_ListView的值
            var value_list_view = field_list_view.GetValue(instance_console_window);
            if (value_list_view == null) return null;

            // 找到类UnityEditor.ConsoleWindow中的成员m_ActiveText
            var field_active_text = type_console_window.GetField("m_ActiveText", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
            if (field_active_text == null) return null;

            // 获得m_ActiveText的值，就是我们需要的stacktrace
            string value_active_text = field_active_text.GetValue(instance_console_window).ToString();
            return value_active_text;
        }

        return null;
    }
}