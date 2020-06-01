using System;
using UnityEditor;
using UnityEngine;
// using UnityEngine.UI;

public class CreateSphereAssetEditor : EditorWindow
{
    private int sub;
    private PreviewRenderUtility prev;
    private Mesh m;
    private Material mat;

    [MenuItem("Generate/Sphere")]
    public static void ShowWindow()
    {
        EditorWindow.GetWindow(typeof(CreateSphereAssetEditor));
    }

    private void OnGUI()
    {
        GUILayout.Label("Settings");
        sub = EditorGUILayout.IntSlider("Subdivison Depth", sub, 0, 5);
        bool g = GUILayout.Button("Generate");
        bool b = GUILayout.Button("Save");

        Rect r = new Rect(0, 0, 512, 512);

        if (mat == null)
        {
            mat = Resources.Load<Material>("_Game/Materials/PlanetSurface");
        }

        if (prev == null)
        {
            prev = new PreviewRenderUtility();
        }

        if (g)
        {
            // SphereMesh sm = new SphereMesh(sub);
            // m = sm.Mesh;
        }

        if (m != null && mat != null && prev != null)
        {
            

            prev.camera.transform.position = -Vector3.forward * 5;
            prev.camera.transform.rotation = Quaternion.identity;

            prev.BeginPreview(r, GUIStyle.none);
            prev.DrawMesh(m, Vector3.zero, Quaternion.identity, mat, 0);
            
            prev.Render();
            GUI.DrawTexture(r,prev.EndPreview());
            
        }


        if (b) SaveMesh();
    }

    private void SaveMesh()
    {
        string path = EditorUtility.SaveFilePanel("Save Mesh to folder", "Assets/", "", ".asset");
        if (path.Length == 0) return;
        path = FileUtil.GetProjectRelativePath(path);

        AssetDatabase.CreateAsset(m, path);
        AssetDatabase.SaveAssets();
    }
}