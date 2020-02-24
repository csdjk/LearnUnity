### AssetBundle和XLua热更新

>大概思路：标记需要热更的资源，打包构建AssetBundle资源（包括lua脚本），上传到服务器，客户端通过服务器下载读取资源，执行lua脚本即可。

#### Unity Xlua中HotFix的环境配置

1. 打开HotFix的宏。打开`File -> Build Setting -> Player Setting ->Scriptsing Define Symbols` ，然后在里面输入宏`HOTFIX_ENABLE`，按ENTER后，Unity后台会编译一下。编译过后会在编译器XLua下面多一个“Hotfix Inject in Editor”（`XLua -> Hotfix Inject in Editor`）。

2. 将XLua-master文件夹下的`Tools`文件夹复制到我们项目的根文件下与Assets文件夹同级。如果没不这样做，当我们更改脚本并“Generate Code”生成代码后，当点击“Hotfix Inject in Editor”会报错。
;

#### 1.首先创建一个打包AssetBundle资源的脚本 [BuildAssetBundles.cs](../../Editor/BuildAssetBundles.cs)，并且放在Editor文件夹下

#### 2. 点击编辑器菜单 Asset -> Build AssetBundles 构建资源

#### 3. 把根路径的AssetBundles文件夹 上传到服务器，该demo测试服务器为 [webSereve](../../webSereve/README.md)，点击 `NetBox2.exe` 运行即可启动服务器， 并且把AssetBundles放在服务器根目录下。

#### 4. 运行工程，通过之前写好的[DownLoad.cs](./Scripts/DownLoad.cs)脚本，运行读取下载好的lua脚本即可。

说明：该demo 运行时 点击`鼠标左键` 原本应该是创建的`Cube`，通过`热更后`，应该是创建一个 `Sphere`









