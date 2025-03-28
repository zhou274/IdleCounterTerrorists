using System.Collections;
using UnityEngine;

public class TreeAnimation : MonoBehaviour
{
    float index;

    public float[] amplitudes = { 1f, 2f, 3f };
    float _amplitudeX, _amplitudeY, _amplitudeZ;

    public float[] rotSpeeds = { 1f, 2f, 3f };
    float _rotSpeedX, _rotSpeedY, _rotSpeedZ;

    private Vector3 _startRotation;
    private float scaleInDuration = 0.5f;

    private void Start()
    {
        _startRotation = transform.localEulerAngles;
    }

    void OnEnable()
    {
        _amplitudeX = amplitudes[Random.Range(0, amplitudes.Length)];
        _amplitudeY = amplitudes[Random.Range(0, amplitudes.Length)];
        _amplitudeZ = amplitudes[Random.Range(0, amplitudes.Length)];

        _rotSpeedX = rotSpeeds[Random.Range(0, rotSpeeds.Length)];
        _rotSpeedY = rotSpeeds[Random.Range(0, rotSpeeds.Length)];
        _rotSpeedZ = rotSpeeds[Random.Range(0, rotSpeeds.Length)];
    }

    void SetScale(Vector3 _scale)
    {
        transform.localScale = _scale;
    }

    public void StartScaleUp(Vector3 _endScale, float _waitTime)
    {
        StartCoroutine(ScaleUp(_endScale, _waitTime));
    }

    IEnumerator ScaleUp(Vector3 _endScale, float _waitTime)
    {
        Vector3 _startScale = transform.localScale;
        Vector3 _scale = _startScale;
        yield return new WaitForSeconds(_waitTime);

        for (float timer = 0; timer < scaleInDuration; timer += Time.deltaTime)
        {
            float progress = timer / scaleInDuration;
            _scale = Vector3.Lerp(_scale, _endScale, progress);
            SetScale(_scale);
            yield return null;
        }
    }

    void Update()
    {
        index += Time.deltaTime;

        float xRot = _amplitudeX * Mathf.Cos(_rotSpeedX * index);
        float yRot = _amplitudeY * Mathf.Cos(_rotSpeedY * index);
        float zRot = _amplitudeZ * Mathf.Cos(_rotSpeedZ * index);

        Vector3 _newRotation = new Vector3(xRot / 2f, yRot, zRot);
        transform.localEulerAngles = _newRotation + _startRotation;
    }
}