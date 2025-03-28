using System.Collections;
using UnityEngine;
using DG.Tweening;

public class UnitLandingAnimations : MonoBehaviour
{
    public Vector3 _positionStart = new Vector3(0f, 15f, 0f);
    public Vector3 _scaleStart = Vector3.one;
    public Vector3 _scaleEnd = new Vector3(1.6f, 0.1f, 1.6f);

    public float landingDuration = 1;
    public float _delay = 0.03f;
    public float _speedScale = 0.07f;
    public float _speedScaleFactor = 13f;
    public float amplitude = 1f;

    private void Start()
    {
        SetStartPosition();
        DoAnimations();
    }

    void SetStartPosition()
    {
        transform.position = new Vector3(transform.position.x, _positionStart.y, transform.position.z);
    }

    void DoAnimations()
    {
        TransformAnimation();
    }

    IEnumerator CoroutineScaleAnimation()
    {
        transform.localScale = _scaleStart;
        yield return new WaitForSeconds(landingDuration - _delay);
        ScaleAnimationIn(_scaleStart, _scaleEnd);
    }

    void ScaleAnimationIn(Vector3 _start, Vector3 _end)
    {
        transform.localScale = _start;
        transform.DOScale(_end, _speedScale).SetEase(Ease.OutQuad).OnComplete(() => { ScaleAnimationOut(_end, _start); });

    }

    void ScaleAnimationOut(Vector3 _start, Vector3 _end)
    {
        float _speed = _speedScale * _speedScaleFactor;
        transform.localScale = _start;
        transform.DOScale(_end, _speed).SetEase(Ease.OutElastic).OnComplete(() => {});
    }

    void TransformAnimation()
    {
        transform.DOMoveY(0f, landingDuration).SetEase(Ease.OutBounce). OnComplete(() => {});
    }
}