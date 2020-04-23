using UnityEngine;
using System.Collections;
using UnityEditor;

// ---------------------------【自定义窗口显示及事件】---------------------------
public class CustomWindow : EditorWindow
{
    [MenuItem("Editor基本组件/显示窗口")]
    public static void showWindow()
    {
        // 能悬浮、能拖拽、能嵌入
        EditorWindow.CreateInstance<CustomWindow>().Show();
        // 只能悬浮
        //    EditorWindow.CreateInstance<CustomWindow>().ShowUtility();
        // 无法关闭的窗口
        // EditorWindow.CreateInstance<CustomWindow>().ShowPopup();

    }


    // ->绘制窗口
    public void OnGUI()
    {
        if (GUILayout.Button("关闭"))
        {
            this.Close();
        }
    }
    // ->刷新方法，100次/秒
    public void Update()
    {

    }
    // ->刷新方法，比Update（）少
    public void OnInspectorUpdate()
    {

    }
    // ->选择对象发生改变
    public void OnSelectionChange()
    {

    }

    // ->获得焦点
    public void OnFocus()
    {

    }
    // ->失去焦点
    public void OnLostFocus()
    {

    }
    // ->Hierarchay视图窗口文件发生改变
    public void OnHierarchayChange()
    {

    }

    // ->Project视图窗口文件发生改变
    public void OnProjectChange()
    {

    }
    // ->销毁窗口
    public void OnDestroy()
    {

    }
}
