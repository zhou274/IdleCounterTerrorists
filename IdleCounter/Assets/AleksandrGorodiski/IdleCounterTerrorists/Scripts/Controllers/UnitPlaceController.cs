using UnityEngine;

public class UnitPlaceController : MonoBehaviour
{
    public bool isEnemyPlace;
    public GameObject placeMarker;

    public Vector3 GetPlacePosition()
    {
        return transform.position;
    }

    private void Start()
    {
        if(isEnemyPlace) PlaceVisibility(false);
    }

    [SerializeField]
    private bool _isTaken;
    public bool IsTaken
    {
        get => _isTaken;
        set
        {
            if (_isTaken == value) return;
            _isTaken = value;
            if (isEnemyPlace) PlaceVisibility(false);
            else PlaceVisibility(!_isTaken);
        }
    }

    void PlaceVisibility(bool value)
    {
        placeMarker.SetActive(value);
    }
}
