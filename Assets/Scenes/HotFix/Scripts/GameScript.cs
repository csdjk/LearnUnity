using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;
//-----------------------------【游戏脚本】-----------------------------
[Hotfix]
public class GameScript : MonoBehaviour
{
    public int num1 = 100;
    private int num2 = 200;
    void Update()
    {
        // 鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            //创建 cube  (后面会通过热更 lua脚本替换掉这里，使之生成Sphere)
            GameObject cubeGo = Resources.Load("Cube") as GameObject;
            Instantiate(cubeGo);
        }
    }
}
