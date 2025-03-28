using UnityEngine;

public class GameModel : GameElement
{
    public BalanceModel balanceModel = new BalanceModel();
    [Header("Spawn Players Rate")]
    [SerializeField]
    private float _spawnPlayersRate = 0.2f;
    [Header("Players On Start")]
    [SerializeField]
    private int _playersOnStart = 3;

    public int _enemiesNeedToKillLevel;
    public int EnemiesNeedToKillLevel
    {
        get => _enemiesNeedToKillLevel;
        set
        {
            if (value != _enemiesNeedToKillLevel)
            {
                _enemiesNeedToKillLevel = value;
            }
        }
    }

    private int _enemiesKilledBuilding;
    public int EnemiesKilledBuilding
    {
        get => _enemiesKilledBuilding;
        set
        {
            if (value != _enemiesKilledBuilding)
            {
                _enemiesKilledBuilding = value;
            }
        }
    }

    public int _enemiesKilledLevel;
    public int EnemiesKilledLevel
    {
        get => _enemiesKilledLevel;
        set
        {
            if (value != _enemiesKilledLevel)
            {
                _enemiesKilledLevel = value;
            }
        }
    }

    private int _enemiesSpawned;
    public int EnemiesSpawned
    {
        get => _enemiesSpawned;
        set
        {
            if (value != _enemiesSpawned)
            {
                _enemiesSpawned = value;
            }
        }
    }
    [SerializeField]
    private int _levelDefault;
    private int _level;
    public int Level
    {
        get => _level = PlayerPrefs.GetInt("level", _levelDefault);
        set
        {
            if (value != _level)
            {
                _level = value;
                PlayerPrefs.SetInt("level", _level);
            }
        }
    }

    [SerializeField]
    private int _buildingDefault;
    private int _building;
    public int Building
    {
        get => _building = PlayerPrefs.GetInt("building", _buildingDefault);
        set
        {
            if (value != _building)
            {
                _building = value;
                PlayerPrefs.SetInt("building", _building);
            }
        }
    }

    [SerializeField]
    private int _delayUnitDestroy = 3;
    [Header("Delays Popup Open")]
    [SerializeField]
    private float _levelComplete = 1;
    [SerializeField]
    private float _startLevel = 1;

    public float GetDelayUnitDestroy()
    {
        return _delayUnitDestroy;
    }
    public float GetDelayOpenPopupLevelComplete()
    {
        return _levelComplete;
    }
    public float GetDelayOpenPopupStartLevel()
    {
        return _startLevel;
    }
    public float GetSpawnPlayersRate()
    {
        return _spawnPlayersRate;
    }
    public float GetLevelDefault()
    {
        return _levelDefault;
    }
}