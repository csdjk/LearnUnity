using UnityEngine;
using System.Collections;
// using DG.Tweening;

public class KnapsackPanel : BasePanel {

    private CanvasGroup canvasGroup;

    void Start()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
    }


    public override void OnEnter()
    {
        if (canvasGroup == null) canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 1;
        canvasGroup.blocksRaycasts = true;

        Vector3 temp = transform.localPosition;
        temp.x = 600;
        transform.localPosition = temp;
        // transform.DOLocalMoveX(0, .5f);
    }

    public override void OnExit()
    {
        canvasGroup.alpha = 0;
        canvasGroup.blocksRaycasts = false;

        // transform.DOLocalMoveX(600, .5f).OnComplete(()=>canvasGroup.alpha = 0);
    }

    public override void OnPause()
    {
        canvasGroup.blocksRaycasts = false;
    }

    public override void OnResume()
    {
        canvasGroup.blocksRaycasts = true;
    }

    public void OnClosePanel()
    {
        UIManager.Instance.PopPanel();
    }
    public void OnItemButtonClick()
    {
        UIManager.Instance.PushPanel(UIPanelType.ItemMessage);
    }
}
