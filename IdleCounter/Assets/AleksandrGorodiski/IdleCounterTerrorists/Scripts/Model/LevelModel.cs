using UnityEngine;
using System;

[Serializable]
public class LevelModel
{
    [SerializeField]
    private LevelConfig _config;
    public LevelConfig Config
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

    public string GetID()
    {
        return Config.ID;
    }

    public GameObject GetSkin()
    {
        return Config.skin;
    }
}