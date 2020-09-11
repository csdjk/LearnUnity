using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class AddForce : MonoBehaviour {

    public float forceAmount = 100;

    /// <summary>
    /// OnMouseDown is called when the user has pressed the mouse button while
    /// over the GUIElement or Collider.
    /// </summary>
    void OnMouseDown () {
        if (CheckGuiRaycastObjects ())
        {
            return;
        }
        Debug.Log ("mouse down" + CheckGuiRaycastObjects ());
        Rigidbody mRigidbody = this.GetComponent<Rigidbody> ();
        mRigidbody.AddForce (Vector3.forward * forceAmount);
        mRigidbody.useGravity = true;
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update () {

    }

    bool CheckGuiRaycastObjects () {
        PointerEventData eventData = new PointerEventData (EventSystem.current);
        eventData.pressPosition = Input.mousePosition;
        eventData.position = Input.mousePosition;
        List<RaycastResult> list = new List<RaycastResult> ();
        // Main.Instance.graphicRaycaster.Raycast(eventData, list);
        EventSystem.current.RaycastAll (eventData, list);
        //Debug.Log(list.Count);
        return list.Count > 0;
    }
}
