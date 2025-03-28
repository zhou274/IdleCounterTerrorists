using UnityEngine;

public class PopupWindowController : MonoBehaviour
{
    public PopupWindowConfig popupWindowConfig;
    public PopupWindowBase activePopup;

    public void ShowWindow(string _ID)
    {
        foreach (GameObject _gameObject in popupWindowConfig.windowPrefabs)
        {
            PopupWindowBase _popup = _gameObject.GetComponent<PopupWindowBase>();
            if (_popup.GetWindowID() == _ID)
            {
                GameObject _out = Instantiate(_gameObject, transform);
                activePopup = _out.GetComponent<PopupWindowBase>();
                return;
            }
        }
    }

    public void NotificationCash(int reward, Vector3 unitPosition)
    {
        GameObject _out = Instantiate(popupWindowConfig.notificationCashPrefab, transform);
        _out.GetComponent<UICashController>().SetUICashNotification(reward, unitPosition);
    }

    public void NotificationSquadFull()
    {
        GameObject _out = Instantiate(popupWindowConfig.notificationSquadFullPrefab, transform);
        _out.GetComponent<UINotificationController>().SetNotification();
    }

    public void NotificationNotEnoughCash()
    {
        GameObject _out = Instantiate(popupWindowConfig.notificationNotEnoughCashPrefab, transform);
        _out.GetComponent<UINotificationController>().SetNotification();
    }

    public void HideActiveWindow()
    {
        activePopup.HideWindow();
    }
}