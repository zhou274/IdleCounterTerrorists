using UnityEngine;
using System;

[Serializable]
public class WeaponModel
{
    public event Action<WeaponModel> ON_CONFIG_CHANGE;
    [SerializeField]
    private WeaponConfig _config;
    public WeaponConfig Config
    {
        get => _config;
        set
        {
            if (_config == value) return;

            _config = value;
            ON_CONFIG_CHANGE?.Invoke(this);
        }
    }

    public bool GetIsMelee()
    {
        return Config.isMelee;
    }
    public float GetRangeOfFire()
    {
        return Config.rangeOfFire;
    }
    public float GetTotalFireTime()
    {
        return Config.rateOfFire * Config.cartridgesCount;
    }
    public float GetReloadTime()
    {
        return Config.reloadTime;
    }
    public GameObject GetSkin()
    {
        return Config.skin;
    }
    public GameObject GetBulletSkin()
    {
        return Config.bulletSkin;
    }
    public GameObject GetCatridgeSkin()
    {
        return Config.catridgeSkin;
    }
    public string GetID()
    {
        return Config.ID;
    }
    public float GetRateOfFire()
    {
        return Config.rateOfFire;
    }
    public float GetCatridgeCaseMass()
    {
        return Config.catridgeCaseMass;
    }
    public float GetCatridgeCaseForce()
    {
        return Config.catridgeCaseForce;
    }
    public float[] GetCatridgeCaseTorque()
    {
        return Config.catridgeCaseTorque;
    }
    public float GetCatridgeCaseDestroyTime()
    {
        return Config.catridgeCaseDestroyTime;
    }
    public float GetCatridgeCaseScale()
    {
        return Config.catridgeCaseScale;
    }
    public float[] GetCatridgeCaseDirectionRight()
    {
        return Config.catridgeCaseDirectionRight;
    }
    public float[] GetCatridgeCaseDirectionUp()
    {
        return Config.catridgeCaseDirectionUp;
    }
    public float[] GetCatridgeCaseDirectionForward()
    {
        return Config.catridgeCaseDirectionForward;
    }
    public float[] GetMuzzleFlashScale()
    {
        return Config.muzzleFlashScale;
    }
    public float GetDamage()
    {
        return Config.damage;
    }
    public float GetAccuracy()
    {
        return Config.accuracy;
    }
    public float GetBulletSpeed()
    {
        return Config.bulletSpeed;
    }
    public float GetBulletLoadScale()
    {
        return Config.bulletLoadScale;
    }
    public float GetBulletScale()
    {
        return Config.bulletScale;
    }
    public float GetOffsetTimeBackward()
    {
        return Config.offsetTimeBackward;
    }
    public float GetOffsetTimeForward()
    {
        return Config.offsetTimeForward;
    }
    public float GetOffsetDistance()
    {
        return Config.offsetDistance;
    }
}