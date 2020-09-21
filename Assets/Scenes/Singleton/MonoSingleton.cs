using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单例模式
/// 使用方式: 直接继承该类即可
/// </summary>
/// <typeparam name="T"></typeparam>
public class MonoSingleton<T> : MonoBehaviour where T : MonoSingleton<T> {
    private static T instance;

    public static T Instance {
        get {
            if (instance == null) {
                // 在场景中根据类型查找引用
                instance = FindObjectOfType<T> ();

                if (instance == null) {
                    // 创建脚本对象(立即执行Awake)
                    new GameObject ("Singleton of " + typeof (T)).AddComponent<T> ();
                } else {
                    instance.Init ();
                }

            }
            return instance;
        }
    }

    // 为了防止异常
    public void Awake () {
        if (instance == null) {
            instance = this as T;
            Init ();
        }
    }
    public virtual void Init () {}
}