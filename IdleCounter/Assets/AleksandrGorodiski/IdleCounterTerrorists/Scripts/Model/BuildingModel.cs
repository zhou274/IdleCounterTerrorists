using UnityEngine;
using System;

[Serializable]
public class BuildingModel
{
    public event Action<BuildingModel> ON_CONFIG_CHANGE;
    public event Action ON_BECKOME_UNLOCKED;


    public string GetID()
    {
        return Config.ID;
    }

    public int GetSpriteID()
    {
        return Config.spriteID;
    }

    public BuildingConfig GetConfig()
    {
        return Config;
    }

    public string GetLocalizationKey()
    {
        return Config.localizationKey;
    }

    public bool GetIsAvailable()
    {
        bool _IsAvailable = false;
        if (PlayerPrefs.GetInt(GetID(), GetDefaultStatus()) == 1) _IsAvailable = true;
        return _IsAvailable;
    }

    public int GetDefaultStatus()
    {
        int _defaultStatus = 0;
        if (Config.availableByDefault) _defaultStatus = 1;
        return _defaultStatus;
    }

    public void SetBeckomeUnlocked()
    {
        PlayerPrefs.SetInt(GetID(), 1);
        ON_BECKOME_UNLOCKED?.Invoke();
    }

    [SerializeField]
    private BuildingConfig _config;
    public BuildingConfig Config
    {
        get => _config;
        set
        {
            if (_config == value) return;

            _config = value;
            ON_CONFIG_CHANGE?.Invoke(this);
        }
    }
}