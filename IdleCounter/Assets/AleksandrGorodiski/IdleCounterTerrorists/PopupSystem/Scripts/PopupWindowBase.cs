using UnityEngine;
using UnityEngine.UI;

public class PopupWindowBase : GameElement
{
    public string _windowID;
    [HideInInspector]
    public RectTransform _rect;
    public Button closeButton;
    public Vector2 _startPosition;
    public float _durationShowAnimation = 0.2f;
    public float _durationHideAnimation = 0.2f;

    public string GetWindowID()
    {
        return _windowID;
    }

    void Start()
    {
        OnShow();
    }

    public virtual void OnShow()
    {
        _rect = GetComponent<RectTransform>();
        PlayShowAnimation();
    }

    public virtual Vector2 GetStartPos()
    {
        return _startPosition;
    }

    protected virtual void PlayShowAnimation()
    {

    }

    protected virtual void PlayHideAnimation()
    {

    }

    public void HideWindow()
    {
        PlayHideAnimation();
    }

    protected virtual void OnShowAnimationComplete()
    {

    }

    protected virtual void OnHideAnimationComplete()
    {
        Destroy(gameObject);
    }
}