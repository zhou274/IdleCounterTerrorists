using UnityEngine;

public class CameraController : MonoBehaviour
{
    Vector3 _targetPos;
    float speedWalk = 10f;

    private void Start()
    {
        _targetPos = transform.position;
    }

    public void SetPosition(Vector3 value)
    {
        transform.position = value;
    }

    public void SetTargetPosition(Vector3 value)
    {
        _targetPos = value;
    }

    private void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _targetPos, Time.deltaTime * speedWalk);
    }
}