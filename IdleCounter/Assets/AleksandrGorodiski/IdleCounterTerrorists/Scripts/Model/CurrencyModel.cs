using UnityEngine;
using System;

[Serializable]
public class CurrencyModel
{
    public event Action<long, long> ON_AMOUNT_CHANGE;

    public string GetDefaultAmount()
    {
        return Config.defaultAmount;
    }
    public string GetID()
    {
        return Config.ID;
    }

    [SerializeField]
    private long _amount;
    public long Amount
    {
        get => _amount;
        set
        {
            if (_amount == value) return;
            long _prevValue = _amount;
            _amount = value;
            ON_AMOUNT_CHANGE?.Invoke(_prevValue, _amount);
        }
    }

    [SerializeField]
    private CurrencyConfig _config;
    public CurrencyConfig Config
    {
        get => _config;
        set
        {
            if (_config == value) return;
            _config = value;
        }
    }
}