using DG.Tweening;
using UnityEngine;

public class UITutorialSignController : UINotificationController
{
    public void SetUITutorialSignNotification(bool isTutorial)
    {
        if (isTutorial)
        {
            SetVisibility(true);

            string _hireButton = "<color=red> hire </color>";
            string _value = "click " + _hireButton + " button to form a squad";

            SetNotificationText(_value);
            PlayScaleInAnimation();
            PlayPingPongAnimation();
        }
        else
            SetVisibility(false);
    }

    void PlayPingPongAnimation()
    {
        rect?.DOKill();
        rect.localScale = Vector3.one;
        rect?.DOScale(Vector3.one * 1.5f, _durationScaleIn).SetLoops(-1, LoopType.Yoyo);
    }
}
