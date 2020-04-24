using System.Net;
using UnityEditor;
using System.IO;
using System;
//-----------------------------【构建AssetBundles资源包】-----------------------------
public class BuildAssetBundles
{

    // 菜单选项目录
    [MenuItem("长生但酒狂的插件/Build AssetBundles")]
    static public void BuildAllAssetBundles()
    {
        // 创建文件目录
        string dir = "AssetBundles";
        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        // 构建
        // 参数1：路径
        // 参数2：压缩算法，none 默认
        // 参数3：设备参数，ios，Android，windows等等
        BuildPipeline.BuildAssetBundles(dir, BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows64);
        UnityEngine.Debug.Log("AssetBundle资源打包完成！");
    }
}
