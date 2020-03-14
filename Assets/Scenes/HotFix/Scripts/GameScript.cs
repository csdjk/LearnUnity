using UnityEngine;
using UnityEngine.EventSystems;
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
            if (EventSystem.current.IsPointerOverGameObject())
            {
                Debug.Log("点击到UGUI的UI界面");
            }
            else
            {
                //创建 cube  (后面会通过热更 lua脚本替换掉这里，使之生成Sphere)
                GameObject cubeGo = Resources.Load("Cube") as GameObject;
                // 在鼠标点击的地方实例cube
                Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				RaycastHit hit;
				if (Physics.Raycast (ray, out hit)) {
					Debug.Log (hit.point);
					GameObject cube = GameObject.Instantiate (cubeGo, hit.point + new Vector3(0,1,0), transform.rotation)as GameObject;
                }
            }

        }
    }

    //射线 - 用于xlua调用 避免重载问题
    public static bool RayFunction(Ray ray, out RaycastHit hit)
    {
        return Physics.Raycast(ray, out hit);
    }

}
