using System.Collections.Generic;
using UnityEngine;

public class UnitRouteController : MonoBehaviour
{
    Color rayColor = Color.yellow;
    private List<Vector3> actualPath = new List<Vector3>();
    Transform[] points;

    public void SetUnitLastPosition(Vector3 value)
    {
        GetPoints();
        SetPointFloor(value);
        AddPoint(value);
    }

    void SetPointFloor(Vector3 value)
    {
        Vector3 floorPointPosition = actualPath[actualPath.Count - 1];
        actualPath[actualPath.Count - 1] = new Vector3( floorPointPosition.x, value.y, floorPointPosition.z);
    }

    void GetPoints()
    {
        points = GetComponentsInChildren<Transform>();
        actualPath.Clear();
        foreach (Transform trans in points)
        {
            if (trans != this.transform)
            {
                actualPath.Add(trans.position);
            }
        }
    }

    void AddPoint(Vector3 value)
    {
        actualPath.Add(value);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        points = GetComponentsInChildren<Transform>();
        actualPath.Clear();

        foreach (Transform trans in points)
        {
            if (trans != this.transform)
            {
                actualPath.Add(trans.position);
            }
        }

        for (int i = 0; i < actualPath.Count; i++)
        {
            Vector3 position = actualPath[i];
            if (i > 0)
            {
                Vector3 previous = actualPath[i - 1];
                Gizmos.DrawLine(previous, position);
                Gizmos.DrawWireSphere(position, 0.05f);
            }
        }
    }

    public Vector3 GetFirstPointOnPath()
    {
        return actualPath[0];
    }

    public List<Vector3> GetActualPathPath()
    {
        return actualPath;
    }
}
