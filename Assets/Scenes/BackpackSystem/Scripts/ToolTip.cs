using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ToolTip : MonoBehaviour {

    private Text toolTipText;
    private Text contentText;
    private CanvasGroup canvasGroup;

    private float targetAlpha = 0 ;

    public float smoothing = 1;

    void Start()
    {
        toolTipText = GetComponent<Text>();
        contentText = transform.Find("Content").GetComponent<Text>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    void Update()
    {
        if (canvasGroup.alpha != targetAlpha)
        {
            canvasGroup.alpha = Mathf.Lerp(canvasGroup.alpha, targetAlpha,smoothing*Time.deltaTime);
            if (Mathf.Abs(canvasGroup.alpha - targetAlpha) < 0.01f)
            {
                canvasGroup.alpha = targetAlpha;
            }
        }
    }

    public void Show(string text)
    {
        toolTipText.text = text;
        contentText.text = text;
        targetAlpha = 1;
    }
    public void Hide()
    {
        targetAlpha = 0;
    }
    public void SetLocalPotion(Vector3 position)
    {
        transform.localPosition = position;
    }
	
}
