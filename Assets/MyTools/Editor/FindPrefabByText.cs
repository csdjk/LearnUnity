/*** 
 * @Descripttion: 根据内容查找所有预制体
 * @Author: lichanglong
 * @Date: 2020-12-15 12:01:04
 * @FilePath: \LearnUnity\Assets\MyTools\Editor\FindPrefabByText.cs
 */
using System.Text;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;
using System.Text.RegularExpressions;
using System.ComponentModel;
using System.Reflection;
using UnityEngine.UI;

namespace MyTools
{
    public enum SearchType
    {
        [Description("精准查询")]
        Accurate,
        [Description("模糊查询")]
        Fuzzy,
        [Description("正则查询")]
        Regex,
    }
    public class FindPrefabByText : EditorWindow
    {
        public static Transform UIRoot;

        private static string inputText;
        private static SearchType searchType = SearchType.Accurate;
        List<Object> destDirList = new List<Object>();
        Object destDir = null;
        List<string> prefabList = new List<string>();
        private int selectIndex = 0;
        private GameObject selectPrefab;
        public static string GUIPath = "Assets/Resources/GUI";
        public static string LogHead = "预制体路径：";
        public static string LogHead2 = "  节点name：";


        [MenuItem("MyTools/根据内容查找所有预制体")]
        private static void GetText()
        {
            EditorWindow window = GetWindow<FindPrefabByText>();
        }

        void OnEnable()
        {
            var currentScene = UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene();
            GameObject[] gos = currentScene.GetRootGameObjects();
            foreach (var go in gos)
            {
                if (go.name == "UIRoot")
                {
                    UIRoot = go.transform;
                    break;
                }
            }
        }

        void OnGUI()
        {
            EditorGUILayoutTools.Horizontal(() =>
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("快速定位GUI文件夹", GUILayout.Width(150), GUILayout.Height(30)))
                {
                    var assetObj = AssetDatabase.LoadAssetAtPath<Object>(GUIPath);
                    EditorGUIUtility.PingObject(assetObj);
                }
                GUILayout.FlexibleSpace();
            });

            GUILayout.Space(15);

            EditorGUILayoutTools.Horizontal(() =>
            {
                inputText = EditorGUILayout.TextField("输入需要查找的Text", inputText);
            });

            EditorGUILayoutTools.Horizontal(() =>
            {
                searchType = (SearchType)EditorGUILayout.EnumPopup("匹配模式", searchType, "ToolbarPopup");
            });

            for (int i = 0; i < destDirList.Count; i++)
            {
                EditorGUILayoutTools.Horizontal(() =>
                {
                    destDirList[i] = EditorGUILayout.ObjectField("文件夹" + (i + 1), destDirList[i], typeof(Object), false);
                    if (GUILayout.Button("Delete", GUILayout.Width(50), GUILayout.Height(15)))
                    {
                        destDirList.RemoveAt(i);
                    }
                });
            }

            EditorGUILayoutTools.DrawObjectField("添加文件夹", ref destDir, typeof(Object), (v) =>
            {
                if (destDirList.Contains(destDir))
                {
                    EditorUtility.DisplayDialog("添加失败!", "已经添加该文件夹", "OK");
                    destDir = null;
                }
                else
                {

                    destDirList.Add(destDir);
                    destDir = null;
                }
            });

            if (prefabList.Count > 0)
            {
                EditorGUILayoutTools.DrawPopup("预制体列表", ref selectIndex, prefabList.ToArray(), (path) =>
                {
                    selectPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(RecoverPrefabPath(path));
                    EditorGUIUtility.PingObject(selectPrefab);
                });
            }

            GUILayout.Space(30);
            EditorGUILayoutTools.Horizontal(() =>
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("查找预制体", GUILayout.Width(300), GUILayout.Height(40)))
                {
                    if (inputText == null || inputText.Equals(""))
                    {
                        EditorUtility.DisplayDialog("查找失败!", "请输入需要查找的字符！", "OK");
                        return;
                    }
                    var stime = EditorApplication.timeSinceStartup;
                    StartSearch();
                    Debug.Log("查询时间：" + (EditorApplication.timeSinceStartup - stime).ToString());
                }
                GUILayout.FlexibleSpace();
            });
            GUILayout.Space(30);
        }

        void StartSearch()
        {
            prefabList.Clear();
            // pathList.Clear();
            ClearConsole();
            // 遍历文件夹目录
            foreach (var item in destDirList)
            {
                string path = AssetDatabase.GetAssetPath(item);
                // 如果选中的是单个文件
                if (File.Exists(path))
                {
                    SearchText(path);
                    return;
                }
                // 获取当前文件夹下所有文件（包括子文件）
                List<FileInfo> prefabs = new List<FileInfo>();
                FileSystem.GetAllFileByPath(path, ref prefabs, "*.prefab");

                // 遍历该文件夹下所有预制体
                foreach (var prefab in prefabs)
                {
                    var arr = Regex.Split(prefab.FullName, "Assets");

                    if (arr != null && arr.Length > 1)
                    {
                        arr[1] = arr[1].Replace('\\', '/');
                        SearchText("Assets" + arr[1]);
                    }
                }
            }

            // PrintAllPath();
        }

        void SearchText(string prefabPath)
        {
            // var go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            // pathStrBuilder = new StringBuilder();
            // 精准查询
            if (searchType == SearchType.Accurate)
            {
                SearchText(prefabPath, (prefabLab) =>
                {
                    return prefabLab.Equals(inputText);
                });
            }
            // 模糊查询
            else if (searchType == SearchType.Fuzzy)
            {
                SearchText(prefabPath, (prefabLab) =>
                {
                    inputText = inputText.ToLower();
                    prefabLab = prefabLab.ToLower();
                    return prefabLab.Contains(inputText);
                });

            }
            // 正则查询
            else if (searchType == SearchType.Regex)
            {
                SearchText(prefabPath, (prefabLab) =>
                {
                    return Regex.IsMatch(prefabLab, inputText);
                });
                //     DeepSearchText(go.transform, prefabPath, (prefabLab) =>
                //   {
                //       return Regex.IsMatch(prefabLab, inputText);
                //   });
            }
        }
        void SearchText(string prefabPath, Func<string, bool> func)
        {
            var go = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (go)
            {
                Text[] labs = go.GetComponentsInChildren<Text>(true);
                // 遍历预制体下所有文字
                foreach (Text lab in labs)
                {
                    if (func(lab.text))
                    {
                        // 打印 路径
                        LogPath(prefabPath, lab.transform.GetPath());
                        // pathList.Add(lab.transform);
                        prefabList.Add(SplitPrefabPath(prefabPath));
                    }
                }
            }
        }

        // private List<Transform> pathList = new List<Transform>();
        // void PrintAllPath()
        // {
        //     int len = pathList.Count;
        //     for (var i = 0; i < len; i++)
        //     {
        //         LogPath(prefabList[i], pathList[i].GetPath());
        //     }
        // }

        // // 深度搜索
        // StringBuilder pathStrBuilder = new StringBuilder();
        // Transform child;
        // UILabel lab;
        // void DeepSearchText(Transform trs, string prefabPath, Func<string, bool> func)
        // {
        //     if (trs)
        //     {
        //         pathStrBuilder.Append(trs.name + "/");
        //         int len = trs.childCount;
        //         for (var i = 0; i < len; i++)
        //         {
        //             child = trs.GetChild(i);
        //             lab = child.GetComponent<UILabel>();
        //             if (lab && func(lab.text))
        //             {
        //                 LogPath(prefabPath, pathStrBuilder.ToString());
        //                 prefabList.Add(SplitPrefabPath(prefabPath));
        //             }
        //             DeepSearchText(child, prefabPath, func);
        //         }
        //         pathStrBuilder.Remove(pathStrBuilder.Length - 1, 1);
        //     }
        // }

        // 打印 路径
        StringBuilder stringBuilder = new StringBuilder();
        void LogPath(string prefabPath, string nodeName)
        {
            stringBuilder.Remove(0, stringBuilder.Length);
            // stringBuilder = new StringBuilder();
            stringBuilder.Append(LogHead);
            stringBuilder.Append("<color=yellow>");
            stringBuilder.Append(prefabPath);
            stringBuilder.Append("</color>");

            stringBuilder.Append(LogHead2);
            stringBuilder.Append("<color=yellow>");
            stringBuilder.Append(nodeName);
            stringBuilder.Append("</color>");

            Debug.Log(stringBuilder.ToString());
            // Debug.Log(string.Format(LogHead + "<color=yellow>{0}</color>" + LogHead2 + "<color=yellow>{1}</color>", prefabPath, nodeName));
        }

        string SplitPrefabPath(string path)
        {
            return path.Replace(GUIPath + "/", String.Empty);
        }
        string RecoverPrefabPath(string path)
        {
            return GUIPath + "/" + path;
        }

        static MethodInfo clearMethod = null;
        private static void ClearConsole()
        {
            if (clearMethod == null)
            {
                Type log = typeof(EditorWindow).Assembly.GetType("UnityEditor.LogEntries");
                clearMethod = log.GetMethod("Clear");
            }
            clearMethod.Invoke(null, null);
        }


       
    }
}

