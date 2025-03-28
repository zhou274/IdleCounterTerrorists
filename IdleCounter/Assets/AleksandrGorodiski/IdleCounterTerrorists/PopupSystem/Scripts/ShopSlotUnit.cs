using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopSlotUnit : GameElement
{
    public Button hireButton;
    public TextMeshProUGUI unitName;
    public TextMeshProUGUI unitPrice;
    public Image unitIcon;
    public Image priceIcon;
    UnitModel unitModel = new UnitModel();
    public UITutorialSignController tutorialSignController;
    private Color iconColor;
    private Color priceColor;

    public void SetSlot(UnitConfig unitConfig, float yPosition, int i)
    {
        hireButton.onClick.AddListener(OnHireButtonClick);
        app.model.balanceModel.cash.ON_AMOUNT_CHANGE += OnCashAmountCgange;

        SetModel(unitConfig, yPosition);
        SetName();
        SetPrice();
        GetIconsColor();
        SetIconStatus(app.model.balanceModel.cash.Amount);

        if (i == 2) tutorialSignController.SetUITutorialSignNotification(app.controller.IsTutorial);
        else tutorialSignController.SetVisibility(false);
    }

    void OnCashAmountCgange(long prevValue, long newValue)
    {
        SetIconStatus(newValue);
    }

    void OnHireButtonClick()
    {
        app.controller.TryToHire(unitModel);
        tutorialSignController.PlayScaleOutAnimation();
    }

    void SetModel(UnitConfig unitConfig, float yPosition)
    {
        unitModel.Config = unitConfig;

        GameObject _unit = new GameObject();
        _unit.name = unitModel.GetID();

        PlayerShopView view = _unit.AddComponent<PlayerShopView>();
        view.SetModel(unitModel);


        _unit.transform.parent = transform;
        Vector3 _position = new Vector3(0f, yPosition, -100f);
        _unit.transform.localPosition = _position;
        _unit.transform.localScale = Vector3.one;
    }

    void SetName()
    {
        unitName.text = unitModel.GetID();
    }

    void SetPrice()
    {
        unitPrice.text = "$ " + unitModel.GetPrice().ToString();
    }

    void GetIconsColor()
    {
        iconColor = unitIcon.color;
        priceColor = priceIcon.color;
    }

    void SetIconStatus(long newValue)
    {
        if (newValue >= unitModel.GetPrice())
        {
            unitIcon.color = iconColor;
            priceIcon.color = priceColor;
        }
        else
        {
            unitIcon.color = Color.grey;
            priceIcon.color = Color.grey;
        }
    }
}
