using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponController : MonoBehaviour
{
    public WeaponModel model = new WeaponModel();
    private WeaponSkinSettings weaponSkinSettings;
    private GameObject skin;
    public List<GameObject> _bullets = new List<GameObject>();
    public bool isLoaded;
    private Vector3 _startPos;
    public float _lastFireTime;
    public float _reloadCounter;
    public float _fireTime;
    public bool isReloading;
    public event Action<float, float> ON_RELOADING;


    public void Shoot(Action OnShootStarted)
    {
        if (!isReloading && _lastFireTime >= model.GetRateOfFire())
        {
            OnShootStarted?.Invoke();

            _lastFireTime = 0f;
            StartCoroutine(CoroutineShoot());

            _fireTime += model.GetRateOfFire();
            if (_fireTime >= model.GetTotalFireTime())
            {
                _fireTime = 0f;
                isReloading = true;
            }
        }
    }

    internal void WeaponVisibility(bool value)
    {
        gameObject.SetActive(value);
        MuzzleFlashStatus(false);
    }

    private void Update()
    {
        if (isReloading)
        {
            _reloadCounter += Time.deltaTime;
            ON_RELOADING?.Invoke(_reloadCounter, model.GetReloadTime());
            if (_reloadCounter >= model.GetReloadTime())
            {
                _reloadCounter = 0f;
                _lastFireTime = Mathf.Infinity;
                isReloading = false;
            }
        }
        else
        {
            _lastFireTime += Time.deltaTime;
        }
    }

    public void CreateWeapon(WeaponModel _model)
    {
        model = _model;

        ResetPosition();
        _lastFireTime = 0f;
        _lastFireTime = Mathf.Infinity;

        if (model.Config != null)
        {
            if (skin != null) Destroy(skin);
            skin = Instantiate(model.GetSkin(), transform);
            _startPos = skin.transform.localPosition;
            weaponSkinSettings = skin.GetComponent<WeaponSkinSettings>();
            MuzzleFlashStatus(false);
            UnloadWeapon();
            ClearBulletsList();
            StartCoroutine(CoroutineLoadWeapon(0f));
        }
    }

    private void ResetPosition()
    {
        transform.localScale = Vector3.one;
        transform.localPosition = Vector3.zero;
        transform.localEulerAngles = Vector3.zero;
    }

    IEnumerator CoroutineLoadWeapon(float _time)
    {
        yield return new WaitForSeconds(_time);
        LoadWeapon();
    }

    public void LoadWeapon()
    {
        if (!isLoaded)
        {
            if(weaponSkinSettings.GetBulletNode().Length > 0)
            {
                foreach (Transform _transform in weaponSkinSettings.GetBulletNode())
                {
                    if (model.GetBulletSkin())
                    {
                        var _bullet = Instantiate(model.GetBulletSkin());

                        BulletController _bulletController = _bullet.GetComponent<BulletController>();
                        _bulletController.Speed = 0f;

                        _bullets.Add(_bullet);

                        _bullet.transform.parent = _transform;
                        _bullet.transform.localPosition = Vector3.zero;
                        _bullet.transform.localEulerAngles = Vector3.zero;

                        float _scale = model.GetBulletLoadScale();
                        _bullet.transform.localScale = new Vector3(_scale, _scale, _scale);
                    }
                }
            }
            isLoaded = true;
        }
    }

    IEnumerator CoroutineShoot()
    {
        yield return new WaitForEndOfFrame();
        if (model.Config)
        {
            Bullet();
            Catridge();
            StartCoroutine(CoroutineMuzzleFlash());

            ClearBulletsList();
            UnloadWeapon();
            StartCoroutine(CoroutineLoadWeapon(model.GetRateOfFire() * 0.5f));

            PlayWeaponRecoilAnimation();
        }
        else Debug.LogError("WeaponController. No Weapon Config.");
    }

    void MuzzleFlashStatus(bool _status)
    {
        if (weaponSkinSettings.GetMuzzleFlashes().Length > 0)
        {
            foreach (GameObject _muzzleFlash in weaponSkinSettings.GetMuzzleFlashes())
            {
                _muzzleFlash.SetActive(_status);
                if (_status)
                {
                    _muzzleFlash.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(0f, 360f));
                    float _scale = Random.Range(model.GetMuzzleFlashScale()[0], model.GetMuzzleFlashScale()[1]);
                    _muzzleFlash.transform.localScale = new Vector3(_scale, _scale, _scale);
                }
            }
        }
    }

    IEnumerator CoroutineMuzzleFlash()
    {
        MuzzleFlashStatus(true);
        float _time = 0.06f;
        yield return new WaitForSeconds(_time);
        MuzzleFlashStatus(false);
    }

    void Bullet()
    {
        if (_bullets.Count > 0)
        {
            for (int i = 0; i < _bullets.Count; i++)
            {
                if (_bullets[i])
                {
                    _bullets[i].transform.parent = null;

                    float _accuracy = model.GetAccuracy();
                    Vector3 _accuracyVector = new Vector3(Random.Range(-_accuracy, _accuracy), Random.Range(-_accuracy, _accuracy), Random.Range(-15f, 15f));

                    if (model.GetIsMelee()) _bullets[i].transform.localEulerAngles = transform.eulerAngles;
                    else _bullets[i].transform.localEulerAngles = _bullets[i].transform.localEulerAngles + _accuracyVector;

                    float _scale = model.GetBulletScale();
                    _bullets[i].transform.localScale = new Vector3(_scale, _scale, _scale);

                    BulletController _bulletController = _bullets[i].GetComponent<BulletController>();
                    _bulletController.Damage = model.GetDamage();
                    _bulletController.Speed = model.GetBulletSpeed();
                    _bulletController.DestroyBullet(5f);
                }
            }
        }
    }

    void ClearBulletsList()
    {
        _bullets.Clear();
    }

    void UnloadWeapon()
    {
        if (_bullets.Count > 0)
        {
            foreach (GameObject _gameObject in _bullets)
            {
                Destroy(_gameObject);
            }
        }
        isLoaded = false;
    }

    void Catridge()
    {
        if (weaponSkinSettings.GetCatridgeNode().Length > 0)
        {
            foreach (Transform _transform in weaponSkinSettings.GetCatridgeNode())
            {
                SpawnCatridge(_transform);
            }
        }
    }

    void SpawnCatridge(Transform _transform)
    {
        if (model.Config.catridgeSkin)
        {
            GameObject _catridge = Instantiate(model.GetCatridgeSkin(), _transform.position, Quaternion.identity);
            float _scale = model.GetCatridgeCaseScale();
            _catridge.transform.localScale = new Vector3(_scale, _scale, _scale);
            _catridge.transform.localEulerAngles = _transform.localEulerAngles;

            Rigidbody rb = _catridge.AddComponent<Rigidbody>();
            rb.mass = model.GetCatridgeCaseMass();
            rb.drag = 0f;
            rb.angularDrag = 0f;
            rb.useGravity = true;

            float _force = model.GetCatridgeCaseForce();
            float[] _directionRight = model.GetCatridgeCaseDirectionRight();
            float[] _directionUp = model.GetCatridgeCaseDirectionUp();
            float[] _directionForward = model.GetCatridgeCaseDirectionForward();


            Vector3 force = transform.right * _force * Random.Range(_directionRight[0], _directionRight[1])
                + transform.up * _force * Random.Range(_directionUp[0], _directionUp[1])
                + transform.forward * _force * Random.Range(_directionForward[0], _directionForward[1]);

            rb.AddForceAtPosition(force, transform.position, ForceMode.Impulse);

            float[] _torque = model.GetCatridgeCaseTorque();
            rb.AddTorque(transform.up * _torque[Random.Range(0, _torque.Length)], ForceMode.Impulse);
            Destroy(_catridge, model.GetCatridgeCaseDestroyTime());
        }
    }

    private void PlayWeaponRecoilAnimation()
    {
        StartCoroutine(CoroutineWeaponRecoilBackAnimation());
    }

    IEnumerator CoroutineWeaponRecoilBackAnimation()
    {
        float _offsetTimeBackward = model.GetOffsetTimeBackward();
        float _currentTime = 0f;
        Vector3 _value = skin.transform.localPosition;
        Vector3 _endPos = _value -= Vector3.forward * model.GetOffsetDistance();

        while (_currentTime <= _offsetTimeBackward)
        {
            _currentTime += Time.deltaTime;
            skin.transform.localPosition = Vector3.Slerp(_startPos, _endPos, _currentTime / _offsetTimeBackward);
            yield return null;
        }
        StartCoroutine(CoroutineWeaponRecoilForwardAnimation());
    }

    IEnumerator CoroutineWeaponRecoilForwardAnimation()
    {
        float _offsetTimeForward = model.GetOffsetTimeForward();
        float _currentTime = 0f;
        Vector3 _value = skin.transform.localPosition;

        while (_currentTime <= _offsetTimeForward)
        {
            _currentTime += Time.deltaTime;
            skin.transform.localPosition = Vector3.Slerp(_value, _startPos, _currentTime / _offsetTimeForward);
            yield return null;
        }
    }
}