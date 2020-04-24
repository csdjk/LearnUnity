// Create By 长生但酒狂
// Create Time 2020.4.24

using System.IO;
using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;

// ---------------------------【从FBX导出材质和设置shader】---------------------------
public class ExtractMaterials : EditorWindow
{
    // 当前选择路径
    public string modelPath = "Assets";
    public Rect modelRect;
    // shader
    public string shaderName = "LayaAir3D/Mesh/PBR(Standard)";

    [MenuItem("长生但酒狂的插件/导出材质和修改Shader")]
    public static void showWindow()
    {
        EditorWindow.CreateInstance<ExtractMaterials>().Show();
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

        // Shader名称
        EditorGUILayout.LabelField("Shader名称 (如: LayaAir3D/Mesh/PBR(Standard))");
        EditorGUILayout.Space();
        GUI.SetNextControlName("input2");//设置下一个控件的名字
        shaderName = EditorGUILayout.TextField(shaderName);
        EditorGUILayout.Space();

        // 文件拖拽
        DragFolder();

        // 导出材质
        if (GUILayout.Button("导出材质球"))
        {
            StartExtractMaterialsFromFBX();
        }

        EditorGUILayout.Space();

        // 设置Shader
        if (GUILayout.Button("修改Shader"))
        {
            setMaterialShader();
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

    void StartExtractMaterialsFromFBX()
    {
        string[] allPath = AssetDatabase.FindAssets("t:GameObject", new string[] { modelPath });
        Debug.Log("-- allPath: " + allPath.Length);
        for (int i = 0, len = allPath.Length; i < len; i++)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(allPath[i]);
            // 设置模型
            setModelSetup(filePath);
            ExtractMaterialsFromFBX(filePath);
        }
        // 如果选取的是FBX模型文件
        if (allPath.Length == 0)
        {
            if (Path.GetExtension(modelPath) == ".fbx")
            {
                // 设置模型
                setModelSetup(modelPath);
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

        HashSet<string> hashSet = new HashSet<string>();
        IEnumerable<Object> enumerable = from x in AssetDatabase.LoadAllAssetsAtPath(assetPath)
                                         where x.GetType() == typeof(Material)
                                         select x;
        foreach (Object item in enumerable)
        {
            string path = System.IO.Path.Combine(materialFolder, item.name) + ".mat";
            path = AssetDatabase.GenerateUniqueAssetPath(path);
            string value = AssetDatabase.ExtractAsset(item, path);
            if (string.IsNullOrEmpty(value))
            {
                hashSet.Add(assetPath);
            }
        }

        foreach (string item2 in hashSet)
        {
            AssetDatabase.WriteImportSettingsIfDirty(item2);
            AssetDatabase.ImportAsset(item2, ImportAssetOptions.ForceUpdate);
        }
        Debug.Log(Path.GetFileName(assetPath) + " 的 Material 导出成功!!");
    }


    void setMaterialShader()
    {
        string[] allPath = AssetDatabase.FindAssets("t:Material", new string[] { this.modelPath });
        if (allPath.Length == 0)
        {
            Debug.LogError("当前选择目录未找到Material: " + this.modelPath);
            return;
        }

        for (int i = 0, len = allPath.Length; i < len; i++)
        {
            string filePath = AssetDatabase.GUIDToAssetPath(allPath[i]);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(filePath);
            Shader mShader = Shader.Find(shaderName);
            if (mShader == null)
            {
                Debug.LogError("设置shader失败, 未找到该shader: " + shaderName);
                return;
            }
            mat.shader = mShader;
            // AssetDatabase.Refresh();
        }
        Debug.Log("Material Shader 设置完成, 一共: " + allPath.Length + "个");
    }

    // 修改模型设置
    void setModelSetup(string path)
    {
        ModelImporter importer = (ModelImporter)ModelImporter.GetAtPath(path);
        if (importer)
        {
            importer.animationType = ModelImporterAnimationType.Generic;
            importer.globalScale = 100;
            importer.SaveAndReimport();
        }
    }

}
