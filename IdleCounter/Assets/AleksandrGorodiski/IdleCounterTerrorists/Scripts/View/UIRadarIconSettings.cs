using TMPro;
using UnityEngine;

public class UIRadarIconSettings : MonoBehaviour
{
    private Transform _target;
    public RectTransform rectTransform;
    public GameObject globalVisNode;
    public GameObject spritesVisNode;
    public GameObject pointer;
    public TextMeshProUGUI iconText;

    private float UIIconOffset;
    float _minX, _maxX, _minY, _maxY;

    Camera _camera;

    private void OnEnable()
    {
        _camera = Camera.main;

        UIIconOffset = 1f;

        float _addX = 15f;
        float _addY = 160f;

        _minX = (rectTransform.sizeDelta.x / 2f) + _addX;
        _maxX = (Screen.width - (rectTransform.sizeDelta.x / 2f)) - _addX;

        _minY = (rectTransform.sizeDelta.y / 2f) + _addY;
        _maxY = (Screen.height - (rectTransform.sizeDelta.y / 2f)) - _addY;
    }

    void Update()
    {
        Follow();
    }

    void Follow()
    {
        Vector2 _targetPos = _camera.WorldToScreenPoint(Target.position + new Vector3(0f, UIIconOffset, 0f));
        if (IsBetweenInclusive(_targetPos.x, _minX, _maxX) && IsBetweenInclusive(_targetPos.y, _minY, _maxY))
        {
            if (spritesVisNode.activeInHierarchy) spritesVisNode.SetActive(false);
        }
        else
        {
            if (!spritesVisNode.activeInHierarchy) spritesVisNode.SetActive(true);
        }

        Vector2 position;
        Vector2 screenCenter = new Vector2(Screen.width*.5f, Screen.height*.5f);

        if (!LineIntersection.FindIntersection(screenCenter, _targetPos, 
            Screen.width, 0, Screen.width, Screen.height, true, out position) &&
            !LineIntersection.FindIntersection(screenCenter, _targetPos, 
            0, 0, 0, Screen.height, true, out position) &&
            !LineIntersection.FindIntersection(screenCenter, _targetPos,
            0, 0, Screen.width, 0, true, out position) &&
            !LineIntersection.FindIntersection(screenCenter, _targetPos, 
            0, Screen.height, Screen.width, Screen.height, true, out position))
        {
            position = _targetPos;
        }

        position.x = Mathf.Clamp(position.x, _minX, _maxX);
        position.y = Mathf.Clamp(position.y, _minY, _maxY);

        transform.position = position;

        Vector2 _diff = new Vector2(_targetPos.x, _targetPos.y) - position;

        _diff.Normalize();
        float _rotZ = Mathf.Atan2(_diff.y, _diff.x) * Mathf.Rad2Deg;
        pointer.transform.rotation = Quaternion.Euler(0f, 0f, _rotZ - 90f);
    }

    public bool IsBetweenInclusive(float value, float bound1, float bound2)
    {
        return value >= bound1 && value <= bound2;
    }

    public TextMeshProUGUI IconText
    {
        get => iconText;
        set
        {
            iconText = value;
        }
    }

    public Transform Target
    {
        get => _target;
        set
        {
            if (_target == value) return;
            _target = value;
        }
    }

    public void GlobalVisibility(bool value)
    {
        globalVisNode.SetActive(value);
    }
}