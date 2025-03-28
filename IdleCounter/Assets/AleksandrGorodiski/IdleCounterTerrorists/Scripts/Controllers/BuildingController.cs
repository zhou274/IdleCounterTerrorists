using System.Collections.Generic;
using UnityEngine;

public class BuildingController : MonoBehaviour
{
    public Transform enemyRoutesHolder;
    public Transform enemyPlacesHolder;
    public Transform squadPosition;

    private List<UnitRouteController> _enemyRoutes = new List<UnitRouteController>();
    private List<UnitPlaceController> _enemyPlaces = new List<UnitPlaceController>();

    public int enemiesNeedToKill = 3;
    public float spawnEnemiesRate = 1f;

    public Vector3 GetSquadPosition()
    {
        return squadPosition.position;
    }

    public void CollectEnemyRoutes()
    {
        _enemyRoutes.Clear();
        foreach (Transform _trans in enemyRoutesHolder)
        {
            _enemyRoutes.Add(_trans.gameObject.GetComponent<UnitRouteController>());
        }
    }

    public void CollectEnemyPlaces()
    {
        _enemyPlaces.Clear();
        foreach (Transform _trans in enemyPlacesHolder)
        {
            _enemyPlaces.Add(_trans.gameObject.GetComponent<UnitPlaceController>());
        }
    }

    public List<UnitRouteController> GetEnemyRoutes()
    {
        return _enemyRoutes;
    }

    public List<UnitPlaceController> GetEnemyPlaces()
    {
        return _enemyPlaces;
    }

    public int GetEnemiesNeedToKill()
    {
        return enemiesNeedToKill;
    }

    public float GetSpawnEnemiesRate()
    {
        return spawnEnemiesRate;
    }
}