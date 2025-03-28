using UnityEngine;

public class BulletMover : MonoBehaviour
{
    private float bulletSpeed;

    void Start()
    {
        bulletSpeed = 30f;
    }

    void Update()
    {
        transform.position += transform.forward * Time.deltaTime * bulletSpeed;
    }
}
