using UnityEngine;
using System.Collections;

public class ShowUI : MonoBehaviour
{
    public Texture2D midBottomPic;//用于修饰的横条

    void OnGUI()
    {
        if (midBottomPic)
        {
            //--------------------------【中下方横条的绘制】-------------------------
            GUI.DrawTexture(new Rect(Screen.width / 2 - midBottomPic.width / 2, 0, midBottomPic.width, midBottomPic.height), midBottomPic);
        }
     
    }
}
