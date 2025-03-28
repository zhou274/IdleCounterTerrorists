using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupWindowLevelComplete : PopupWindowBase
{
    public Button nextLevelButton;
    public Image backgroundImage;
    public float alphaStartValue = 0f;
    public float alphaEndValue = 0.5f;
    public float alphaAnimationDuration = 0.3f;
    public RectTransform _nextLevelButtonRectTransform;

    public override void OnShow()
    {
        closeButton.onClick.AddListener(HideWindow);
        nextLevelButton.onClick.AddListener(OnNextLevelButtonClick);

        SetAlpha(0f);
        NextLevelButtonVisibility(false);

        _nextLevelButtonRectTransform = nextLevelButton.GetComponent<RectTransform>();

        base.OnShow();
    }

    private void OnNextLevelButtonClick()
    {
        app.controller.NextLevel();
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(HideWindow);
        nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClick);
    }

    protected override void PlayShowAnimation()
    {
        _rect.DOKill();
        _rect.anchoredPosition = GetStartPos();
        _rect?.DOAnchorPosY(0f, _durationShowAnimation).SetEase(Ease.OutBack).OnComplete(OnShowAnimationComplete);
    }

    protected override void PlayHideAnimation()
    {
        _rect.DOKill();
        _rect?.DOAnchorPosY(GetStartPos().y, _durationHideAnimation).SetEase(Ease.InBack).OnComplete(OnHideAnimationComplete);
    }

    protected override void OnShowAnimationComplete()
    {
        ShowNextLevelButton();
        AnimateBackgroundImageAlpha(alphaStartValue, alphaEndValue, alphaAnimationDuration);
    }

    void AnimateBackgroundImageAlpha(float alphaStartValue, float alphaEndValue, float alphaAnimationDuration)
    {
        float value = 0f;
        var transitionTweener = DOTween.To(x => value = x, alphaStartValue, alphaEndValue, alphaAnimationDuration)
                           .SetEase(Ease.InQuint);
        transitionTweener.OnUpdate(() => SetAlpha(value));
    }

    void SetAlpha(float value)
    {
        Color color = backgroundImage.color;
        color.a = value;
        backgroundImage.color = color;
    }

    public void NextLevelButtonVisibility(bool value)
    {
        nextLevelButton.gameObject.SetActive(value);
    }

    public void ShowNextLevelButton()
    {
        NextLevelButtonVisibility(true);

        float scale = 0.5f;
        float duration = 0.2f;

        _nextLevelButtonRectTransform.localScale = new Vector3(scale, scale, scale);
        _nextLevelButtonRectTransform?.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }
}