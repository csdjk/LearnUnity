//-----------------------------------------------【脚本说明】-------------------------------------------------------
//      脚本功能：   在游戏运行时显示Logo
//      使用语言：   C#
//      开发所用IDE版本：Unity4.5 06f 、Visual Studio 2010    
//      2014年10月 Created by 浅墨    
//      更多内容或交流，请访问浅墨的博客：http://blog.csdn.net/poem_qianmo
//---------------------------------------------------------------------------------------------------------------------

//-----------------------------------------------【使用方法】-------------------------------------------------------
//      第一步：在Unity中拖拽此脚本到主摄像机之上，或在Inspector中[Add Component]->[浅墨's Toolkit v1.0]->[ShowLogo]
//      第二步：在Inspector里,[Show Logo]栏中的[Logo Texture]参数中选择你自己的logo或其他图片
//      第三步：在[Show Logo]栏中[corner Display Logo]设置LOGO显示的位置
//---------------------------------------------------------------------------------------------------------------------

//------------------------------------------【命名空间包含部分】----------------------------------------------------
//  说明：命名空间包含
//----------------------------------------------------------------------------------------------------------------------
#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.Collections;



//------------------------------------------【枚举体】--------------------------------------------------------------
//  说明：代表四个显示角落的枚举体
//---------------------------------------------------------------------------------------------------------------------
public enum DisplayCorner
{
    TopLeft,
    TopRight,
    BottomLeft,
    BottomRight
}

//添加组件菜单
[AddComponentMenu("浅墨's Toolkit v1.0/ShowLogo")]
//开始ShowLogo类
public class ShowLogo : MonoBehaviour
{

    //------------------------------------------【变量声明部分】----------------------------------------------------
    //  说明：变量声明部分
    //------------------------------------------------------------------------------------------------------------------
    public Texture2D logoTexture = null;//Logo图片
    public DisplayCorner cornerDisplayLogo = DisplayCorner.BottomRight;//显示的角落
    public Vector2 logoPixelOffset = new Vector2(0, 0);//像素偏移量
    public Vector2 logoPercentOffset = new Vector2(0, 0);//百分比偏移量
    private Rect textureRect = new Rect(0, 0, 0, 0);//纹理所在矩形



    //-----------------------------------------【Reset()函数】------------------------------------------------------
    // 说明：重置时的操作
    //------------------------------------------------------------------------------------------------------------------
    void Reset()
    {
        logoTexture = Resources.Load("浅墨的Logo") as Texture2D;
    }

    //-----------------------------------------【updateTexRect()函数】------------------------------------------
    // 说明：用于更新显示Logo的位置
    //------------------------------------------------------------------------------------------------------------------
    void updateTextureRect()
    {
        if (logoTexture)
        {
            //获取一些值
            float textureWidth = logoTexture.width;
            float textureHeight = logoTexture.height;
            float cameraWidth = 0f;
            float cameraHeight = 0f;

            //检查是否有摄像机依附，并返回像素宽和高
            if (this.GetComponent<Camera>())
            {
                cameraWidth = GetComponent<Camera>().pixelWidth;
                cameraHeight = GetComponent<Camera>().pixelHeight;
            }
            //否则用Camera.main
            else if (Camera.main)
            {
                cameraWidth = Camera.main.pixelWidth;
                cameraHeight = Camera.main.pixelHeight;
            }
            //前面两个都不行，就用Camera.current
            else if (Camera.current)
            {
                cameraWidth = Camera.current.pixelWidth;
                cameraHeight = Camera.current.pixelHeight;
            }
            //计算偏移量
            float offsetX = logoPixelOffset.x + logoPercentOffset.x * cameraWidth * 0.01f;
            float offsetY = logoPixelOffset.y + logoPercentOffset.y * cameraHeight * 0.01f;

            //开始一个switch，确定四个角落的坐标位置
            switch (cornerDisplayLogo)
            {
                //左上角
                case DisplayCorner.TopLeft:
                    textureRect.x = offsetX;
                    textureRect.y = offsetY;
                    break;

                //右上角
                case DisplayCorner.TopRight:
                    textureRect.x = cameraWidth - offsetX - textureWidth;
                    textureRect.y = offsetY;
                    break;

                //左下角
                case DisplayCorner.BottomLeft:
                    textureRect.x = offsetX;
                    textureRect.y = cameraHeight - offsetY - textureHeight;
                    break;

                //右下角
                case DisplayCorner.BottomRight:
                    textureRect.x = cameraWidth - offsetX - textureWidth;
                    textureRect.y = cameraHeight - offsetY - textureHeight;
                    break;
            };

            //确定图片宽高
            textureRect.width = textureWidth;
            textureRect.height = textureHeight;
        }
    }

    //-----------------------------------------【OnGUI()函数】-----------------------------------------------------
    // 说明：游戏运行时GUI的显示
    //------------------------------------------------------------------------------------------------------------------
    void OnGUI()
    {
        //更新图片的坐标位置
        updateTextureRect();
        //绘制图片
        if (logoTexture)
        {
            GUI.DrawTexture(textureRect, logoTexture);
        }
    }

}

#endif