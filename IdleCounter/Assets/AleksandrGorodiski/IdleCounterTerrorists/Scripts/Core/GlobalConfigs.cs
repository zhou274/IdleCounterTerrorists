using System.Collections.Generic;
using UnityEngine;


public class GlobalConfigs: GameElement
{
    [Header("Players")]
    public UnitConfig[] playerConfigs;
    [Header("Weapons")]
    public List<WeaponConfig> weaponConfigs;
    [Header("Enemies")]
    public List<UnitConfig> enemyConfigs;
    [Header("Levels")]
    public List<LevelConfig> levelsConfigs;
}

