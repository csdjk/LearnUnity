
//-----------------------------------------------【脚本说明】-------------------------------------------------------
//      脚本功能：   在游戏运行时显示GUI说明，帧率相关信息
//      使用语言：   C#
//      开发所用IDE版本：Unity4.5 06f 、Visual Studio 2010    
//      2014年10月 Created by 浅墨    
//      更多内容或交流，请访问浅墨的博客：http://blog.csdn.net/poem_qianmo
//---------------------------------------------------------------------------------------------------------------------

//-----------------------------------------------【使用方法】-------------------------------------------------------
//      方法一：在Unity中拖拽此脚本到场景主摄像机之上
//      方法二：在Inspector中[Add Component]->[浅墨's Toolkit v1.0]->[ShowGameInfo]
//---------------------------------------------------------------------------------------------------------------------


using UnityEngine;
using System.Collections;

//添加组件菜单
[AddComponentMenu("浅墨's Toolkit v1.0/ShowGameInfo")]

public class ShowGameInfo : MonoBehaviour
{
    public bool ShowGUI = true;//是否显示GUI
    public bool LockCursor = false;//是否显示Cursor
    private bool buttonEscClicked = false;//用于标识ESC是否被按下
    private static int count = 0;//用于控制帧率的显示速度的count
    private static float milliSecond = 0;//毫秒数
    private static float fps = 0;//帧率值
    private static float deltaTime = 0.0f;//用于显示帧率的deltaTime

    void Start()
    {
        if (ShowGUI)
        {
            //隐藏光标
            Screen.lockCursor = false;

            if (LockCursor)
            {
                Screen.lockCursor = true;
            }

        }
    }
    void OnGUI()
    {
        if (ShowGUI)
        {

            //--------------------------【左上方的文字说明】-------------------------
            GUILayout.Label(" 欢迎来到浅墨的Unity3D游戏编程Demo~");
            GUILayout.Label(" 此为【浅墨Unity Shader编程】系列文章之一的配套场景");
            //开始Horizontal（水平组），用于创建水平的文字+按钮混合效果
            GUILayout.BeginHorizontal(); 
            GUILayout.Label(" 查看此场景配套博文请访问：");
            if (GUILayout.Button("http://blog.csdn.net/poem_qianmo"))
            {
                Application.OpenURL("http://blog.csdn.net/poem_qianmo");
            }
            //结束Horizontal（水平组）
            GUILayout.EndHorizontal();


            //左上方帧数显示
            if (++count>10)
            {
               count = 0;
               milliSecond= deltaTime * 1000.0f;
               fps=1.0f / deltaTime;
            }

            string text = string.Format(" 当前每帧渲染间隔：{0:0.0} ms ({1:0.} 帧每秒)", milliSecond, fps);
            GUILayout.Label(text);

            //---------------------------【左下方的控制说明】--------------------------
            // 开始一个GUI.Group
            GUI.BeginGroup(new Rect(0, Screen.height - 200, 300, 200));
            //创建一个矩形区域
            GUI.Box(new Rect(0, 0, 300, 200), "控制说明");
            //输出文字说明
            GUILayout.Label(" 程序退出： 键盘【Esc】");
            GUILayout.Label(" 位置移动： 键   盘【W】、【A】、【S】、【D】");
            GUILayout.Label("                  方向键【↑】、【↓】、【←】、【→】");
            GUILayout.Label("                  小键盘【8】、【4】、【5】、【6】");
            GUILayout.Label(" 视角移动： 鼠标");
            GUILayout.Label(" 跳       跃： 键盘【Space】");

            //结束GUI.Group
            GUI.EndGroup();

            //记录Esc键是否有被按下
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                buttonEscClicked = true;
            }

            //如果Esc键被按下了，解锁鼠标、显示光标，显示退出对话框
            if (buttonEscClicked)
            {
                //解锁鼠标、显示光标
                Screen.lockCursor = false;
                Cursor.visible = true;
                //显示退出对话框
                GUI.Box(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 60, 100, 120), "是否退出？");
                if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 - 20, 50, 24), "退出"))
                {
                    //退出程序
                    Application.Quit();
                }

                if (GUI.Button(new Rect(Screen.width / 2 - 25, Screen.height / 2 + 20, 50, 24), "返回"))
                {
                    //置ESC键标识为false
                    buttonEscClicked = false;
                    //恢复显示图标
                    Screen.lockCursor = true;
                    Cursor.visible = false;
                }
            }
        }
    }

    void Update()
    {
        //帧数显示的计时delataTime
        deltaTime += (Time.deltaTime - deltaTime) * 0.1f;
    }

}

