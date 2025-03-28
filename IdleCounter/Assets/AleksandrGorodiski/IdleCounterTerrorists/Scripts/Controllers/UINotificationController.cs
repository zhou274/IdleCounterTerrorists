using DG.Tweening;
using TMPro;
using UnityEngine;

public class UINotificationController : MonoBehaviour
{
    public RectTransform rect;
    public CanvasGroup canvasGroup;
    public Vector2 _startPosition;
    public float _durationScaleIn = 0.1f;
    public float _durationMove = 1f;
    public float _amplitudeScaleIn = 1f;
    public float _deltaPositionY = 20f;
    public TextMeshProUGUI notificationText;

    public void SetNotification()
    {
        SetStartPosition();
        PlayScaleInAnimation();
        PlayMoveAnimation();
        CanvasTransparency();
    }

    public void SetNotificationText(string value)
    {
        if(notificationText) notificationText.text = value;
    }

    void SetStartPosition()
    {
        rect.anchoredPosition = _startPosition;
    }

    Vector2 GetEndPosition()
    {
        return new Vector2(rect.anchoredPosition.x, rect.anchoredPosition.y + _deltaPositionY);
    }

    public void PlayScaleInAnimation()
    {
        rect?.DOKill();
        rect.localScale = Vector3.zero;
        rect?.DOScale(Vector3.one, _durationScaleIn).SetEase(Ease.OutBack, _amplitudeScaleIn);
    }

    public void PlayScaleOutAnimation()
    {
        rect?.DOKill();
        rect?.DOScale(Vector3.zero, _durationScaleIn).SetEase(Ease.InBack, _amplitudeScaleIn).OnComplete(HideObject);
    }

    public void PlayMoveAnimation()
    {
        rect?.DOAnchorPosY(GetEndPosition().y, _durationMove).SetEase(Ease.OutQuad).OnComplete(KillObject);
    }

    public void CanvasTransparency()
    {
        float transitionValue = 0f;
        var transitionTweener = DOTween.To(x => transitionValue = x, 1f, 0f, _durationMove)
                           .SetEase(Ease.InQuint);
        transitionTweener.OnUpdate(() => canvasGroup.alpha = transitionValue);
    }

    void KillObject()
    {
        Destroy(gameObject);
    }

    void HideObject()
    {
        SetVisibility(false);
    }

    public void SetVisibility(bool value)
    {
        gameObject.SetActive(value);
    }
}