using UnityEngine;

public class BulletController : MonoBehaviour
{
    public void DamageCount(float value)
    {
        Damage -= value;
        if (Damage == 0) DestroyBullet(0f);
    }

    public void DestroyBullet(float _value)
    {
        Destroy(gameObject, _value);
    }

    void FixedUpdate()
    {
        transform.position += transform.forward * Time.fixedDeltaTime * Speed;
    }

    [SerializeField]
    private float _damage;
    public float Damage
    {
        get => _damage;
        set
        {
            _damage = value;
        }
    }

    [SerializeField]
    private float _speed;
    public float Speed
    {
        get => _speed;
        set
        {
            _speed = value;
        }
    }
}