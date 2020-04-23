using System.IO;
using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;

// ---------------------------【构建Animation Controller 和 保存Prefabs】---------------------------
public class BuildAnimationAndPrefab : EditorWindow
{
    // 当前选择路径
    public string path = "Assets";
    public Rect pathRect;
    public GUILayoutOption test;
    // 打包后的文件
    public string buildPath = "Assets/Build";
    public Rect buildPathRect;


    [MenuItem("Tools/构建Animation")]
    public static void showWindow()
    {
        // 能悬浮、能拖拽、能嵌入
        EditorWindow.CreateInstance<BuildAnimationAndPrefab>().Show();
        // var window = EditorWindow.GetWindow<TestWindow>(false, "FolderWindow");
        // window.minSize = new Vector2(400, 400);
        // window.Show();
    }

    public void OnGUI()
    {
        // 鼠标当前选中路径
        // string[] strs = Selection.assetGUIDs;
        // if (strs.Length != 0)
        // this.path = AssetDatabase.GUIDToAssetPath(strs[0]);

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

        // 创建资源按钮
        if (GUILayout.Button("Create"))
        {
            this.DoCreateAnimationAssets();
        }
    }

    // 鼠标拖拽文件
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

    // 创建Animation
    void DoCreateAnimationAssets()
    {
        string[] allPath = AssetDatabase.FindAssets("t:GameObject", new string[] { this.path });
        if (allPath.Length == 0)
        {
            CreateAnimation(this.path);
        }
        else
        {
            for (int i = 0, len = allPath.Length; i < len; i++)
            {
                string childpath = AssetDatabase.GUIDToAssetPath(allPath[i]);
                CreateAnimation(childpath);
            }
        }
    }

    // 创建Animation
    void CreateAnimation(string path)
    {
        if (System.IO.Path.GetExtension(path) == ".fbx")
        {
            //创建Controller
            AnimatorController animatorController = AnimatorController.CreateAnimatorControllerAtPath(path.Replace(".fbx", ".controller"));
            //得到它的Layer
            AnimatorControllerLayer layer = animatorController.layers[0];
            AddStateTransition(path, layer);
            // 设置模型设置
            setModelSetup(path);

            Material mat = CreateMaterial(path);
            // 创建预制体
            CreatePrefab(path, animatorController, mat);
            Debug.Log("-- 创建成功: " + path);
        }
    }

    // 修改模型设置
    void setModelSetup(string path)
    {
        ModelImporter importer = ModelImporter.GetAtPath(path) as ModelImporter;
        if (importer)
        {
            importer.animationType = ModelImporterAnimationType.Generic;
            importer.avatarSetup = ModelImporterAvatarSetup.CreateFromThisModel;
            importer.globalScale = 100;
            importer.materialImportMode = ModelImporterMaterialImportMode.None;
            importer.SaveAndReimport();
            // Debug.Log("-- 修改模型设置: " + importer.name);
        }
    }
    // 创建预制体
    void CreatePrefab(string path, AnimatorController animatorCtorl, Material mat)
    {
        // 创建预制体
        GameObject gameObj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        // 先实例化
        GameObject instanceRoot = PrefabUtility.InstantiatePrefab(gameObj) as GameObject;
        // 绑定AnimationController
        Animator animator = instanceRoot.GetComponent<Animator>();
        animator.runtimeAnimatorController = animatorCtorl;
        // 赋值材质
        SkinnedMeshRenderer skinMeshRender = instanceRoot.GetComponentInChildren<SkinnedMeshRenderer>();
        skinMeshRender.material = mat;
        // 保存为预制体
        // path.Replace(".fbx", ".prefab")
        var variantRoot = PrefabUtility.SaveAsPrefabAsset(instanceRoot, buildPath + "/" + Path.GetFileNameWithoutExtension(path) + ".prefab");
        // 删除游戏物体
        Object.DestroyImmediate(instanceRoot);
    }

    // 创建模型并且使用Laya Shader
    Material CreateMaterial(string path)
    {
        Material mat = new Material(Shader.Find("LayaAir3D/Mesh/Unlit"));
        // 读取纹理
        Texture2D tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path.Replace(".fbx", "_base.png"));
        if (tex == null){
            Debug.LogError("未找到该纹理贴图" + path.Replace(".fbx", "_base.png"));
            // 重新查找
            tex = AssetDatabase.LoadAssetAtPath<Texture2D>(path.Replace(".fbx", ".png"));
        }
        mat.mainTexture = tex;
        //保存材质球
        AssetDatabase.CreateAsset(mat, path.Replace(".fbx", ".mat"));
        AssetDatabase.Refresh();
        return mat;
    }

    // 添加状态
    void AddStateTransition(string path, AnimatorControllerLayer layer)
    {
        AnimatorStateMachine sm = layer.stateMachine;
        // 读取所有动画AnimationClip对象
        Object[] objs = AssetDatabase.LoadAllAssetsAtPath(path);
        foreach (Object o in objs)
        {
            if (o is AnimationClip)
            {
                AnimationClip newClip = o as AnimationClip;
                if (newClip == null || newClip.name.Contains("__preview__"))
                    continue;
                // 取出动画名子 添加到state里面
                AnimatorState state = sm.AddState(newClip.name);
                state.motion = newClip;
                // AnimatorStateTransition trans = sm.AddAnyStateTransition(state);
            }
        }
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
