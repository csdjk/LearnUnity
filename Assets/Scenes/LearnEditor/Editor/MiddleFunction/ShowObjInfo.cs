using UnityEngine;
using UnityEditor;
public class ShowObjInfo : Editor
{
    // 选中状态时绘制
    [DrawGizmo(GizmoType.InSelectionHierarchy)]
    static void DrawGameObjectInfo1(Transform transform, GizmoType gizmoType)
    {
        if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name != "Editor")
            return;
        GUIStyle style = new GUIStyle("textfield");
        style.normal.textColor = Color.yellow;
        Handles.Label(transform.position + new Vector3(0, 1.5f, 0), "这是选中时的标签 - 长生但酒狂", style);
    }


    // 未选中状态时绘制
    [DrawGizmo(GizmoType.NotInSelectionHierarchy)]
    static void DrawGameObjectInfo2(Transform transform, GizmoType gizmoType)
    {
        if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name != "Editor")
            return;
        GUIStyle style = new GUIStyle("textfield");
        style.normal.textColor = Color.white;
        Handles.Label(transform.position + new Vector3(0, 1.5f, 0), "这是未选中的标签 - 长生但酒狂", style);
    }


    // // 绘制全部
    // [DrawGizmo(GizmoType.NotInSelectionHierarchy | GizmoType.InSelectionHierarchy)]
    // static void DrawGameObjectInfo3(Transform transform, GizmoType gizmoType)
    // {
    // if (UnityEditor.SceneManagement.EditorSceneManager.GetActiveScene().name != "Editor")
    // return;
    //     GUIStyle style = new GUIStyle("textfield");
    //     style.normal.textColor = Color.white;
    //     Handles.Label(transform.position + new Vector3(0, 1.5f, 0), "这是绘制全部的标签 - 长生但酒狂", style);
    // }
}
