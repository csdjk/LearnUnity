using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Common {
    public class ResourceManager {
        private static Dictionary<string, string> configMap;

        static ResourceManager () {
            string fileContent = GetConfigFile ("ConfigMap.txt");
            BuildMap (fileContent);
        }
        public static T Load<T> (string prefabeName) where T : Object {
            string prefabePath = configMap[prefabeName];
            return Resources.Load<T> (prefabePath);
        }

        public static string GetConfigFile (string fileName) {
            string url;

#if UNITY_EDITOR || UNITY_STANDALONE
            url = $"file://{Application.dataPath}/StreamingAssets/{fileName}";
#elif UNITY_IPHONE
            url = $"file://{Application.dataPath}/Raw/{fileName}";
#elif UNITY_ANDROID
            url = $"jar:file://{Application.dataPath}!/assets/{fileName}";
#endif

            UnityWebRequest www = UnityWebRequest.Get (url);
            www.SendWebRequest ();
            while (true) {
                if (www.isDone) {
                    return www.downloadHandler.text;
                }
                if (www.isNetworkError) {
                    Debug.LogError(www.error);
                    return null;
                }
            }
        }

        public static void BuildMap (string fileContent) {
            configMap = new Dictionary<string, string> ();
            // 字符串读取器
            using (StringReader reader = new StringReader (fileContent)) {
                // 逐行读取
                string line;
                while ((line = reader.ReadLine ()) != null) {
                    string[] keyValue = line.Split ('=');
                    configMap.Add (keyValue[0], keyValue[1]);
                }
            }
        }
    }

}