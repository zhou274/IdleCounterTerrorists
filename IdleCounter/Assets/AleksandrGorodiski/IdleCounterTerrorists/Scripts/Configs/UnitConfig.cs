using UnityEngine;

[CreateAssetMenu(menuName ="config/unit")]
public class UnitConfig: ScriptableObject
{
    public GameObject skin;
    public Sprite icon;

    public string ID;
    public string localizationKey;

    public bool isAvailableByDefault;
    public float speed;
    public int spriteID;

    public float diedDelay;
    public float hurtOffsetDistance;
    public float hurtOffsetTime;

    public float health;

    public int price;
    public int reward;

    public WeaponConfig weaponConfig;
}