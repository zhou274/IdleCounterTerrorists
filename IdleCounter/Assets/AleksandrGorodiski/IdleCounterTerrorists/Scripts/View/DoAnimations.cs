using UnityEngine;
using DG.Tweening;
using System;

public class DoAnimations : MonoBehaviour
{
    public event Action ON_SCALE_IN_ANIMATION_FINISHED;
    public event Action ON_SCALE_OUT_ANIMATION_FINISHED;

    private float _posYOnStart = 15f;
    [SerializeField]
    private Vector3 _scaleStart = Vector3.one;
    [SerializeField]
    private Vector3 _scaleEnd = new Vector3(1f, 0.4f, 1f);
    private float _speedTransform = 0.2f;
    [SerializeField]
    private float _speedScaleIn = 0.1f;
    [SerializeField]
    private float _speedScaleOut = 3f;

    private void Awake()
    {
        _posYOnStart = 15f;
        _speedTransform = 0.2f;
    }

    public void DoScaleInAnimation()
    {
        transform.DOKill();
        ScaleAnimationIn(_scaleStart, _scaleEnd);
    }

    public void DoScaleOutAnimation()
    {
        transform.DOKill();
        ScaleAnimationOut(_scaleEnd, _scaleStart);
    }

    void ScaleAnimationIn(Vector3 _start, Vector3 _end)
    {
        transform.localScale = _start;
        transform.DOScale(_end, _speedScaleIn).SetEase(Ease.OutQuad).OnComplete(() => { ON_SCALE_IN_ANIMATION_FINISHED?.Invoke(); });
    }

    void ScaleAnimationOut(Vector3 _start, Vector3 _end)
    {
        float _speed = _speedScaleIn * _speedScaleOut;
        transform.localScale = _start;
        transform.DOScale(_end, _speed).SetEase(Ease.OutBack).OnComplete(() => { ON_SCALE_OUT_ANIMATION_FINISHED?.Invoke(); });
    }

    Vector3 GetStartPos()
    {
        return transform.position;
    }

    void SetStartPosition()
    {
        transform.position = new Vector3(GetStartPos().x, _posYOnStart, GetStartPos().z);
    }

    void TransformAnimation()
    {
        SetStartPosition();
        Vector3 _endPosition = new Vector3(GetStartPos().x, 0f, GetStartPos().z);
        transform.DOMove(_endPosition, _speedTransform).SetEase(Ease.InSine).OnComplete(() => { });
    }
}