using UnityEngine;

public class GameSettings : GameElement
{
    [Header("Target Frame Rate")]
    public int targetFrameRate = 60;

    [Header("Cheating")]
    public bool isCheating;

    [Header("Spawn Enemies")]
    public bool spawnEnemies;

    [Header("Spawn Weapon")]
    public bool spawnWeapon;

    private void Awake()
    {
        Application.targetFrameRate = targetFrameRate;
    }
}
