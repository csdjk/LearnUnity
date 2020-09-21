using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
//-----------------------------【加载AssetBundle】-----------------------------
public class LoadFormFileAB : MonoBehaviour
{
    void Start()
    {
        string matPath = "AssetBundles/matrials.lcl";
        string objPath = "AssetBundles/obj.lcl";
        //-----------------------------【加载本地AssetBundle资源】-----------------------------
        // 1.从文件读取
        // 同步
        AssetBundle matAb = AssetBundle.LoadFromFile(matPath);//加载材质
        AssetBundle ab = AssetBundle.LoadFromFile(objPath);
        // 异步(需要协程)
        // AssetBundleCreateRequest abRequest = AssetBundle.LoadFromFileAsync(objPath);
        // yield return abRequest;
        // AssetBundle ab = abRequest.assetBundle;
        // 2.从内存读取
        // 同步
        // AssetBundle mat = AssetBundle.LoadFromMemory(File.ReadAllBytes(matPath));//加载材质
        // AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(objPath));
        // 异步同上
        // .........

        // 从ab加载预制体
        // GameObject prefab = ab.LoadAsset<GameObject>("assetBundle_Cube");
        GameObject[] prefabs = ab.LoadAllAssets<GameObject>();
        foreach (var item in prefabs)
        {
            // 实例化
            Instantiate(item);
        }

        //-----------------------------【加载远程AssetBundle资源】-----------------------------
        // 先启动webServer服务器
        // 通过UnityWebRequest加载
        // StartCoroutine(GetAssetBundle());




        //-----------------------------【卸载AssetBundle】-----------------------------
        matAb.Unload(false);
        // matAb.Unload(true);
        // ab.Unload(false);
        ab.Unload(true);

    }

    IEnumerator GetAssetBundle()
    {
        Debug.Log(Application.dataPath);
        string url = @"http://localhost/AssetBundles/obj.lcl";
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(url);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
            GameObject prefab = bundle.LoadAsset<GameObject>("assetBundle_Cube");
            Instantiate(prefab);
        }
    }

}
