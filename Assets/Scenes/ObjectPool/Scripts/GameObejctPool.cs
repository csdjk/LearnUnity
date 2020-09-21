using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Common {

    public interface IResetable {
        void OnReset ();
    }

    /// <summary>
    /// 对象池 - 单例
    /// </summary>
    public class GameObejctPool : MonoSingleton<GameObejctPool> {
        // 池缓存
        private Dictionary<string, List<GameObject>> cache = new Dictionary<string, List<GameObject>>();

        /// <summary>
        /// 创建对象, 并放入对象池
        /// </summary>
        /// <param name="key"></param>
        /// <param name="prefab"></param>
        /// <param name="pos"></param>
        /// <param name="rotate"></param>
        /// <returns></returns>
        public GameObject CreateObject (string key, GameObject prefab, Vector3 pos, Quaternion rotate) {

            GameObject go = FindUseableObject (key);

            if (go == null) {
                go = AddObject (key, prefab);
            }

            UseObject (go, pos, rotate);
            return go;

        }

        // 使用
        void UseObject (GameObject go, Vector3 pos, Quaternion rotate) {
            go.transform.position = pos;
            go.transform.rotation = rotate;
            go.SetActive (true);

            // 对于 实现了 重置接口的 对象 重置一下
            IResetable reset = go.GetComponent<IResetable> ();
            if (reset != null)
                reset.OnReset ();
        }

        // 添加对象
        GameObject AddObject (string key, GameObject prefab) {
            GameObject go = Instantiate (prefab);

            if (!cache.ContainsKey (key))
                cache.Add (key, new List<GameObject> ());
            // 加入池中
            cache[key].Add (go);
            return go;
        }

        //查找可以使用的对象
        GameObject FindUseableObject (string key) {
            if (cache.ContainsKey (key))
                return cache[key].Find (g => !g.activeInHierarchy);
            return null;
        }

        // 回收对象
        public void CollectObejct (GameObject go, float delay = 0) {
            StartCoroutine (CollectObejctDelay (go, delay));
        }

        // 延迟回收
        private IEnumerator CollectObejctDelay (GameObject go, float delay) {
            yield return new WaitForSeconds (delay);
            Debug.Log($"回收 对象{go.name} 等待时间{delay}");
            go.SetActive (false);
        }

        /// <summary>
        /// 清空对象池
        /// </summary>
        /// <param name="key"></param>
        public void Clear (string key) {

            foreach (var item in cache[key]) {
                Destroy (item);
            }
            cache.Remove (key);
        }

        /// <summary>
        /// 清空所有
        /// </summary>
        public void ClearAll () {
            List<string> list = new List<string> (cache.Keys);
            foreach (var item in list) {
                Clear (item);
            }
        }
    }
}