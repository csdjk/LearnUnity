using System;
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

    private Editor mapTempEditor;
    private Editor mapEditor;
    private Editor objEditor;

    private GameObject curMap;
    private string curMapName;
    private int curMapID;
    private string newMapName = "map";
    private int newMapID = 1000;
    private int curObjID = 10000;
    private Color curObjColor = Color.white;
    private Vector3 curObjPos;


    [MenuItem("长生但酒狂的插件/地图编辑器")]
    public static void showWindow()
    {
        editor = EditorWindow.GetWindow<MapEdit>();
        editor.Show();
    }

    private void Awake()
    {
        SceneView.duringSceneGui += OnScene;
        EditorApplication.hierarchyChanged += OnHierarchyChanged;

        mapTempLists = FindAllObjFromFiles(mapTempsPath);
        mapLists = FindAllObjFromFiles(mapsPath);
        objsLists = FindAllObjFromFiles(objsPath);
        if (mapLists.Length == 0)
        {
            Debug.Log("没有找到地图, 请新创建一个地图");
            return;
        }
        Debug.Log(mapsPath + "/" + mapLists[0]);
        // 实例化
        ReloadMap(0);
        mapTempEditor = CreateGameObjectPreview("mapTemp");
        mapEditor = CreateGameObjectPreview("map");
        objEditor = CreateGameObjectPreview("obj");
    }

    Editor CreateGameObjectPreview(string type)
    {
        string path = "";
        switch (type)
        {
            case "mapTemp":
                path = Path.Combine(mapTempsPath, mapTempLists[selectMapTempIndex] + ".prefab");
                break;
            case "map":
                path = Path.Combine(mapsPath, mapLists[selectMapIndex] + ".prefab");
                break;
            case "obj":
                path = Path.Combine(objsPath, objsLists[selectObjIndex] + ".prefab");
                break;
        }
        GameObject gameObject = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        return Editor.CreateEditor(gameObject);
    }

    // 【新地图】
    void DrawNewMapGUI(GUIStyle titleSkin)
    {
        CreateCenterHorizontalLayout(() =>
        {
            GUILayout.Label("  新地图:", titleSkin);
        });

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(30);
            GUILayout.Label("地图模板:");
            int preMapTempIndex = selectMapTempIndex;
            selectMapTempIndex = EditorGUILayout.Popup(selectMapTempIndex, mapTempLists);
            if (preMapTempIndex != selectMapTempIndex)
                mapTempEditor = CreateGameObjectPreview("mapTemp");
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
        // 预览图
        if (mapTempEditor)
            mapTempEditor.OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), EditorStyles.whiteLabel);

        EditorGUILayout.Space(10);

        if (GUILayout.Button("创建新地图"))
        {
            CreateNewMap();
        }
    }

    void DrawEditMapGUI(GUIStyle titleSkin)
    {
        CreateCenterHorizontalLayout(() =>
        {
            GUILayout.Label("  编辑地图:", titleSkin);
        });
        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(30);
            GUILayout.Label("选择地图:");
            int preMapIndex = this.selectMapIndex;
            selectMapIndex = EditorGUILayout.Popup(this.selectMapIndex, mapLists);
            if (preMapIndex != selectMapIndex)
            {
                ReloadMap(selectMapIndex);
                mapEditor = CreateGameObjectPreview("map");
            }
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

        EditorGUILayout.Space(10);

        // 预览图
        if (mapEditor)
            mapEditor.OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), EditorStyles.whiteLabel);

        EditorGUILayout.Space(10);
        if (GUILayout.Button("删除地图:"))
        {
            DeleteMap();
        }
    }

    void DrawNewObjectGUI(GUIStyle titleSkin)
    {
        CreateCenterHorizontalLayout(() =>
       {
           GUILayout.Label("  创建物体:", titleSkin);
       });

        GUILayout.Space(10);

        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.Space(30);
            GUILayout.Label("物体:");
            objsLists = FindAllObjFromFiles(objsPath);
            int preObjIndex = selectObjIndex;
            selectObjIndex = EditorGUILayout.Popup(selectObjIndex, objsLists);
            if (preObjIndex != selectObjIndex)
                objEditor = CreateGameObjectPreview("obj");
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

        GUILayout.Space(10);

        // 预览图
        if (objEditor)
            objEditor.OnPreviewGUI(GUILayoutUtility.GetRect(100, 100), EditorStyles.whiteLabel);


        EditorGUILayout.Space(10);

        if (GUILayout.Button("创建物体 (或者鼠标点击地图指定位置快速创建)"))
        {
            CreateGameObj(curObjPos, Vector3.up);
        }
    }
    public void OnGUI()
    {
        GUIStyle titleSkin = new GUIStyle();
        titleSkin.normal.textColor = Color.yellow;
        titleSkin.fontSize = 20;
        GUIStyle btnSkin = GUI.skin.GetStyle("flow node 4");
        // ---------------------------【新地图】---------------------------
        EditorGUILayout.Space(20);
        DrawNewMapGUI(titleSkin);
        // ---------------------------【编辑地图】---------------------------
        EditorGUILayout.Space(20);
        DrawEditMapGUI(titleSkin);
        // ---------------------------【创建物体】---------------------------
        EditorGUILayout.Space(20);
        DrawNewObjectGUI(titleSkin);
        // ---------------------------【保存当前地图按钮】---------------------------
        EditorGUILayout.Space(50);
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

        curObjID = curMap.transform.GetChild(curMap.transform.childCount - 1).GetComponent<MapObj>().id + 1;
    }

    // 创建物体
    void CreateGameObj(Vector3 pos, Vector3 up)
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
        go.transform.position = pos;
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
            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("id", child.GetComponent<MapObj>().id);
            obj.Add("x", Round(child.position.x));
            obj.Add("y", Round(child.position.y));
            obj.Add("z", Round(child.position.z));
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
    public static bool isFastCreateObj = true;
    private static void OnScene(SceneView sceneview)
    {

        if (MapEdit.editor && MapEdit.editor.curMap)
        {
            Event e = Event.current;
            GUIStyle boxSink = new GUIStyle("box");
            boxSink.normal.textColor = Color.yellow;
            GUIStyle btnSink = new GUIStyle("sv_label_3");
            Handles.BeginGUI();
            DrawMapInfoBox(boxSink, btnSink);
            DrawSelectObjInfoBox(boxSink, btnSink);
            DrawFunctionInfoBox(boxSink);
            // 点击场景快速创建
            if (Event.current.type == EventType.MouseDown && Event.current.button == 0 && isFastCreateObj)
            {
                editor.CreateGameObjectMouse();
            }
            Handles.EndGUI();

            // 禁止移动map
            if (!editor.curMap.transform.position.Equals(Vector3.zero))
            {
                editor.curMap.transform.position = Vector3.zero;
            }
        }
    }

    // 绘制地图信息面板
    static void DrawMapInfoBox(GUIStyle boxSink, GUIStyle btnSink)
    {
        GUI.Box(new Rect(0, 0, 200, 220), "地图信息", boxSink);
        GUI.Box(new Rect(0, 0, 200, 220), "地图信息", boxSink);
        GUILayout.Label("当前编辑的地图 : " + MapEdit.editor.curMap.name);
        GUILayout.Label("ID:  " + editor.curMap.GetComponent<MapObj>().id);
        GUILayout.Label("子物体: " + editor.curMap.transform.childCount);

        mPos = GUILayout.BeginScrollView(mPos, GUILayout.Width(200), GUILayout.Height(130));
        foreach (Transform child in editor.curMap.transform)
        {
            MapObj childScript = child.GetComponent<MapObj>();
            GUILayout.BeginHorizontal();
            GUILayout.Label(childScript.id + ": " + child.name);
            if (GUILayout.Button("删除", btnSink))
            {
                DestroyImmediate(child.gameObject);
            }
            GUILayout.EndHorizontal();
        }
        GUILayout.EndScrollView();
    }

    // 绘制当前选中物体信息面板
    static void DrawSelectObjInfoBox(GUIStyle boxSink, GUIStyle btnSink)
    {
        if (Selection.activeTransform)
        {
            Vector2 centerBoxPos = new Vector2(Screen.width / 2 - centerBoxSize.x / 2, 10);
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
                if (selectObj.Equals(editor.curMap))
                {
                    if (EditorUtility.DisplayDialog("警告", "当前选中的是地图! 是否确认删除?", "Ok", "Cancel"))
                        editor.DeleteMap();
                }
                else
                    DestroyImmediate(selectObj);
            }
            GUILayout.EndArea();
        }
    }

    // 其他功能面板
    static void DrawFunctionInfoBox(GUIStyle boxSink)
    {
        Vector2 boxPos = new Vector2(Screen.width - 300, 0);
        GUILayout.BeginArea(new Rect(boxPos.x, boxPos.y, 160, 100), "功能面板", boxSink);
        GUILayout.Space(20);

        // 物体信息面板开关
        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            isDrawObjInfo = GUILayout.Toggle(isDrawObjInfo, "开启物体信息面板");
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();

        GUILayout.Space(20);

        // 快速创建开关
        GUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            isFastCreateObj = GUILayout.Toggle(isFastCreateObj, "开启快速创建物体");
            GUILayout.FlexibleSpace();
        }
        GUILayout.EndHorizontal();


        GUILayout.EndArea();
    }


    // 鼠标创建物体
    private void CreateGameObjectMouse()
    {
        Event e = Event.current;
        //Upside-down and offset a little because of menus
        Ray ray = SceneView.lastActiveSceneView.camera.ScreenPointToRay(new Vector3(e.mousePosition.x, Screen.height - e.mousePosition.y - 36, 0));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 1000.0f))
        {
            CreateGameObj(hit.point, hit.normal);
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

    decimal Round(float num)
    {
        return Math.Round((decimal)num, 2, MidpointRounding.AwayFromZero);
    }

    public void CreateCenterHorizontalLayout(Action fun)
    {
        EditorGUILayout.BeginHorizontal();
        {
            GUILayout.FlexibleSpace();
            fun();
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndHorizontal();
    }

    public void CreateCenterVerticalLayout(Action fun)
    {
        EditorGUILayout.BeginVertical();
        {
            GUILayout.FlexibleSpace();
            fun();
            GUILayout.FlexibleSpace();
        }
        EditorGUILayout.EndVertical();
    }

}
