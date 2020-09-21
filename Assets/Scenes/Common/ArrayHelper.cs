using System;
using System.Collections.Generic;

namespace Common {
    /// <summary>
    /// 数组扩展
    /// </summary>
    public static class ArrayExtension {

        /// <summary>
        /// 查找所有满足条件的元素
        /// </summary>
        /// <param name="array"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T[] FindAll<T>(this T[] array,Func<T,bool> callback){
            List<T> list = new List<T>();
            for (int i = 0; i < array.Length; i++)
            {
                if (callback(array[i]))
                {
                    list.Add(array[i]);
                }
            }
            return list.ToArray();
        }

        /// <summary>
        /// 选择元素
        /// </summary>
        /// <param name="array"></param>
        /// <param name="callback"></param>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="Q"></typeparam>
        /// <returns></returns>
        public static Q[] Select<T, Q> (this T[] array, Func<T, Q> callback) {
            List<Q> result = new List<Q> ();
            for (int i = 0; i < array.Length; i++) {
                result.Add (callback (array[i]));
            }
            return result.ToArray ();
        }

        /// <summary>
        /// 获取最小值
        /// </summary>
        /// <param name="array">数组</param>
        /// <param name="callback">处理方法</param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetMin<T> (this T[] array, Func<T, float> callback) {
            int minIndex = 0;
            float min = callback (array[minIndex]);

            for (int i = 1; i < array.Length; i++) {
                float value = callback (array[i]);
                if (value < min) {
                    min = value;
                    minIndex = i;
                }
            }
            return array[minIndex];
        }
    }
}