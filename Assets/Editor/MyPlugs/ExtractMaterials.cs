// Create By 长生但酒狂
// Create Time 2020.4.24
using System.IO;
using UnityEngine;
using UnityEditor;

// ---------------------------【一键从FBX导出材质和设置shader】---------------------------
public class ExtractMaterials : EditorWindow
{
    // 当前选择路径
    public string modelPath = "Assets";
    public Rect modelRect;
    // shader
    int selectShaderIndex = 0;
    string[] shaderNameLists;
    // Scale Factor
    int scaleFactor = 1;


    [MenuItem("长生但酒狂的插件/导出材质和修改Shader")]
    public static void showWindow()
    {
        EditorWindow.CreateInstance<ExtractMaterials>().Show();
    }

    private void Awake()
    {
        // 获取所有Shader
        ShaderInfo[] shaderLists = ShaderUtil.GetAllShaderInfo();
        shaderNameLists = new string[shaderLists.Length];
        for (int i = 0; i < shaderLists.Length; i++)
        {
            shaderNameLists[i] = shaderLists[i].name;
            if (shaderNameLists[i] == "LayaAir3D/Mesh/PBR(Standard)")
            {
                selectShaderIndex = i;
            }
        }
    }

    public void OnGUI()
    {
        // 模型路径
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("模型路径 (鼠标拖拽文件夹到这里)");
        EditorGUILayout.Space();
        GUI.SetNextControlName("input1");//设置下一个控件的名字
        modelRect = EditorGUILayout.GetControlRect();
        modelPath = EditorGUI.TextField(modelRect, modelPath);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // Shader下拉列表
        EditorGUILayout.LabelField("当前选中Shader：");
        EditorGUILayout.Space();
        selectShaderIndex = EditorGUILayout.Popup(selectShaderIndex, shaderNameLists);
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // scale Factor
        EditorGUILayout.LabelField("模型缩放设置");
        scaleFactor = EditorGUILayout.IntField("Scale Factor：", scaleFactor);

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        // 文件拖拽
        DragFolder();

        // 设置模型缩放比例
        if (GUILayout.Button("设置模型缩放比例"))
        {
            ForEachModels(true);
        }
        EditorGUILayout.Space();

        // 导出材质
        if (GUILayout.Button("导出材质球"))
        {
            ForEachModels(false);
        }

        EditorGUILayout.Space();

        // 设置Shader
        if (GUILayout.Button("设置Shader"))
        {
            ForEachMaterials();
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
                if (modelRect.Contains(Event.current.mousePosition))
                    GUI.FocusControl("input1");
            }
            //拖入窗口并松开鼠标
            else if (Event.current.type == EventType.DragExited)
            {
                string dragPath = string.Join("", DragAndDrop.paths);
                // 判断区域
                if (modelRect.Contains(Event.current.mousePosition))
                    this.modelPath = dragPath;

                // 取消焦点(不然GUI不会刷新)
                GUI.FocusControl(null);
            }
        }
    }

    // 遍历模型
    void ForEachModels(bool isSetModelSetup)
    {
        string[] allPath = AssetDatabase.FindAssets("t:GameObject", new string[] { modelPath });
        // Debug.Log("-- allPath: " + allPath.Length);
        for (int i = 0, len = allPath.Length; i < len; i++)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(allPath[i]);
            // 设置模型
            if (isSetModelSetup)
                setModelSetup(filePath);
            else
                ExtractMaterialsFromFBX(filePath);
        }
        // 如果选取的是FBX模型文件
        if (allPath.Length == 0)
        {
            if (Path.GetExtension(modelPath) == ".fbx")
            {
                // 设置模型
                if (isSetModelSetup)
                    setModelSetup(modelPath);
                else
                    ExtractMaterialsFromFBX(modelPath);
            }
            else
            {
                Debug.LogError("当前选择目录未找到FBX文件: " + this.modelPath);
            }
        }

    }

    // 从fbx模型导出材质球
    public void ExtractMaterialsFromFBX(string assetPath)
    {
        // 材质目录
        string materialFolder = Path.GetDirectoryName(assetPath) + "/Material";
        // 如果不存在该文件夹则创建一个新的
        if (!AssetDatabase.IsValidFolder(materialFolder))
            AssetDatabase.CreateFolder(Path.GetDirectoryName(assetPath), "Material");
        // 获取 assetPath 下所有资源。
        Object[] assets = AssetDatabase.LoadAllAssetsAtPath(assetPath);
        foreach (Object item in assets)
        {
            if (item.GetType() == typeof(Material))
            {
                string path = System.IO.Path.Combine(materialFolder, item.name) + ".mat";
                // 为资源创建一个新的唯一路径。
                path = AssetDatabase.GenerateUniqueAssetPath(path);
                // 通过在导入资源（例如，FBX 文件）中提取外部资源，在对象（例如，材质）中创建此资源。
                string value = AssetDatabase.ExtractAsset(item, path);
                // 成功提取( 如果 Unity 已成功提取资源，则返回一个空字符串)
                if (string.IsNullOrEmpty(value))
                {
                    AssetDatabase.WriteImportSettingsIfDirty(assetPath);
                    AssetDatabase.ImportAsset(assetPath, ImportAssetOptions.ForceUpdate);
                }
            }
        }
        Debug.Log(Path.GetFileName(assetPath) + " 的 Material 导出成功!!");
    }

    // 修改模型设置
    void setModelSetup(string path)
    {
        ModelImporter importer = (ModelImporter)ModelImporter.GetAtPath(path);
        if (importer)
        {
            importer.globalScale = scaleFactor;
            importer.SaveAndReimport();
            Debug.Log("Scale Factor 设置成功: " + Path.GetFileName(path));
        }
    }

    // 遍历材质
    void ForEachMaterials()
    {
        string[] allPath = AssetDatabase.FindAssets("t:Material", new string[] { modelPath });
        if (allPath.Length == 0)
        {
            if (System.IO.Path.GetExtension(modelPath) == ".mat")
                setMaterialShader(modelPath);
            else
                Debug.LogError("当前目录未找到Material: " + modelPath);
            return;
        }

        for (int i = 0, len = allPath.Length; i < len; i++)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(allPath[i]);
            setMaterialShader(filePath);
        }
        Debug.Log("Material Shader 设置完成, 一共: " + allPath.Length + "个");
    }

    void setMaterialShader(string path)
    {
        Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
        Shader mShader = Shader.Find(shaderNameLists[selectShaderIndex]);
        if (mShader == null)
        {
            Debug.LogError("设置shader失败, 未找到该shader: " + shaderNameLists[selectShaderIndex]);
            return;
        }
        mat.shader = mShader;
        // AssetDatabase.Refresh();
    }

}
