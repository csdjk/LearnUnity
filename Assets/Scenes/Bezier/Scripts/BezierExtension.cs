using System;
using System.Collections;
using System.Collections.Generic;
using CustomCoroutine;
using UnityEngine;

namespace BezierUtils
{
    public static class BezierExtension
    {
        //阶乘
        private static float Factorial(float i)
        {
            float n = 1;
            for (float j = 1; j <= i; j++)
                n *= j;
            return n;
        }


        private static float Evaluate(EaseType easeType, float time, float duration, float period = 0, float overshootOrAmplitude = 1)
        {
            // switch (easeType)
            // {
            //     case EaseType.Linear:
            //         return time / duration;
            //     case EaseType.InQuad:
            //         return (time /= duration) * time;
            //     case EaseType.OutQuad:
            //         return -(time /= duration) * (time - 2);
            //     case EaseType.InCubic:
            //         return (time /= duration) * time * time;
            //     case EaseType.OutCubic:
            //         return ((time = time / duration - 1) * time * time + 1);
            // }
            // return time / duration;

            switch (easeType)
            {
                case EaseType.Linear:
                    return time / duration;
                case EaseType.InSine:
                    return 0f - (float)Math.Cos(time / duration * ((float)Math.PI / 2f)) + 1f;
                case EaseType.OutSine:
                    return (float)Math.Sin(time / duration * ((float)Math.PI / 2f));
                case EaseType.InOutSine:
                    return -0.5f * ((float)Math.Cos((float)Math.PI * time / duration) - 1f);
                case EaseType.InQuad:
                    return (time /= duration) * time;
                case EaseType.OutQuad:
                    return (0f - (time /= duration)) * (time - 2f);
                case EaseType.InOutQuad:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * time * time;
                    }
                    return -0.5f * ((time -= 1f) * (time - 2f) - 1f);
                case EaseType.InCubic:
                    return (time /= duration) * time * time;
                case EaseType.OutCubic:
                    return (time = time / duration - 1f) * time * time + 1f;
                case EaseType.InOutCubic:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * time * time * time;
                    }
                    return 0.5f * ((time -= 2f) * time * time + 2f);
                case EaseType.InQuart:
                    return (time /= duration) * time * time * time;
                case EaseType.OutQuart:
                    return 0f - ((time = time / duration - 1f) * time * time * time - 1f);
                case EaseType.InOutQuart:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * time * time * time * time;
                    }
                    return -0.5f * ((time -= 2f) * time * time * time - 2f);
                case EaseType.InQuint:
                    return (time /= duration) * time * time * time * time;
                case EaseType.OutQuint:
                    return (time = time / duration - 1f) * time * time * time * time + 1f;
                case EaseType.InOutQuint:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * time * time * time * time * time;
                    }
                    return 0.5f * ((time -= 2f) * time * time * time * time + 2f);
                case EaseType.InExpo:
                    if (time != 0f)
                    {
                        return (float)Math.Pow(2.0, 10f * (time / duration - 1f));
                    }
                    return 0f;
                case EaseType.OutExpo:
                    if (time == duration)
                    {
                        return 1f;
                    }
                    return 0f - (float)Math.Pow(2.0, -10f * time / duration) + 1f;
                case EaseType.InOutExpo:
                    if (time == 0f)
                    {
                        return 0f;
                    }
                    if (time == duration)
                    {
                        return 1f;
                    }
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * (float)Math.Pow(2.0, 10f * (time - 1f));
                    }
                    return 0.5f * (0f - (float)Math.Pow(2.0, -10f * (time -= 1f)) + 2f);
                case EaseType.InCirc:
                    return 0f - ((float)Math.Sqrt(1f - (time /= duration) * time) - 1f);
                case EaseType.OutCirc:
                    return (float)Math.Sqrt(1f - (time = time / duration - 1f) * time);
                case EaseType.InOutCirc:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return -0.5f * ((float)Math.Sqrt(1f - time * time) - 1f);
                    }
                    return 0.5f * ((float)Math.Sqrt(1f - (time -= 2f) * time) + 1f);
                case EaseType.InElastic:
                    {
                        if (time == 0f)
                        {
                            return 0f;
                        }
                        if ((time /= duration) == 1f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.3f;
                        }
                        float num3;
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num3 = period / 4f;
                        }
                        else
                        {
                            num3 = period / ((float)Math.PI * 2f) * (float)Math.Asin(1f / overshootOrAmplitude);
                        }
                        return 0f - overshootOrAmplitude * (float)Math.Pow(2.0, 10f * (time -= 1f)) * (float)Math.Sin((time * duration - num3) * ((float)Math.PI * 2f) / period);
                    }
                case EaseType.OutElastic:
                    {
                        if (time == 0f)
                        {
                            return 0f;
                        }
                        if ((time /= duration) == 1f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.3f;
                        }
                        float num2;
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num2 = period / 4f;
                        }
                        else
                        {
                            num2 = period / ((float)Math.PI * 2f) * (float)Math.Asin(1f / overshootOrAmplitude);
                        }
                        return overshootOrAmplitude * (float)Math.Pow(2.0, -10f * time) * (float)Math.Sin((time * duration - num2) * ((float)Math.PI * 2f) / period) + 1f;
                    }
                case EaseType.InOutElastic:
                    {
                        if (time == 0f)
                        {
                            return 0f;
                        }
                        if ((time /= duration * 0.5f) == 2f)
                        {
                            return 1f;
                        }
                        if (period == 0f)
                        {
                            period = duration * 0.450000018f;
                        }
                        float num;
                        if (overshootOrAmplitude < 1f)
                        {
                            overshootOrAmplitude = 1f;
                            num = period / 4f;
                        }
                        else
                        {
                            num = period / ((float)Math.PI * 2f) * (float)Math.Asin(1f / overshootOrAmplitude);
                        }
                        if (time < 1f)
                        {
                            return -0.5f * (overshootOrAmplitude * (float)Math.Pow(2.0, 10f * (time -= 1f)) * (float)Math.Sin((time * duration - num) * ((float)Math.PI * 2f) / period));
                        }
                        return overshootOrAmplitude * (float)Math.Pow(2.0, -10f * (time -= 1f)) * (float)Math.Sin((time * duration - num) * ((float)Math.PI * 2f) / period) * 0.5f + 1f;
                    }
                case EaseType.InBack:
                    return (time /= duration) * time * ((overshootOrAmplitude + 1f) * time - overshootOrAmplitude);
                case EaseType.OutBack:
                    return (time = time / duration - 1f) * time * ((overshootOrAmplitude + 1f) * time + overshootOrAmplitude) + 1f;
                case EaseType.InOutBack:
                    if ((time /= duration * 0.5f) < 1f)
                    {
                        return 0.5f * (time * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time - overshootOrAmplitude));
                    }
                    return 0.5f * ((time -= 2f) * time * (((overshootOrAmplitude *= 1.525f) + 1f) * time + overshootOrAmplitude) + 2f);
                case EaseType.InBounce:
                    return Bounce.EaseIn(time, duration, overshootOrAmplitude, period);
                case EaseType.OutBounce:
                    return Bounce.EaseOut(time, duration, overshootOrAmplitude, period);
                case EaseType.InOutBounce:
                    return Bounce.EaseInOut(time, duration, overshootOrAmplitude, period);
                case EaseType.Flash:
                    return Flash.Ease(time, duration, overshootOrAmplitude, period);
                case EaseType.InFlash:
                    return Flash.EaseIn(time, duration, overshootOrAmplitude, period);
                case EaseType.OutFlash:
                    return Flash.EaseOut(time, duration, overshootOrAmplitude, period);
                case EaseType.InOutFlash:
                    return Flash.EaseInOut(time, duration, overshootOrAmplitude, period);
                default:
                    return (0f - (time /= duration)) * (time - 2f);
            }
        }


        public static Vector3 ComputeBezierPoint(Vector3[] pointArr, float runTime, float duration, EaseType ease = EaseType.Linear)
        {
            // 把时间从 [0,runTime] 映射到 [0,1] 之间
            float t = Evaluate(ease, runTime, duration);

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
            // float length = (float)(Math.Sqrt(Math.Pow(_prevPos.pos.x - x, 2) + Math.Pow(_prevPos.pos.y - y, 2) + Math.Pow(_prevPos.pos.z - z, 2)));
            // BVector3 v3 = new BVector3(new Vector3(x, y, z), length, 0);

            // 存储当前节点z
            // _pointLists.Add(v3);
            // _prevPos = v3;
            // 累计长度
            // _totalLength += length;
            return new Vector3(x, y, z);
        }

        public static IEnumerator Move(Transform ts, Vector3[] pointArr, float duration, EaseType ease = EaseType.Linear)
        {
            // this._runTime = duration;
            // this.pointArr = pointArr;
            // this.ease = ease;

            // ResetData();
            ts.position = pointArr[0];
            float runTime = 0;

            while (runTime < duration)
            {
                runTime += Time.deltaTime;
                if (runTime > duration) runTime = duration;

                ts.position = ComputeBezierPoint(pointArr, runTime, duration, ease);
                Debug.Log("当前时间：" + runTime);
                yield return 0;
            }
        }

        public static void MovePath(this Transform ts, Vector3[] pointArr, float duration, EaseType ease = EaseType.Linear)
        {
            CoroutineManager.Instance.StartCoroutine(Move(ts, pointArr, duration, ease));
            // StartCoroutine(MovePath())
        }

    }
}
