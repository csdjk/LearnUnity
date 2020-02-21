using UnityEditor;
using System.IO;
//-----------------------------【构建AssetBundles资源包】-----------------------------
public class BuildAssetBundles
{
    // 菜单选项目录
    [MenuItem("Assets/Build AssetBundles")]
    static public void BuildAllAssetBundles()
    {
        // 文件目录
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
    }
}
