using UnityEngine;
using UnityEditor;
public class DragFloderWindow : EditorWindow
{

      // 当前选择路径
    public string path = "Assets";
    public Rect pathRect;
    public GUILayoutOption test;
    // 打包后的文件
    public string buildPath = "Assets/Build";
    public Rect buildPathRect;


    [MenuItem("Tools/文件拖拽")]
    static void createWindow()
    {
        var window = EditorWindow.GetWindow<DragFloderWindow>(false, "FolderWindow");
        window.minSize = new Vector2(400, 400);
        window.Show();
    }
    private void OnGUI()
    {
         // 需要构建的路径
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("需要打包的路径 (鼠标拖拽文件夹到这里)");
        EditorGUILayout.Space();
        // this.path = EditorGUILayout.TextField(this.path);
        GUI.SetNextControlName("input1");//设置下一个控件的名字
        pathRect = EditorGUILayout.GetControlRect(GUILayout.Width(400));
        EditorGUI.TextField(pathRect, path);
        EditorGUILayout.Space();

        // 构建后的路径
        EditorGUILayout.LabelField("打包后的路径 (鼠标拖拽文件夹到这里)");
        EditorGUILayout.Space();
        // this.buildPath = EditorGUILayout.TextField(this.buildPath);
        GUI.SetNextControlName("input2");//设置下一个控件的名字
        buildPathRect = EditorGUILayout.GetControlRect(GUILayout.Width(400));
        EditorGUI.TextField(buildPathRect, buildPath);
        EditorGUILayout.Space();

        // 文件拖拽
        DragFolder();
    }

    void DragFolder()
    {
        //鼠标位于当前窗口
        if (mouseOverWindow == this)
        {
            //拖入窗口未松开鼠标
            if (Event.current.type == EventType.DragUpdated)
            {
                DragAndDrop.visualMode = DragAndDropVisualMode.Generic;//改变鼠标外观
                // 判断区域
                if (pathRect.Contains(Event.current.mousePosition))
                    GUI.FocusControl("input1");
                else if (buildPathRect.Contains(Event.current.mousePosition))
                    GUI.FocusControl("input2");
            }
            //拖入窗口并松开鼠标
            else if (Event.current.type == EventType.DragExited)
            {
                //获取焦点，
                string dragPath = string.Join("", DragAndDrop.paths);
                // 判断区域
                if (pathRect.Contains(Event.current.mousePosition))
                    this.path = dragPath;
                else if (buildPathRect.Contains(Event.current.mousePosition))
                    this.buildPath = dragPath;
                // Debug.Log("path:" + path);
                // Debug.Log("buildPath:" + buildPath);

                // 取消焦点(不然GUI不会刷新)
                GUI.FocusControl(null);
            }
        }
    }
}