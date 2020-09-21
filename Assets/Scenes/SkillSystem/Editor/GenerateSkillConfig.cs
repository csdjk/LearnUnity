using System.IO;
using UnityEngine;
using UnityEditor;

public class GenerateSkillConfig {

    private static string skillPath = "Assets/Scenes/SkillSystem/Resources";

    [MenuItem("长生但酒狂的插件/技能系统/生成技能配置文件")]
    public static void Generate(){
        // 查找 resource 文件下所有预制体
        string[] resFiles = AssetDatabase.FindAssets("t:prefab", new string[] { skillPath });

        for (int i = 0; i < resFiles.Length; i++)
        {
            resFiles[i] = AssetDatabase.GUIDToAssetPath(resFiles[i]);
            string fileName = Path.GetFileNameWithoutExtension(resFiles[i]);
            string filePath = resFiles[i].Replace($"{skillPath}/",string.Empty).Replace(".prefab",string.Empty);
            resFiles[i] = $"{fileName}={filePath}";
        }
        File.WriteAllLines("Assets/StreamingAssets/ConfigMap.txt",resFiles);
        // 刷新文件夹
        AssetDatabase.Refresh();
    }
}