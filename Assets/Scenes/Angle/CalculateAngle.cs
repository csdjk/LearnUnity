using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalculateAngle : MonoBehaviour
{
    public static float degToRad = 3.14159265358f / 180f;
    public static float radToDeg = 180f / 3.14159265358f;

    public Transform dir1;
    public Transform dir2;


    // public static float dir2Angle(Vector3 dir)
    // {
    //     var nor = Vector3.zero;
    //     Vector3.Normalize(dir);
    //     return Mathf.Atan2(nor.x, nor.z) * radToDeg;
    // }

    // //方向角度转方向向量
    // public static angle2Dir(angle: number)
    // {
    //     let rad = angle * this.degToRad
    //     return new Laya.Vector3(Math.sin(rad), 0, Math.cos(rad))
    // }

    void Start()
    {

    }

    /// <summary>
    /// 返回两向量的夹角，带方向。
    /// </summary>
    /// <param name="v1">向量1</param>
    /// <param name="v2">向量2</param>
    /// <param name="n">法向量（在法向量为n的平面所构成的角度）</param>
    /// <returns></returns>
    public static float AngleSigned(Vector3 v1, Vector3 v2, Vector3 n)
    {
        return Mathf.Atan2(
            Vector3.Dot(n, Vector3.Cross(v1, v2)),
            Vector3.Dot(v1, v2)) * Mathf.Rad2Deg;
    }
    /// <summary>
    /// 返回两向量的夹角，带方向。(在z为0的平面)
    /// </summary>
    /// <param name="v1">向量1</param>
    /// <param name="v2">向量2</param>
    /// <returns></returns>
    public static float AngleSigned2(Vector3 v1, Vector3 v2)
    {
        var dir1 = v1.normalized;
        var dir2 = v2.normalized;
        var dot = Vector3.Dot(dir1, dir2);
        var theat = Mathf.Acos(dot);
        var normal = Vector3.Cross(dir1, dir2);
        theat = normal.z > 0 ? theat : -theat;
        return theat * Mathf.Rad2Deg;
    }

    void Update()
    {
        Debug.Log(AngleSigned(dir1.position, dir2.position, Vector3.forward).ToString());
        Debug.Log(AngleSigned2(dir1.position, dir2.position).ToString());
    }
}
