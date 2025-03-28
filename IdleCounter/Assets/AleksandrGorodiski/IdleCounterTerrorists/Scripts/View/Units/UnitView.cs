using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using DG.Tweening;

public class UnitView : GameElement
{
    public UnitModel model;
    public GameObject skinUnit;
    public UnitSkinSettings unitSkinSettings;
    public WeaponController weaponController;
    public UnitPlaceController placeController;
    public Transform _target;

    void OnShootStarted()
    {
        unitSkinSettings.unitAnimation.PlayAttackAnimation();
    }

    void OnHealthChange()
    {
        UpdateHealthBar();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("Bullet"))
        {
            OnBulletHit(collider);
        }
    }

    public void OnBulletHit(Collider collider)
    {
        if (!model.GetIsDead())
        {
            BulletController bulletController = collider.GetComponent<BulletController>();
            float _damage = bulletController.Damage;
            if (_damage > 0f)
            {
                float _health = model.Health;
                float _newHealth = _health - _damage;
                float _actualDamage;
                if (_newHealth <= 0f)
                {
                    _actualDamage = _health;
                }
                else
                {
                    _actualDamage = _damage;
                }

                bulletController.DamageCount(_actualDamage);
                model.Health = _newHealth;

                PlayParticles();
                ChangeMaterial();
            }
        }
    }

    void OffsetUnit()
    {
        StartCoroutine(CoroutineOffsetUnit());
    }

    IEnumerator CoroutineOffsetUnit()
    {
        float currentTime = 0;
        Vector3 vector = transform.position;
        Vector3 _startPos = transform.position;
        Vector3 _endPos = vector -= transform.forward * model.GetHurtOffsetDistance();

        while (currentTime <= model.GetHurtOffsetTime())
        {
            currentTime += Time.deltaTime;
            transform.position = Vector3.Slerp(_startPos, _endPos, currentTime / model.GetHurtOffsetTime());
            yield return null;
        }
    }

    public void PlayParticles()
    {
        unitSkinSettings.bloodParticles.Play();
    }

    public void ChangeMaterial()
    {
        unitSkinSettings.SetMaterial(unitSkinSettings.hitMaterial);
        StartCoroutine(CoroutineChangeMaterialBack());
    }

    IEnumerator CoroutineChangeMaterialBack()
    {
        yield return new WaitForSeconds(0.08f);
        unitSkinSettings.SetMaterial(unitSkinSettings.normalMaterial);
    }

    public virtual void AddUnitTransform()
    {
        app.view.AddEnemyTransform(transform);
    }

    public virtual void RemoveUnitTransform()
    {
        app.view.RemoveEnemyTransform(transform);
    }

    private void Subscribe()
    {
        model.ON_HEALTH_CHANGE += OnHealthChange;
        model.ON_DIED += OnDied;

        app.controller.ON_BUILDING_CHANGED += OnBuildingChanged;
        app.controller.ON_BUILDING_CLEARED += OnBuildingCleared;
    }

    private void OnDestroy()
    {
        model.ON_HEALTH_CHANGE -= OnHealthChange;
        model.ON_DIED -= OnDied;

        if (app) app.controller.ON_BUILDING_CHANGED -= OnBuildingChanged;
        if (app) app.controller.ON_BUILDING_CLEARED -= OnBuildingCleared;
    }

    public virtual void OnBuildingCleared()
    {

    }
    public virtual void OnBuildingChanged()
    {

    }

    public virtual void OnDied()
    {
        StopUnit();
        ResetRotationX();
        OffsetUnit();
        unitSkinSettings.unitAnimation.PlayDeadAnimation();
        RemoveUnitTransform();
        DestroyUnit(app.model.GetDelayUnitDestroy());
    }

    public void DestroyUnit(float delay)
    {
        StartCoroutine(CoroutineDestroyUnit(delay));
    }

    IEnumerator CoroutineDestroyUnit(float delay)
    {
        yield return new WaitForSeconds(delay);
        RemoveUnitTransform();
        if(placeController) placeController.IsTaken = false;
        Destroy(gameObject);
    }

    public void SetModel(UnitModel _model)
    {
        model = _model;

        Subscribe();
        UpdateSkin(model);
        GetSkinSettings();
        GetNavMeshAgent();
        CreateWeapon();
        UpdateHealthOnLevelStarts();
        UpdateHealthBar();
        UpdateReloadBar(0f, 1f);
        PlayAnimationOnStart();
        SetLayerOnStart();
    }

    public virtual void SetLayerOnStart()
    {
        
    }

    public virtual void PlayAnimationOnStart()
    {
        unitSkinSettings.unitAnimation.PlayWalkAnimation();
    }

    void PlayAnimationOnArrived()
    {
        unitSkinSettings.unitAnimation.PlayIdleAnimation();
    }

    public void SetUnitRotation(float value)
    {
        transform.eulerAngles = new Vector3(0f, value, 0f);
    }

    void ResetRotationX()
    {
        Vector3 vector3 = transform.eulerAngles;
        transform.eulerAngles = new Vector3(0f, vector3.y, vector3.z);
    }

    public void SetPosition(Vector3 value)
    {
        transform.position = value;
    }

    public void SetPlaceController(UnitPlaceController _placeController)
    {
        placeController = _placeController;
        placeController.IsTaken = true;
    }

    public void ClearPath()
    {
        path.Clear();
    }

    public void AddPointToPath(Vector3 value)
    {
        path.Add(value);
    }

    public void SetTargetNull()
    {
        _target = null;
    }

    protected virtual void UpdateHealthOnLevelStarts()
    {
        model.Health = model.GetNominalHealth();
    }

    void UpdateHealthBar()
    {
        unitSkinSettings.healthBarController.UpdateBar(model.Health / model.GetNominalHealth());
    }

    void UpdateReloadBar(float currentTime, float totalTime)
    {
        if (model.GetIsDead()) currentTime = totalTime;
        unitSkinSettings.reloadBarController.UpdateBar(1f - (currentTime / totalTime));
    }

    protected virtual void UpdateSkin(UnitModel _model)
    {
        Debug.Log("UnitView. UpdateView: " + _model.GetID());
        if (_model.Config != null)
        {
            if (skinUnit != null) Destroy(skinUnit);
            skinUnit = Instantiate(_model.Config.skin, transform);
        }
    }

    protected virtual void GetSkinSettings()
    {
        if (skinUnit != null)
        {
            unitSkinSettings = skinUnit.GetComponent<UnitSkinSettings>();
            unitSkinSettings.unitAnimation = skinUnit.GetComponent<UnitAnimation>();

            Vector3 _scale = unitSkinSettings.GetUnitTransformNode().localScale;
            if (Random.Range(0, 2) == 0) unitSkinSettings.GetUnitTransformNode().localScale = new Vector3(-_scale.x, _scale.y, _scale.z);
        }
    }

    protected virtual void GetNavMeshAgent()
    {

    }

    public virtual void CreateWeapon()
    {
        if (app.gameSettings.spawnWeapon)
        {
            if (unitSkinSettings.GetWeaponNode())
            {
                if (weaponController) Destroy(weaponController.gameObject);

                if (model.GetWeaponConfig())
                {
                    WeaponModel weaponModel  = new WeaponModel();
                    weaponModel.Config = model.GetWeaponConfig();
                    CreateWeaponView(weaponModel);
                } 
                else Debug.LogError("UnitView. UnitConfig has NO WeaponConfig.");
            }
            else Debug.LogError("UnitView. Skin Settings has NO WEAPON NODE.");
        }
    }

    void CreateWeaponView(WeaponModel model)
    {
        if (app.gameSettings.spawnWeapon)
        {
            GameObject weapon = new GameObject();
            weapon.name = model.GetID();
            weapon.transform.parent = unitSkinSettings.GetWeaponNode();

            weaponController = weapon.AddComponent<WeaponController>();
            weaponController.ON_RELOADING += UpdateReloadBar;
            weaponController.CreateWeapon(model);
        }
    }

    public int CurrentWayPointID;
    float reachDistance = 0.3f;
    public float speedWalk = 10f;
    float speedRotationWalk = 5f;
    float speedRotationAim = 50f;
    public List<Vector3> path = new List<Vector3>();
    public bool isArrived;

    protected virtual void MoveUnit()
    {
        transform.position = Vector3.MoveTowards(transform.position, path[CurrentWayPointID], Time.deltaTime * speedWalk);
    }

    protected virtual void RotateUnit()
    {
        Vector3 vector = path[CurrentWayPointID] - transform.position;
        if (vector == Vector3.zero) return;
        var rotation = Quaternion.LookRotation(vector);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, Time.deltaTime * speedRotationWalk);
    }

    protected virtual void StopUnit()
    {
        
    }

    public virtual void Update()
    {
        if (!model.GetIsDead())
        {
            if (!isArrived)
            {
                float distance = Vector3.Distance(path[CurrentWayPointID], transform.position);
                MoveUnit();
                RotateUnit();

                if (distance <= reachDistance)
                {
                    CurrentWayPointID++;
                    if (CurrentWayPointID >= path.Count)
                    {
                        isArrived = true;
                        AddUnitTransform();
                        PlayAnimationOnArrived();
                        SetUnitRotation(180f);
                    }
                }
            }
            else
            {
                if (_target && app.controller.IsLevelStarted)
                {
                    OnUnitShoot();
                    var lookPos = (_target.position + (Vector3.up * 0.7f)) - transform.position;
                    //lookPos.y = 0;
                    var rotationB = Quaternion.LookRotation(lookPos);
                    transform.rotation = Quaternion.Slerp(transform.rotation, rotationB, Time.deltaTime * speedRotationAim);
                }
                else
                {
                    if(weaponController) GetClosestUnit(app.view.playersTransforms);
                }
            }
        }
    }

    public void OnUnitShoot()
    {
        if (weaponController)
        {
            weaponController.Shoot(OnShootStarted);
        }
    }

    float minDist;
    public virtual Transform GetClosestUnit(List<Transform> unitTransforms)
    {
        minDist = Mathf.Infinity;
        if (unitTransforms.Count > 0)
        {
            foreach (Transform temp in unitTransforms)
            {
                if (temp)
                {
                    float dist = Vector3.Distance(temp.position, transform.position);
                    if (dist < minDist && dist < weaponController.model.GetRangeOfFire())
                    {
                        _target = temp;
                        _target.GetComponent<UnitView>().model.ON_DIED += SetTargetNull;
                        minDist = dist;
                    }
                }

            }
        }
        return _target;
    }
}