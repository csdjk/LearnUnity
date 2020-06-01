

using UnityEditor;
using UnityEngine;

// ---------------------------【预览点击物体】---------------------------
[UnityEditor.CustomEditor(typeof(ObjTest), true)]
public class PreviewTest : UnityEditor.Editor
{
    public override bool HasPreviewGUI() { return true; }
    private PreviewRenderUtility _previewRenderUtility;
    Editor gameObjectEditor;

    public override void OnPreviewGUI(Rect r, GUIStyle background)
    {
        GameObject obj = (target as ObjTest).gameObject ;
        if (obj)
        {
            if (gameObjectEditor == null)
                gameObjectEditor = Editor.CreateEditor(obj);
            gameObjectEditor.OnPreviewGUI(r, background);
        }

    }
}