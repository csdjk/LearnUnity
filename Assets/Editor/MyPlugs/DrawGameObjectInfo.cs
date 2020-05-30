
using System.Text;
using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(MapObj))]
public class DrawGameObjectInfo : Editor
{
    [DrawGizmo(GizmoType.InSelectionHierarchy | GizmoType.NotInSelectionHierarchy)]
    static void DrawGameObjectName(Transform transform, GizmoType gizmoType)
    {
        if (!MapEdit.isDrawObjInfo)
            return;
        MapObj obj = transform.GetComponent<MapObj>();
        StringBuilder str = new StringBuilder();
        Color color = Color.white;
        if (obj)
        {
            str.Append("ID: " + obj.id);
            str.Append("\n");
            color = obj.nameColor;
        }
        if (obj == null || obj.id < 10000)
        {
            return;
        }

        str.Append("name: " + transform.gameObject.name);
        str.Append("\n");
        str.Append("pos: " + transform.position.ToString());
        str.Append("\n");

        GUIStyle style = new GUIStyle("textfield");
        style.normal.textColor = color;
        style.fixedHeight = 50;
        Handles.Label(transform.position + new Vector3(0, 2f, 0), str.ToString(), style);
    }
   
}