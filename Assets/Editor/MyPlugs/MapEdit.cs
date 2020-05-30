using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using Newtonsoft.Json;

// [CustomEditor(typeof(MapObj))]
public class MapEdit : EditorWindow
{
    public static MapEdit editor;
    // 存储地图上的所有物体
    private List<Dictionary<string, object>> objs = new List<Dictionary<string, object>>();
    private string mapJsonPath = "Assets/Scenes/Map/Json";
    private string mapTempsPath = "Assets/Scenes/Map/MapTemps";
    private string mapsPath = "Assets/Scenes/Map/MapPrefabs";
    private string objsPath = "Assets/Scenes/Map/ObjPrefabs";
    private string[] mapTempLists;
    private string[] mapLists;
    private string[] objsLists;
    private int selectMapTempIndex = 0;
    private int selectMapIndex = 0;
    private int selectObjIndex = 0;

    private GameObject curMap;
    private string curMapName;
    private int curMapID;
    private string newMapName = "map";
    private int newMapID = 1000;
    private int curObjID = 10000;
    private Color curObjColor = Color.white;
    private Vector3 curObjPos;


    [MenuItem("长生但酒狂的插件/地图编辑")]
    public static void showWindow()
    {
        editor = EditorWindow.GetWindow<MapEdit>();
        editor.Show();
        EditorApplication.hierarchyChanged += OnHierarchyChanged;
        SceneView.duringSceneGui += OnScene;
    }

    private void Awake()
    {
        mapTempLists = FindAllObjFromFiles(mapTempsPath);
        mapLists = FindAllObjFromFiles(mapsPath);
        if (mapLists.Length == 0)
        {
            Debug.Log("没有找到地图, 请新创建一个地图");
            return;
        }
        Debug.Log(mapsPath + "/" + mapLists[0]);
        // 实例化
        ReloadMap(0);

    }

    public void OnGUI()
    {
        GUIStyle titleSkin = new GUIStyle();
        titleSkin.normal.textColor = Color.yellow;

        GUIStyle btnSkin = GUI.skin.GetStyle("flow node 4");

        EditorGUILayout.Space(20);
        GUILayout.Label("  新地图:", titleSkin);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(30);
            GUILayout.Label("地图模板:");
            selectMapTempIndex = EditorGUILayout.Popup(selectMapTempIndex, mapTempLists);
            GUILayout.Space(50);

            GUILayout.Label("name:");
            newMapName = EditorGUILayout.TextField(newMapName);
            GUILayout.Space(50);

            GUILayout.Label("ID:");
            newMapID = EditorGUILayout.IntField(newMapID);
            GUILayout.Space(50);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space(10);

        if (GUILayout.Button("创建新地图"))
        {
            CreateNewMap();
        }
        EditorGUILayout.Space(20);

        GUILayout.Label("  编辑地图:", titleSkin);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(30);
            GUILayout.Label("选择地图:");
            // mapLists = FindAllObjFromFiles(mapsPath);
            int preMapIndex = this.selectMapIndex;
            selectMapIndex = EditorGUILayout.Popup(this.selectMapIndex, mapLists);
            if (preMapIndex != selectMapIndex) ReloadMap(selectMapIndex);
            GUILayout.Space(50);
            // 
            GUILayout.Label("name:");
            curMapName = EditorGUILayout.TextField(curMapName);
            GUILayout.Space(50);
            GUILayout.Label("ID:");
            curMapID = EditorGUILayout.IntField(curMapID);

            GUILayout.Space(30);
        }
        EditorGUILayout.EndHorizontal();
        EditorGUILayout.Space();
        if (GUILayout.Button("删除地图:"))
        {
            DeleteMap();
        }
        EditorGUILayout.Space(20);

        GUILayout.Label("  创建物体:", titleSkin);
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(30);
            GUILayout.Label("物体:");
            objsLists = FindAllObjFromFiles(objsPath);
            selectObjIndex = EditorGUILayout.Popup(this.selectObjIndex, objsLists);
            GUILayout.Space(50);

            GUILayout.Label("输入ID");
            this.curObjID = EditorGUILayout.IntField(this.curObjID);
            GUILayout.Space(50);

            GUILayout.Label("标签颜色");
            this.curObjColor = EditorGUILayout.ColorField(this.curObjColor);
            GUILayout.Space(30);
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(30);
        this.curObjPos = EditorGUILayout.Vector3Field("坐标: ", this.curObjPos);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space(10);

        if (GUILayout.Button("创建物体"))
        {
            CreateGameObj();
        }

        EditorGUILayout.Space(50);
        // 水平居中
        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("保存当前地图", btnSkin, GUILayout.Width(150), GUILayout.Height(30)))
            {
                SaveMap();
            }
            GUILayout.FlexibleSpace();
        }

        GUILayout.EndHorizontal();
    }


    void CreateNewMap()
    {
        if (MapIsExistByName(newMapName) && !EditorUtility.DisplayDialog("警告!", newMapName + "已存在, 确定覆盖?", "ok", "cancel"))
            return;
        if (MapIsExistByID(newMapID))
        {
            EditorUtility.DisplayDialog("创建失败!", newMapID + " ID 已存在,请重新输入唯一ID", "ok", "cancel");
            return;
        }

        if (curMap) DestroyImmediate(curMap);
        // 实例化
        curMap = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(mapTempsPath + "/" + mapTempLists[selectMapTempIndex] + ".prefab"));
        curMap.transform.position = Vector3.zero;
        curMap.name = newMapName;
        MapObj script = curMap.AddComponent<MapObj>();
        script.id = newMapID;
        // 保存
        PrefabUtility.SaveAsPrefabAsset(curMap, mapsPath + "/" + curMap.name + ".prefab");
        CreateMapJson();
        AssetDatabase.Refresh();
        UpdateSelectMapUI();
    }

    bool MapIsExistByName(string name)
    {
        foreach (var map in mapLists)
        {
            if (map == name)
                return true;
        }
        return false;
    }

    bool MapIsExistByID(int id)
    {
        foreach (var map in mapLists)
        {
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(mapsPath + "/" + map + ".prefab");
            if (go.GetComponent<MapObj>().id == id)
                return true;
        }
        return false;
    }

    void UpdateSelectMapUI()
    {
        curMapName = newMapName;
        curMapID = newMapID;
        GUI.FocusControl(null);
        mapLists = FindAllObjFromFiles(mapsPath);
        for (int i = 0; i < mapLists.Length; i++)
        {
            if (curMap.name == mapLists[i])
            {
                selectMapIndex = i;
                return;
            }
        }
    }

    void ReloadMap(int index)
    {
        if (curMap)
            SaveMap();
        if (curMap) DestroyImmediate(curMap);
        // 实例化
        GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(mapsPath + "/" + mapLists[index] + ".prefab");
        if (go == null)
        {
            Debug.LogError("加载失败! " + mapLists[index] + "文件未找到");
            mapLists = FindAllObjFromFiles(mapsPath);
            return;
        }
        curMap = Instantiate(go);
        curMap.transform.position = Vector3.zero;
        curMap.name = mapLists[index];
        curMapID = curMap.GetComponent<MapObj>().id;
        curMapName = curMap.name;
    }

    // 创建物体
    void CreateGameObj()
    {
        if (curMap == null)
        {
            EditorUtility.DisplayDialog("创建失败!", "未找到地图, 请先创建一个新地图或者选择一个可编辑的地图", "ok", "cancel");
            return;
        }
        MapObj[] children = curMap.transform.GetComponentsInChildren<MapObj>();
        foreach (var child in children)
        {
            if (child.id == curObjID)
            {
                EditorUtility.DisplayDialog("创建失败!", "当前ID已存在,请重新输入唯一ID", "ok", "cancel");
                return;
            }
        }

        GameObject go = Instantiate(AssetDatabase.LoadAssetAtPath<GameObject>(objsPath + "/" + objsLists[selectObjIndex] + ".prefab"));
        go.transform.parent = curMap.transform;
        go.transform.position = curObjPos;
        go.name = objsLists[selectObjIndex];
        MapObj objScript = go.GetComponent<MapObj>();
        objScript = objScript ? objScript : go.AddComponent<MapObj>();
        objScript.id = curObjID;
        objScript.nameColor = curObjColor;
        curObjID++;
        // 取消聚焦
        GUI.FocusControl(null);
    }

    // 查找所有文件夹下所有物体
    string[] FindAllObjFromFiles(string path)
    {
        string[] allPath = AssetDatabase.FindAssets("t:GameObject", new string[] { path });
        string[] prefabsNameList = new string[allPath.Length];
        for (int i = 0, len = allPath.Length; i < len; i++)
        {
            string objPath = AssetDatabase.GUIDToAssetPath(allPath[i]);
            prefabsNameList[i] = Path.GetFileNameWithoutExtension(objPath);
        }
        return prefabsNameList;
    }

    public void CreateMapJson()
    {
        objs.Clear();
        foreach (Transform child in curMap.transform)
        {
            Debug.Log(child.name);
            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("id", child.GetComponent<MapObj>().id);
            obj.Add("x", child.position.x);
            obj.Add("y", child.position.y);
            obj.Add("z", child.position.z);
            objs.Add(obj);
        }
        string objstring = JsonConvert.SerializeObject(objs);

        string mapStr = "{\"mapid\":" + curMap.GetComponent<MapObj>().id + ",\"res\":\"" + curMap.name + "\",\"objs\":" + objstring + "}";

        FileStream fileStream = new FileStream(Path.Combine(mapJsonPath, curMap.name + ".json"), FileMode.Create);
        StreamWriter streamWriter = new StreamWriter(fileStream);
        streamWriter.Write(mapStr);
        streamWriter.Flush();
        streamWriter.Close();
        fileStream.Close();
    }

    public void SaveMap()
    {
        if (curMap == null)
        {
            EditorUtility.DisplayDialog("保存失败!", "未找到地图, 请先创建一个新地图或者选择一个可编辑的地图", "ok", "cancel");
            return;
        }
        // 检查名字
        if (curMapName != curMap.name)
        {
            if (MapIsExistByName(curMapName) && !EditorUtility.DisplayDialog("保存失败!", curMapName + "已存在, 请重新输入名字?", "ok", "cancel"))
                return;
        }
        // 检查ID
        if (curMapID != curMap.GetComponent<MapObj>().id)
        {
            if (MapIsExistByID(curMapID))
            {
                EditorUtility.DisplayDialog("保存失败!", "保存失败! " + curMapID + " ID 已存在,请重新输入唯一ID", "ok", "cancel");
                return;
            }
        }
        // 删除之前文件
        DeleteMapFile(curMap.name);
        // 保存
        curMap.name = curMapName;
        curMap.GetComponent<MapObj>().id = curMapID;
        PrefabUtility.SaveAsPrefabAsset(curMap, mapsPath + "/" + curMap.name + ".prefab");
        CreateMapJson();
        AssetDatabase.Refresh();
        mapLists = FindAllObjFromFiles(mapsPath);
        Debug.Log("保存成功!");
    }

    public void DeleteMapFile(string name)
    {
        string _mapPath = Path.Combine(mapsPath, name + ".prefab");
        string _mapPath_meta = Path.Combine(mapsPath, name + ".prefab.meta");
        File.Delete(_mapPath);
        File.Delete(_mapPath_meta);
        // 删除json文件
        string _mapJsonPath = Path.Combine(mapJsonPath, name + ".json");
        string _mapJsonPath_meta = Path.Combine(mapJsonPath, name + ".json.meta");
        File.Delete(_mapJsonPath);
        File.Delete(_mapJsonPath_meta);
        // 刷新资源
        AssetDatabase.Refresh();
        mapLists = FindAllObjFromFiles(mapsPath);
    }

    public void DeleteMap()
    {
        if (curMap)
        {
            DeleteMapFile(curMap.name);
            // 删除场景对象
            DestroyImmediate(curMap);
            curMap = null;
            selectMapIndex = -1;
            Debug.Log("删除成功!");
        }
    }


    // ->销毁窗口
    public void OnDestroy()
    {
        if (curMap == null)
            return;
        if (EditorUtility.DisplayDialog("提示", "是否保存当前编辑的地图", "Save", "Don't Save"))
            SaveMap();
        SceneView.duringSceneGui -= OnScene;
        EditorApplication.hierarchyChanged -= OnHierarchyChanged;
        if (curMap) DestroyImmediate(curMap);
    }

    public static Vector2 mPos;
    public static Vector2 centerBoxSize = new Vector2(200, 80);
    public static bool isDrawObjInfo = true;
    private static void OnScene(SceneView sceneview)
    {

        if (MapEdit.editor && MapEdit.editor.curMap)
        {
            Vector2 centerBoxPos = new Vector2(Screen.width / 2 - centerBoxSize.x / 2, 10);

            GUIStyle boxSink = new GUIStyle("box");
            boxSink.normal.textColor = Color.yellow;

            GUIStyle btnSink = new GUIStyle("sv_label_3");

            Handles.BeginGUI();
            // 地图信息(左上角)
            GUI.Box(new Rect(0, 0, 200, 220), "地图信息", boxSink);
            GUI.Box(new Rect(0, 0, 200, 220), "地图信息", boxSink);
            GUILayout.Label("当前编辑的地图 : " + MapEdit.editor.curMap.name);
            GUILayout.Label("ID:  " + editor.curMap.GetComponent<MapObj>().id);
            GUILayout.Label("个子物体: " + editor.curMap.transform.childCount);

            mPos = GUILayout.BeginScrollView(mPos, GUILayout.Width(200), GUILayout.Height(130));
            foreach (Transform child in editor.curMap.transform)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(child.GetComponent<MapObj>().id + ": " + child.name);
                if (GUILayout.Button("删除", btnSink))
                {
                    DestroyImmediate(child.gameObject);
                }

                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();
            // 当前选中物体信息UI
            if (Selection.activeTransform)
            {
                GameObject selectObj = Selection.activeTransform.gameObject;
                MapObj selectScript = selectObj.GetComponent<MapObj>();
                // 当前选中物体
                GUILayout.BeginArea(new Rect(centerBoxPos.x, centerBoxPos.y, centerBoxSize.x, centerBoxSize.y), "当前选中物体", boxSink);
                GUILayout.Space(30);
                EditorGUILayout.BeginHorizontal();
                GUILayout.FlexibleSpace();
                GUILayout.Label("Name: " + selectObj.name);
                GUILayout.Label("ID: " + selectScript.id);
                GUILayout.FlexibleSpace();
                EditorGUILayout.EndHorizontal();
                GUILayout.Space(10);
                if (GUILayout.Button("删除当前选中物体", btnSink))
                {
                    // 地图
                    if (selectScript.id < 10000)
                    {
                        if (EditorUtility.DisplayDialog("警告", "当前选中的是地图! 是否确认删除?", "Ok", "Cancel"))
                            editor.DeleteMap();
                    }
                    else
                        DestroyImmediate(selectObj);
                }
                GUILayout.EndArea();
            }

            // 是否开启GameObject info UI
            Vector2 togglePos = new Vector2(Screen.width - 300, 0);
            GUI.Box(new Rect(togglePos.x, togglePos.y, 160, 100), "功能面板", boxSink);
            isDrawObjInfo = GUI.Toggle(new Rect(togglePos.x + 20, togglePos.y + 10, 120, 50), isDrawObjInfo, "开启物体信息面板");

            Handles.EndGUI();
        }
    }

    //当层级视图中的对象或对象组发生更改时引发的事件
    private static void OnHierarchyChanged()
    {
        if (Selection.activeGameObject != null && PrefabUtility.GetPrefabType(Selection.activeGameObject) == PrefabType.PrefabInstance)
        {
            if (Selection.activeGameObject)
                DestroyImmediate(Selection.activeGameObject);
            Debug.LogError("非法操作! 禁止拖拽物体到场景! 请通过编辑器添加物体!");
        }
    }
}
