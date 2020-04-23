using UnityEngine;
using System.Collections;
using UnityEditor;
// ---------------------------【自定义窗口显示及事件测试】---------------------------
public class CustomWindowEvents : EditorWindow
{

    [MenuItem("Editor基本组件/窗口2")]
    public static void showWindwo()
    {
        //利用单例只可以打开一个窗口
        EditorWindow.GetWindow<CustomWindowEvents>().Show();
    }

    /// <summary>
    /// 绘制操作面板
    /// </summary>
    public void OnGUI()
    {
        if (GUILayout.Button("关闭"))
        {
            this.Close();
        }
    }

    public int index_update = 0;

    public int index_OnInspectorUpdate = 0;

    /// <summary>
    /// 刷新，每秒100次
    /// </summary>
    public void Update()
    {
        index_update++;
        // Debug.Log("index_update:" + index_update);
    }

    /// <summary>
    /// 刷新方法，比Update少
    /// </summary>
    public void OnInspectorUpdate()
    {
        index_OnInspectorUpdate++;
        // Debug.Log("index_OnInspectorUpdate:" + index_OnInspectorUpdate);
    }
    /// <summary>
    /// 视图被删除
    /// </summary>
    public void OnDestroy()
    {
        Debug.Log("视图被删除");
    }

    /// <summary>
    /// 选择对象发生改变
    /// </summary>
    public void OnSelectionChange()
    {
        Debug.Log("选择一个场景内对象：" + Selection.gameObjects.Length);

        //打印出场景里选择的对象
        for (int i = 0; i < Selection.gameObjects.Length; i++)
        {
            Debug.Log("选择一个场景内对象：" + Selection.gameObjects[i].name);
        }

        //打开所有选择的对象
        for (int i = 0; i < Selection.objects.Length; i++)
        {
            Debug.Log("选择一个对象：" + Selection.objects[i].name);
        }

    }
    // 获取焦点
    public void OnFocus()
    {
        Debug.Log("OnFocus");
    }

    // 失去焦点
    public void OnLostFocus()
    {
        Debug.Log("OnLostFocus");
    }

    /// <summary>
    /// HirearchyChange更改
    /// </summary>
    public void OnHierarchyChange()
    {

        Debug.Log("OnHierarchyChange");

    }

    /// <summary>
    /// Project 更改
    /// </summary>
    public void OnProjectChange()
    {
        Debug.Log("OnProjectChange");
    }

}
