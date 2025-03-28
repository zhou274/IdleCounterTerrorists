using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[CreateAssetMenu(menuName ="config/weapon")]
public class WeaponConfig: ScriptableObject
{
    public bool isMelee;
    public GameObject skin;
    public GameObject bulletSkin;
    public GameObject catridgeSkin;

    public string ID;

    [Header("Weapon")]
    public float damage = 1f;
    [SerializeField, Range(3f, 10f)] public float reloadTime = 3f;
    [SerializeField, Range(0.1f, 10f)] public float rateOfFire = 1f;
    public float accuracy = 1f;
    [SerializeField, Range(0.5f, 500f)] public float rangeOfFire = 50f;
    [SerializeField, Range(1, 50)] public int cartridgesCount = 1;
    [Header("Catridge Case")]
    public float catridgeCaseMass = 1f;
    public float catridgeCaseForce = 5f;
    public float[] catridgeCaseTorque = { -10000f, 10000f };
    public float catridgeCaseScale = 1;
    public float catridgeCaseDestroyTime = 3f;
    [Header("Catridge Case Direction")]
    public float[] catridgeCaseDirectionRight = { 1f, 1f };
    public float[] catridgeCaseDirectionUp = { 1f, 1f };
    public float[] catridgeCaseDirectionForward = { 1f, 1f };
    [Header("Bullet")]
    public float bulletSpeed = 50f;
    public float bulletLoadScale = 0f;
    public float bulletScale = 1f;
    [Header("Muzzle Flash")]
    public float[] muzzleFlashScale = { 0.5f, 0.5f };
    [Header("Recoil Animation")]
    public float offsetTimeForward = 0.1f;
    public float offsetTimeBackward = 0.2f;
    public float offsetDistance = 1f;
}


