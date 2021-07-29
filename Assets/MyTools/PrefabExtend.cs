using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

static public class PrefabExten
{
    [MenuItem("CONTEXT/Transform/SavePrefab")]
    static public void SavePrefab()
    {
        Debug.LogWarning(Selection.activeGameObject.name);
        var go = Selection.activeGameObject;
        string prefabPath = GetPrefabAssetPath(go);
        if (prefabPath == null) return;
        
        if (prefabPath.EndsWith(".prefab") == false) return;
        bool succes;
        // 使用该函数，在给定路径上，从给定的游戏对象创建一个预制件资源（包括场景中的任何子项），而不修改输入对象。
        // GameObject newPrefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath, out succes);
        // 使用该函数，在给定路径上，从给定的游戏对象创建一个预制件资源（包括场景中的任何子项），同时让给定游戏对象进入新预制件的实例。
        PrefabUtility.SaveAsPrefabAssetAndConnect(go,prefabPath,InteractionMode.UserAction,out succes);
        if (succes)
        {
            Debug.LogWarning("prefab保存成功：" + prefabPath);
        }
        else
        {
            Debug.LogWarning("prefab保存失败：" + prefabPath);
        }
    }

     // 快捷键 保存预设 crtl + alt + s
    [MenuItem("MyTools/CustomKeys/SavePrefab %&s")]
    static public void QuickSavePrefab()
    {
        if(!EditorApplication.isPlaying){
            SavePrefab();
        }
    }

    /// <summary>
    /// 获取预制体资源路径。
    /// </summary>
    /// <param name="gameObject"></param>
    /// <returns></returns>
    public static string GetPrefabAssetPath(GameObject gameObject)
    {
#if UNITY_EDITOR
        // Project中的Prefab是Asset不是Instance
        if (UnityEditor.PrefabUtility.IsPartOfPrefabAsset(gameObject))
        {
            // 预制体资源就是自身
            return UnityEditor.AssetDatabase.GetAssetPath(gameObject);
        }

        // Scene中的Prefab Instance是Instance不是Asset
        if (UnityEditor.PrefabUtility.IsPartOfPrefabInstance(gameObject))
        {
            // 获取预制体资源
            var prefabAsset = UnityEditor.PrefabUtility.GetCorrespondingObjectFromOriginalSource(gameObject);
            return UnityEditor.AssetDatabase.GetAssetPath(prefabAsset);
        }

        // PrefabMode中的GameObject既不是Instance也不是Asset
        var prefabStage = UnityEditor.Experimental.SceneManagement.PrefabStageUtility.GetPrefabStage(gameObject);
        if (prefabStage != null)
        {
            // 预制体资源：prefabAsset = prefabStage.prefabContentsRoot
            return prefabStage.prefabAssetPath;
        }
#endif

        // 不是预制体
        return null;
    }

}
