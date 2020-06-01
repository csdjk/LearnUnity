using UnityEngine;
using UnityEditor;

public class GameObjectEditorWindow : EditorWindow
{
    GameObject gameObject;
    Editor gameObjectEditor;

    [MenuItem("Editor教程/常用小功能/预览GameObject")]
    static void ShowWindow()
    {
        GetWindow<GameObjectEditorWindow>("GameObject Editor");
    }

    void OnGUI()
    {
        gameObject = (GameObject)EditorGUILayout.ObjectField(gameObject, typeof(GameObject), true);

        if (gameObject != null)
        {
            if (gameObjectEditor == null)
                gameObjectEditor = Editor.CreateEditor(gameObject);

            gameObjectEditor.OnPreviewGUI(GUILayoutUtility.GetRect(500, 500), EditorStyles.whiteLabel);
        }
    }
}