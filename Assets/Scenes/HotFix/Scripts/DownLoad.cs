using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.Networking;
using XLua;
//-----------------------------【下载资源】-----------------------------
public class DownLoad : MonoBehaviour
{
    // private string path = @"file://E:\WorkSpaces\Unity\XLuaFix\AssetBundles\newobj.u3d";
    private string verPath = @"http://localhost/AssetBundles/version.txt";
    private string path = @"http://localhost/AssetBundles/newobj.u3d";

    public AssetBundle assetBundle;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("run DownLoad Script");
        // 启动协程
        StartCoroutine(GetAssetBundle(ExcuteHotFix));
        StartCoroutine(GetVersion());
    }

    // 获取版本号
    IEnumerator GetVersion()
    {

        UnityWebRequest www = UnityWebRequest.Get(verPath);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {
            // 将结果显示为文本
            Debug.Log("当前版本号 : " + www.downloadHandler.text);
        }

    }

    //-----------------------------【从服务器下载热更资源】-----------------------------
    IEnumerator GetAssetBundle(Action callBack)
    {
        UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(path);
        yield return www.SendWebRequest();
        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log("DownLoad Err: " + www.error);
        }
        else
        {
            assetBundle = DownloadHandlerAssetBundle.GetContent(www);
            TextAsset hot = assetBundle.LoadAsset<TextAsset>("luaScript.lua.txt");

            string newPath = Application.persistentDataPath + @"/luaScript.lua.txt";
            if (!File.Exists(newPath))
            {
                // Create后如果不主动释放资源就会被占用,下次打开会报错，所以一定要加上 .Dispose()
                File.Create(newPath).Dispose();
            }

            File.WriteAllText(newPath, hot.text);

            Debug.Log("下载资源成功！new Path : " + newPath);
            // 下载成功后 读取执行lua脚本
            callBack();
        }
    }

    //-----------------------------【执行热更脚本】-----------------------------
    public  void ExcuteHotFix()
    {
        Debug.Log("开始执行热更脚本 luaScript");
        LuaEnv luaenv = new LuaEnv();
        luaenv.AddLoader(MyLoader);
        luaenv.DoString("require 'luaScript'");
    }

    // 自定义Loader
    public byte[] MyLoader(ref string filePath)
    {
        // 读取下载的脚本资源
        string newPath = Application.persistentDataPath + @"/" + filePath + ".lua.txt";
        Debug.Log("执行脚本路径：" + newPath);
        string txtString = File.ReadAllText(newPath);
        return System.Text.Encoding.UTF8.GetBytes(txtString);
    }
}
