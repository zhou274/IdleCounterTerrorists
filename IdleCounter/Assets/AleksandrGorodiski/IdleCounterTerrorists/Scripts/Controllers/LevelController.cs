using System.Collections.Generic;
using UnityEngine;

public class LevelController : GameElement
{
    public Transform playerPlacesHolder;
    public Transform playerBirthPlacesHolder;

    private List<UnitPlaceController> _playerPlaces = new List<UnitPlaceController>();
    private List<Transform> _playerBirthPlaces = new List<Transform>();

    public LevelView levelView;

    public void GetEnemiesNeedToKillLevel()
    {
        int _buildingID = app.model.Building;
        for (int i = 0; i < levelView.levelSettings.buildingControllers.Count; i++)
        {
            int _value = levelView.levelSettings.buildingControllers[i].GetEnemiesNeedToKill();
            app.model.EnemiesNeedToKillLevel += _value;

            if (i < _buildingID)
            {
                app.model.EnemiesKilledLevel += levelView.levelSettings.buildingControllers[i].GetEnemiesNeedToKill();
            }
        }
    }

    public UnitRouteController SelectEnemyRouteConrtroller()
    {
        return levelView.currentBuilding.GetEnemyRoutes()[Random.Range(0, levelView.currentBuilding.GetEnemyRoutes().Count)];
    }

    public Transform SelectPlayerBirthPlace()
    {
        return _playerBirthPlaces[Random.Range(0, _playerBirthPlaces.Count)];
    }

    public UnitPlaceController SelectPlayerPlaceConrtroller()
    {
        UnitPlaceController placeController = null;
        foreach (UnitPlaceController _value in _playerPlaces)
        {
            if (!_value.IsTaken)
            {
                placeController = _value;
                break;
            }
        }
        return placeController;
    }

    public UnitPlaceController SelectEnemyPlaceConrtroller()
    {
        UnitPlaceController placeController = null;
        foreach (UnitPlaceController _value in levelView.currentBuilding.GetEnemyPlaces())
        {
            if (!_value.IsTaken)
            {
                placeController = _value;
                break;
            }
        }
        return placeController;
    }

    public void LoadLevelView()
    {
        GameObject gameObject = new GameObject();
        levelView = gameObject.AddComponent<LevelView>();
        levelView.SetModel(SelectLevelConfig());
    }

    public void LoadCurrentBuilding()
    {
        int _houseIDToLoad = app.model.Building;
        if (_houseIDToLoad >= levelView.levelSettings.buildingControllers.Count)
        {
            app.model.Building = 0;
            levelView.currentBuilding = levelView.levelSettings.buildingControllers[0];
        }
        else levelView.currentBuilding = levelView.levelSettings.buildingControllers[_houseIDToLoad];

        levelView.currentBuilding.CollectEnemyPlaces();
    }

    LevelConfig SelectLevelConfig()
    {
        int _levelIDToLoad = app.model.Level;
        if (_levelIDToLoad >= app.globalConfigs.levelsConfigs.Count)
        {
            _levelIDToLoad = 0;
            app.model.Level = _levelIDToLoad;
            return app.globalConfigs.levelsConfigs[_levelIDToLoad];
        }
        else return app.globalConfigs.levelsConfigs[_levelIDToLoad];
    }

    public void GetPlayerPlaces()
    {
        CollectPlayerPlaces();
        CollectPlayerBirthPlaces();
    }

    void CollectPlayerPlaces()
    {
        _playerPlaces.Clear();
        foreach (Transform _trans in playerPlacesHolder)
        {
            _playerPlaces.Add(_trans.gameObject.GetComponent<UnitPlaceController>());
        }
    }

    void CollectPlayerBirthPlaces()
    {
        _playerBirthPlaces.Clear();
        foreach (Transform _trans in playerBirthPlacesHolder)
        {
            _playerBirthPlaces.Add(_trans);
        }
    }
}