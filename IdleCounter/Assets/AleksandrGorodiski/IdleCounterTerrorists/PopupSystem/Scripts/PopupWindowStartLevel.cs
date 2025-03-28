using UnityEngine.UI;
using DG.Tweening;

public class PopupWindowStartLevel : PopupWindowBase
{
    public Button startLevelButton;

    public override void OnShow()
    {
        base.OnShow();

        closeButton.onClick.AddListener(HideWindow);
        startLevelButton.onClick.AddListener(OnStartLevelButtonClick);
    }

    private void OnStartLevelButtonClick()
    {
        app.controller.IsLevelStarted = true;
        HideWindow();
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(HideWindow);
    }

    protected override void PlayShowAnimation()
    {
        _rect.DOKill();
        _rect.anchoredPosition = GetStartPos();
        _rect?.DOAnchorPosY(0f, _durationShowAnimation).SetEase(Ease.OutBack);
    }

    protected override void PlayHideAnimation()
    {
        _rect.DOKill();
        _rect?.DOAnchorPosY(GetStartPos().y, _durationHideAnimation).SetEase(Ease.InBack).OnComplete(OnHideAnimationComplete);
    }
}