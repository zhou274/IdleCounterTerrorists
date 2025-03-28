using UnityEngine;
using System;

public class BalanceController : GameElement
{
    public BalanceView balanceView;
    private BalanceModel _balanceModel
    {
        get => app.model.balanceModel;
    }

    public void LoadBalance()
    {
        Load(_balanceModel.cash);
    }

    void Start()
    {
        _balanceModel.cash.ON_AMOUNT_CHANGE += OnCashAmountCgange;

        UpdateCashText(_balanceModel.cash.Amount, _balanceModel.cash.Amount);
    }

    void OnCashAmountCgange(long prevValue, long newValue)
    {
        Save(_balanceModel.cash, newValue);
        UpdateCashText(prevValue, newValue);
    }

    void UpdateCashText(long prevValue, long newValue)
    {
        balanceView.UpdateCashText(prevValue, newValue);
    }

    public bool CanPurchase(int price)
    {
        if (_balanceModel.cash.Amount >= price) return true;
        else return false;
    }

    public void AddCash(long value)
    {
        long _amount = _balanceModel.cash.Amount;
        long _newAmount = _amount + value;
        _balanceModel.cash.Amount = _newAmount;
    }

    public void MinusCash(long value)
    {
        long _amount = _balanceModel.cash.Amount;
        long _newAmount = _amount - value;
        _balanceModel.cash.Amount = _newAmount;
    }

    void Load(CurrencyModel model)
    {
        model.Amount = Convert.ToInt64(PlayerPrefs.GetString(model.GetID(), model.GetDefaultAmount()));
    }

    void Save(CurrencyModel model, long _value)
    {
        PlayerPrefs.SetString(model.GetID(), Convert.ToString(_value));
    }
}