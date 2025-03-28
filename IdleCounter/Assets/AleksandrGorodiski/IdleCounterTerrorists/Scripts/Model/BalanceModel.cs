using System;

[Serializable]
public class BalanceModel
{
    public CurrencyModel cash;

    public BalanceModel GetBalanceModel()
    {
        return this;
    }
}