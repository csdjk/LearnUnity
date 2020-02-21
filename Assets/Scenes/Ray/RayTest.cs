using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//编辑器下运行
[ExecuteInEditMode]
public class RayTest : MonoBehaviour {
    public AnimationCurve curve;

    //初始位置
    private Vector3 startPos;
    //终点
    private Vector3 endPos;
    //方向
    private Vector3 dir;
    //长度
    private int distance = 10;
    //射线
    private Ray ray;

    //--------------------
    //投射类型
    public float m_CastType = 0;
    //最大距离
    public float m_MaxDistance = 4.0f;
    public float m_Speed = 20.0f;
    bool m_HitDetect;
    Collider m_Collider;
    RaycastHit m_Hit;

    void Start () {
        startPos = transform.position;
        endPos = transform.forward * distance;
        dir = transform.forward;
        //-------------------
        m_Collider = GetComponent<Collider> ();
    }

    // void Update () {
    //     // LineRay();
    //     BodyCast();
    // }

    void LineRay () {
        // Ray camerRay = Camera.main.ScreenPointToRay (Input.mousePosition);

        ray = new Ray (startPos, dir);
        //单体检测
        RaycastHit hit;
        Physics.Raycast (ray, out hit, distance);
        // Physics.Raycast(ray,out hit,distance,LayerMask.GetMask("ball"));

        //检测所有
        // RaycastHit[] hits = Physics.RaycastAll(ray,distance);
        RaycastHit[] hits = Physics.RaycastAll (ray, distance, LayerMask.GetMask ("ball"));

        //两点检测: 如果有任何碰撞器与start和end之间的直线相交，则返回true
        bool isCrash = Physics.Linecast (startPos, endPos);
        // bool isCrash = Physics.Linecast(startPos,endPos,LayerMask.GetMask("ball"));
        // Debug.Log(isCrash);

    }

    void BodyCast () {
        ray = new Ray (startPos, dir);
        RaycastHit hit;

        // bool isCrash = Physics.BoxCast (startPos, transform.localScale, dir, out hit);
        // Physics.BoxCast(startPos,transform.localScale,dir,out hit,transform.rotation,distance,LayerMask.GetMask("ball"));
        // Debug.Log(hit.collider.name);

        bool isHit = Physics.SphereCast (startPos, transform.localScale.x, dir, out hit, m_MaxDistance);

        Debug.Log (isHit);
    }

    //------------------debug 绘制射线-----------------------------------

    bool MyCast () {
        bool isHit = false;
        switch (m_CastType) {
            case 0:
                isHit = Physics.BoxCast (m_Collider.bounds.center, transform.localScale, transform.forward, out m_Hit, transform.rotation, m_MaxDistance);
                break;
            case 1:
                isHit = Physics.SphereCast (m_Collider.bounds.center, transform.localScale.x, transform.forward, out m_Hit, m_MaxDistance);
                break;
        }
        return isHit;
    }

    void DrawLineObj () {
        float distance;
        //检查是否有击中
        if (m_HitDetect) {
            //GameObject(游戏物体)到目标的距离
            distance = m_Hit.distance;
        } else {
            //最大距离
            distance = m_MaxDistance;
        }
        //从GameObject(游戏物体)中向前画一条射线
        Gizmos.DrawRay (transform.position, transform.forward * distance);

        switch (m_CastType) {
            case 0:
                Gizmos.DrawWireCube (transform.position + transform.forward * distance, transform.localScale);
                break;
            case 1:
                Gizmos.DrawWireSphere (transform.position + transform.forward * distance, transform.localScale.x);
                break;
        }
    }

    void Update () {
        //Simple movement in x and z axes
        float xAxis = Input.GetAxis ("Horizontal") * m_Speed;
        float zAxis = Input.GetAxis ("Vertical") * m_Speed;
        transform.Translate (new Vector3 (xAxis, 0, zAxis));
    }

    void FixedUpdate () {
        //Test to see if there is a hit using a BoxCast
        //Calculate using the center of the GameObject's Collider(could also just use the GameObject's position), half the GameObject's size, the direction, the GameObject's rotation, and the maximum distance as variables.
        //Also fetch the hit data
        m_HitDetect = MyCast ();
        if (m_HitDetect) {
            //Output the name of the Collider your Box hit
            Debug.Log ("Hit : " + m_Hit.collider.name);
        } else {
            Debug.Log ("no collider！");
        }
    }

    //将BoxCast画成一个小装置来显示它当前正在测试的位置。点击Gizmos按钮来查看这个
    void OnDrawGizmos () {
        Gizmos.color = Color.red;
        DrawLineObj ();
    }

}