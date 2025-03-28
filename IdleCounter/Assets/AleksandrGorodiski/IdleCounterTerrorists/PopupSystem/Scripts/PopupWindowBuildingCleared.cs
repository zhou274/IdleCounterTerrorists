using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using StarkSDKSpace;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;


public class PopupWindowBuildingCleared : PopupWindowBase
{
    public Button nextBuildingButton;
    public Image backgroundImage;
    public float alphaStartValue = 0f;
    public float alphaEndValue = 0.5f;
    public float alphaAnimationDuration = 0.3f;
    private RectTransform _nextLevelButtonRectTransform;
    private StarkAdManager starkAdManager;

    public string clickid;

    public override void OnShow()
    {
        closeButton.onClick.AddListener(HideWindow);
        nextBuildingButton.onClick.AddListener(OnNextBuildingButtonClick);

        SetAlpha(0f);
        NextBuildingButtonVisibility(false);

        _nextLevelButtonRectTransform = nextBuildingButton.GetComponent<RectTransform>();

        base.OnShow();
    }

    private void OnNextBuildingButtonClick()
    {
        app.controller.NextBuilding();
        AnimateBackgroundImageAlpha(alphaEndValue, alphaStartValue, alphaAnimationDuration * 0.5f);
        HideWindow();
    }

    private void OnDestroy()
    {
        closeButton.onClick.RemoveListener(HideWindow);
        nextBuildingButton.onClick.RemoveListener(OnNextBuildingButtonClick);
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
    /// <summary>
    /// 播放插屏广告
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="errorCallBack"></param>
    /// <param name="closeCallBack"></param>
    public void ShowInterstitialAd(string adId, System.Action closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            var mInterstitialAd = starkAdManager.CreateInterstitialAd(adId, errorCallBack, closeCallBack);
            mInterstitialAd.Load();
            mInterstitialAd.Show();
        }
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

    public void NextBuildingButtonVisibility(bool value)
    {
        nextBuildingButton.gameObject.SetActive(value);
    }

    public void ShowNextLevelButton()
    {
        ShowInterstitialAd("1lcaf5895d5l1293dc",
            () => {
                Debug.LogError("--插屏广告完成--");

            },
            (it, str) => {
                Debug.LogError("Error->" + str);
            });
        NextBuildingButtonVisibility(true);

        float scale = 0.5f;
        float duration = 0.2f;

        _nextLevelButtonRectTransform.localScale = new Vector3(scale, scale, scale);
        _nextLevelButtonRectTransform?.DOScale(Vector3.one, duration).SetEase(Ease.OutBack);
    }
}