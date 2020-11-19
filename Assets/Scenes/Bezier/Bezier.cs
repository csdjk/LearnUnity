using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class BVector3
{
    public Vector3 pos;
    public float length; //上一点到当前点的距离
    public float lenScale;// 

    public BVector3(Vector3 pos, float length, float lenScale)
    {
        this.pos = pos;
        this.length = length;
        this.lenScale = lenScale;
    }
}


public class Bezier
{
    private static Bezier instance;
    // private Bezier() { }

    private float _runTime;
    private float _count;
    private float _currentRunTime;
    private float _totalLength;

    private Vector3[] _pointArr;

    private List<BVector3> _pointLists = new List<BVector3>();
    private BVector3 _prevPos;


    public static Bezier Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Bezier();
            }
            return instance;
        }
    }


    // 重置数据
    private void ResetData()
    {
        // 点集合
        _pointLists.Clear();
        // 线段总长度
        _totalLength = _currentRunTime = 0;
        // 初始位置
        _prevPos = new BVector3(this._pointArr[0], 0, 0);
    }

    /**
     * 
     * @param pointArr 贝塞尔关键点列表
     * @param allTime 运行总时间
     * @param smoothness 曲线平滑度
     */
    public List<BVector3> CreateBezierList(Vector3[] pointArr, float allTime = 2, float smoothness = 20)
    {
        this._runTime = allTime;
        this._pointArr = pointArr;
        this._count = smoothness;

        this.ResetData();
        // 分割时间
        float dt = this._runTime / this._count;
        // 开始分割曲线
        for (float i = 0, len = this._count + 1; i < len; i++)
        {
            ComputeBezier(dt, this._runTime);
        }
        return this._pointLists;
    }


    private void ComputeBezier(float dt, float runTime)
    {
        // 把时间从 [0,runTime] 映射到 [0,1] 之间
        float t = this._currentRunTime / runTime;

        float x = 0, y = 0, z = 0;
        //控制点数组
        float n = this._pointArr.Length - 1;
        for (int i = 0; i < this._pointArr.Length; i++)
        {

            Vector3 item = _pointArr[i];
            if (i == 0)
            {
                x += item.x * (float)(Math.Pow((1 - t), n - i) * Math.Pow(t, i));
                y += item.y * (float)(Math.Pow((1 - t), n - i) * Math.Pow(t, i));
                z += item.z * (float)(Math.Pow((1 - t), n - i) * Math.Pow(t, i));
            }
            else
            {
                //factorial为阶乘函数
                x += Factorial(n) / Factorial(i) / Factorial(n - i) * item.x * (float)Math.Pow((1 - t), n - i) * (float)Math.Pow(t, i);
                y += Factorial(n) / Factorial(i) / Factorial(n - i) * item.y * (float)Math.Pow((1 - t), n - i) * (float)Math.Pow(t, i);
                z += Factorial(n) / Factorial(i) / Factorial(n - i) * item.z * (float)Math.Pow((1 - t), n - i) * (float)Math.Pow(t, i);
            }
        }

        // 计算两点距离
        float length = (float)(Math.Sqrt(Math.Pow(this._prevPos.pos.x - x, 2) + Math.Pow(this._prevPos.pos.y - y, 2) + Math.Pow(this._prevPos.pos.z - z, 2)));
        BVector3 v3 = new BVector3(new Vector3(x, y, z), length, 0);

        // 存储当前节点z
        this._pointLists.Add(v3);
        this._prevPos = v3;
        // 累计长度
        this._totalLength += length;
        // 累计时间
        this._currentRunTime += dt;
    }

    //阶乘
    private float Factorial(float i)
    {
        float n = 1;
        for (float j = 1; j <= i; j++)
            n *= j;
        return n;
    }

    public float GetCurveLength()
    {
        return this._totalLength;
    }
}
