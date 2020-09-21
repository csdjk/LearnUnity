using UnityEngine;
using System.Collections;

public class GameRoot : MonoBehaviour {

	// Use this for initialization
	void Start () {
        UIManager.Instance.PushPanel(UIPanelType.MainMenu);
	}
	

}
