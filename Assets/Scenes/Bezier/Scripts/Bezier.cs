using System;
using System.Collections;
using System.Collections.Generic;
using CustomCoroutine;
using UnityEngine;
namespace BezierUtils
{
    public enum EaseType
    {
        Linear,
        InQuad,
        OutQuad,
        InCubic,
        OutCubic,
        InSine,
        OutSine,
        InOutSine,
        InOutQuad,
        InOutCubic,
        InQuart,
        OutQuart,
        InOutQuart,
        InQuint,
        OutQuint,
        InOutQuint,
        InExpo,
        OutExpo,
        InOutExpo,
        InCirc,
        OutCirc,
        InOutCirc,
        InElastic,
        OutElastic,
        InOutElastic,
        InBack,
        OutBack,
        InOutBack,
        InBounce,
        Flash,
        OutBounce,
        InOutBounce,
        InFlash,
        OutFlash,
        InOutFlash,
    }
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

        private Vector3[] pointArr;

        private List<BVector3> _pointLists = new List<BVector3>();
        private BVector3 _prevPos;
        private EaseType ease;

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
            _prevPos = new BVector3(this.pointArr[0], 0, 0);
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
            this.pointArr = pointArr;
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


        public void ComputeBezier(float dt, float runTime)
        {
            // 把时间从 [0,runTime] 映射到 [0,1] 之间
            float t = this._currentRunTime / runTime;

            float x = 0, y = 0, z = 0;
            //控制点数组
            float n = this.pointArr.Length - 1;
            for (int i = 0; i < this.pointArr.Length; i++)
            {

                Vector3 item = pointArr[i];
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



        private float Evaluate(EaseType type, float time, float duration)
        {
            switch (type)
            {
                case EaseType.Linear:
                    return time / duration;
                case EaseType.InQuad:
                    return (time /= duration) * time;
                case EaseType.OutQuad:
                    return -(time /= duration) * (time - 2);
                case EaseType.InCubic:
                    return (time /= duration) * time * time;
                case EaseType.OutCubic:
                    return ((time = time / duration - 1) * time * time + 1);
            }
            return time / duration;
        }


        public BVector3 ComputeBezierPoint(float curTime, float runTime)
        {
            // 把时间从 [0,runTime] 映射到 [0,1] 之间
            float t = Evaluate(ease, curTime, runTime);

            float x = 0, y = 0, z = 0;
            //控制点数组
            float n = pointArr.Length - 1;
            for (int i = 0; i < pointArr.Length; i++)
            {

                Vector3 item = pointArr[i];
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
            float length = (float)(Math.Sqrt(Math.Pow(_prevPos.pos.x - x, 2) + Math.Pow(_prevPos.pos.y - y, 2) + Math.Pow(_prevPos.pos.z - z, 2)));
            BVector3 v3 = new BVector3(new Vector3(x, y, z), length, 0);

            // 存储当前节点z
            _pointLists.Add(v3);
            _prevPos = v3;
            // 累计长度
            _totalLength += length;
            return v3;
        }

        public IEnumerator Move(Transform ts, Vector3[] pointArr, float duration, EaseType ease = EaseType.Linear)
        {
            this._runTime = duration;
            this.pointArr = pointArr;
            this.ease = ease;

            ResetData();
            ts.position = pointArr[0];

            for (float timeCount = 0; timeCount < duration; timeCount += Time.deltaTime)
            {
                ts.position = ComputeBezierPoint(timeCount, duration).pos;
                // Debug.Log("当前时间：" + timeCount);
                yield return 0;
            }
        }

        public void MovePath(Transform ts, Vector3[] pointArr, float duration, EaseType ease = EaseType.Linear)
        {
            CoroutineManager.Instance.StartCoroutine(Move(ts, pointArr, duration, ease));
            // StartCoroutine(MovePath())
        }

    }
}