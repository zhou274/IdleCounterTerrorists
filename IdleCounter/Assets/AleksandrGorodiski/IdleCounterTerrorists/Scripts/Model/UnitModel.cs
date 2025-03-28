using UnityEngine;
using System;

[Serializable]
public class UnitModel
{
    public event Action ON_HEALTH_CHANGE;
    public event Action ON_DIED;

    private bool _isDead;

    [SerializeField]
    private UnitConfig _config;
    public UnitConfig Config
    {
        get => _config;
        set
        {
            if (_config == value) return;

            _config = value;
        }
    }

    public Sprite GetIcon()
    {
        return Config.icon;
    }

    public WeaponConfig GetWeaponConfig()
    {
        return Config.weaponConfig;
    }

    public string GetID()
    {
        return Config.ID;
    }

    public float GetDiedDelay()
    {
        return Config.diedDelay;
    }

    public float GetHurtOffsetDistance()
    {
        return Config.hurtOffsetDistance;
    }

    public float GetHurtOffsetTime()
    {
        return Config.hurtOffsetTime;
    }

    public int GetSpriteID()
    {
        return Config.spriteID;
    }

    public string GetLocalizationKey()
    {
        return Config.localizationKey;
    }

    public float GetSpeed()
    {
        return Config.speed;
    }

    public bool GetIsDead()
    {
        return _isDead;
    }

    public float GetNominalHealth()
    {
        return Config.health;
    }

    public int GetPrice()
    {
        return Config.price;
    }
    public int GetReward()
    {
        return Config.reward;
    }

    public bool GetIsAvailable()
    {
        bool _isAvailable;
        if (Config.isAvailableByDefault) _isAvailable = true;
        else
        {
            if (PlayerPrefs.GetInt(GetID(), 0) == 1) _isAvailable = true;
            else _isAvailable = false;
        }
        return _isAvailable;
    }

    [SerializeField]
    public float _health;
    public float Health
    {
        get => _health;
        set
        {
            if (!_isDead)
            {
                _health = value;
                ON_HEALTH_CHANGE?.Invoke();
                if (_health <= 0f)
                {
                    _isDead = true;
                    Debug.Log("UnitModel. " + GetID() + " is Dead.");
                    ON_DIED?.Invoke();
                }
            }
        }
    }
}
