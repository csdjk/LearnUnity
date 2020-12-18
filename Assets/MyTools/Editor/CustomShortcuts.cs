/*
 * @Descripttion: 自定义快捷键
 * @Author: lichanglong
 * @Date: 2020-12-18 11:33:56
 */

using UnityEditor;
using UnityEngine;

/// <summary>
/// 自定义快捷键
/// </summary>
public class CustomShortcuts
{

    [MenuItem("MyTools/CustomKeys/播放 _F5")]
    static void EditorPlayCommand()
    {
        EditorApplication.isPlaying = !EditorApplication.isPlaying;
    }

    [MenuItem("MyTools/CustomKeys/暂停 _F6")]
    static void EditorPauseCommand()
    {
        EditorApplication.isPaused = !EditorApplication.isPaused;
    }

    [MenuItem("MyTools/CustomKeys/快速定位到Lua %l")]
    static void QuickPositioningLua()
    {
        var assetObj = AssetDatabase.LoadAssetAtPath<Object>("Assets/ToLua/Lua");
        EditorGUIUtility.PingObject(assetObj);
    }


    [MenuItem("MyTools/CustomKeys/快速定位到GUI %g")]
    static void QuickPositioningGUI()
    {
        var assetObj = AssetDatabase.LoadAssetAtPath<Object>("Assets/Resources/GUI");
        EditorGUIUtility.PingObject(assetObj);
    }
}
