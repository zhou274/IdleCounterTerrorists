using UnityEngine;

[CreateAssetMenu(menuName = "config/building")]
public class BuildingConfig: ScriptableObject
{
    public bool availableByDefault;

    public string ID;
    public string localizationKey;

    public Sprite picture;

    public int spriteID;
}