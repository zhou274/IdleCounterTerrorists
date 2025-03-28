using DG.Tweening;
using UnityEngine;

public class UICashController : UINotificationController
{
    public void SetUICashNotification(int reward, Vector3 unitPosition)
    {
        SetNotificationText("+" + reward);
        SetStartPosition(unitPosition);

        PlayScaleInAnimation();
        PlayMoveAnimation();

        CanvasTransparency();
    }

    void SetStartPosition(Vector3 unitPosition)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(unitPosition);
        rect.gameObject.transform.position = screenPoint;
    }
}