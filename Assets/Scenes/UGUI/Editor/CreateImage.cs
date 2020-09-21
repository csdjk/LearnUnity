using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 创建Image 默认设置 raycastTarget 为false
/// </summary>
public class CreateImage : Editor
{
   [MenuItem("GameObject/UI/Image (raycastTarget is false)")]
	static void CreatImage()
	{
		if(Selection.activeTransform)
		{
			if(Selection.activeTransform.GetComponentInParent<Canvas>())
			{
				GameObject go = new GameObject("image",typeof(Image));
				go.GetComponent<Image>().raycastTarget = false;
				go.transform.SetParent(Selection.activeTransform);
			}
		}
	}


}
