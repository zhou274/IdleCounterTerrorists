using UnityEngine;

[CreateAssetMenu(menuName = "config/currency")]
public class CurrencyConfig : ScriptableObject
{
    public string ID;
    public string defaultAmount;
}