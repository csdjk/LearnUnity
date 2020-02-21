
## 1.构建AssetBundle
构建脚本路径：Editor/BuildAssetBundles

先给需要打包的资源指定 AssetBundle 标签   
然后点击菜单栏 Assets/Build AssetBundles 构建打包


## 2.加载AssetBundle资源

### 1) 加载本地资源
```csharp
// 从文件读取
AssetBundle ab = AssetBundle.LoadFromFile(Path);
// 异步(需要协程)
// AssetBundleCreateRequest abRequest = AssetBundle.LoadFromFileAsync(objPath);
// yield return abRequest;
// AssetBundle ab = abRequest.assetBundle;


// 从内存读取
AssetBundle ab = AssetBundle.LoadFromMemory(File.ReadAllBytes(Path));
// 异步同上
// AssetBundle ab = AssetBundle.LoadFromMemoryAsync(File.ReadAllBytes(Path));
// .....
```

### 2) 加载远程资源
需要先把打包好的`AssetBundle`文件放在`webSereve`目录下，然后`启动webSereve服务器`。

```csharp
void Start()
{
    // 开启协程
    StartCoroutine(GetAssetBundle());
}

IEnumerator GetAssetBundle()
{
    string uri = @"http://localhost/AssetBundles/obj.lcl";
    UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(uri);
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
```


## 3.卸载AssetBundle
```csharp
ab.Unload(false);
ab.Unload(true);
```

## 4 AssetBundle浏览插件

[AssetBundles-Browser GitHub地址](https://github.com/Unity-Technologies/AssetBundles-Browser/releases)

下载解压文件到Editor文件夹即可

点击 window ->  AssetBundle Browser 打开查看




