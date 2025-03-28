using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PopupWindowShop : PopupWindowBase
{
    public GameObject container;
    public GameObject shopSlotUnit;

    public GridLayoutGroup _containerGridLayoutGroup;
    public RectTransform _containerRectTransform;

    public RectTransform cameraHolder;
    public Camera shopCamera;
    public RawImage viewFromCameraRawImage;

    public override void OnShow()
    {
        base.OnShow();

        closeButton.onClick.AddListener(HideWindow);

        SetContainerSize(app.globalConfigs.playerConfigs.Length);
        InstantiateUnits(app.globalConfigs.playerConfigs);

        SetCameraSize();
        SetView();
    }

    void SetContainerSize(int unitsCount)
    {
        float rows = Mathf.Ceil(unitsCount / (_containerGridLayoutGroup.constraintCount * 1f));
        float _ySize = (rows * (_containerGridLayoutGroup.cellSize.y + _containerGridLayoutGroup.spacing.y)) + _containerGridLayoutGroup.padding.bottom;
        _containerRectTransform.sizeDelta = new Vector2(_containerRectTransform.sizeDelta.x, _ySize);
    }

    public void InstantiateUnits(UnitConfig[] _unitsConfigs)
    {
        for (int i = 0; i < _unitsConfigs.Length; i++)
        {
            Instantiate(shopSlotUnit, container.transform).GetComponent<ShopSlotUnit>().SetSlot(_unitsConfigs[i], cameraHolder.anchoredPosition.y, i);
        }
    }

    void SetCameraSize()
    {
        shopCamera.orthographicSize = Screen.height * 0.5f;
    }

    void SetView()
    {
        RenderTexture renderTexture = new RenderTexture(Screen.width, Screen.height, 24);
        shopCamera.targetTexture = renderTexture;

        float screenRatio = (1f * Screen.width) / (1f * Screen.height);
        viewFromCameraRawImage.GetComponent<AspectRatioFitter>().aspectRatio = screenRatio;

        viewFromCameraRawImage.gameObject.SetActive(true);
        viewFromCameraRawImage.texture = renderTexture;
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(HideWindow);
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
}