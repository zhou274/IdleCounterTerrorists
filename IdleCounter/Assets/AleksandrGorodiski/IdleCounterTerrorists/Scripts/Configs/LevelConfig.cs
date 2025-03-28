using UnityEngine;

[CreateAssetMenu(menuName ="config/level")]
public class LevelConfig: ScriptableObject
{
    public Sprite icon;
    public string ID;
    public GameObject skin;
}