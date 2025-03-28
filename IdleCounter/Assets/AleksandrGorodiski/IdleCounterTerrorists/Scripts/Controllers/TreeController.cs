using UnityEngine;

public class TreeController : MonoBehaviour
{
    public GameObject marker;
    public GameObject[] treePrefabs;
    public float[] scale;

    void Start()
    {
        HideMarker();
        SetTree();
    }

    void HideMarker()
    {
        if (marker) marker.SetActive(false);
    }

    void SetTree()
    {
        GameObject _item = Instantiate(GetTreePrefab(), transform);
        _item.transform.localPosition = GetPosition();
        _item.transform.localScale = GetScale();
        _item.transform.localEulerAngles = GetRotation();
    }

    public virtual GameObject GetTreePrefab()
    {
        return treePrefabs[Random.Range(0, treePrefabs.Length)];
    }

    Vector3 GetPosition()
    {
        return Vector3.zero;
    }

    Vector3 GetScale()
    {
        float _scale = Random.Range(scale[0], scale[1]);
        return new Vector3(_scale, _scale, _scale);
    }

    Vector3 GetRotation()
    {
        float _angleX = 0f;
        float _angleY = Random.Range(0, 360f);
        float _angleZ = 0f;

        return Vector3.zero;
    }
}