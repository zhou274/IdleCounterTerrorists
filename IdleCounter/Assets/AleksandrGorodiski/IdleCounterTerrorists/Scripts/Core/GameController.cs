using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using TTSDK.UNBridgeLib.LitJson;
using TTSDK;
using StarkSDKSpace;
using System.Collections.Generic;

public class GameController : GameElement
{
    public BalanceController balanceController;
    public PopupWindowController popupWindowController;
    public LevelController levelController;
    public SaveSystemController saveSystem;
    public CameraController cameraController;
    public SquadController squadController;

    public event Action ON_BUILDING_CHANGED;
    public event Action ON_BUILDING_CLEARED;
    public event Action ON_LEVEL_CLEARED;
    public string clickid;
    private StarkAdManager starkAdManager;
    private GameModel _gameModel
    {
        get => app.model;
    }

    private void Awake()
    {
        
        saveSystem.ON_LOAD_UNIT_DATA_FINISHED += OnLoadUnitsDataFinished;

        levelController.LoadLevelView();
        levelController.LoadCurrentBuilding();

        levelController.GetPlayerPlaces();
        levelController.GetEnemiesNeedToKillLevel();

        balanceController.LoadBalance();
    }

    private void Start()
    {
        cameraController.SetPosition(levelController.levelView.currentBuilding.GetSquadPosition());
        squadController.SetTargetPosition(levelController.levelView.currentBuilding.GetSquadPosition());

        CheckIfLiftUI();

        app.view.gameHud.shopButton.onClick.AddListener(OpenPopupShop);

        UpdateLevelIcon();
        UpdateProgressBars();

        saveSystem.LoadUnitsData();

        OpenPopupShop();
        
        if (!IsTutorial) OpenPopupStartLevel(0f);
    }

    void OnLoadUnitsDataFinished(SaveData data)
    {
        int quantity = (null == data) ? 0 : data.unitsDatas.Count;
        if (quantity > 0)
        {
            for (int i = 0; i < data.unitsDatas.Count; i++)
            {
                UnitConfig unit = FindUnitConfigs(data.unitsDatas[i]);
                if (unit != null) CreatePlayer(unit);
                else Debug.LogWarning("GameController. Can find UnitConfig");
            }
        }
    }

    private UnitConfig FindUnitConfigs(UnitsData unitsData)
    {
        UnitConfig configOut = null;
        foreach (UnitConfig config in app.globalConfigs.playerConfigs)
        {
            if (config.ID == unitsData.unitID)
            {
                configOut = config;
            }
        }
        return configOut;
    }

    public void NextBuilding()
    {
        _gameModel.Building++;

        levelController.LoadCurrentBuilding();
        popupWindowController.HideActiveWindow();

        cameraController.SetTargetPosition(levelController.levelView.currentBuilding.GetSquadPosition());
        squadController.SetTargetPosition(levelController.levelView.currentBuilding.GetSquadPosition());

        _gameModel.EnemiesKilledBuilding = 0;
        _gameModel.EnemiesSpawned = 0;

        UpdateProgressBars();

        OpenPopupStartLevel(_gameModel.GetDelayOpenPopupStartLevel());

        ON_BUILDING_CHANGED?.Invoke();
    }

    public void NextLevel()
    {
        _gameModel.Building = 0;
        _gameModel.Level++;

        saveSystem.SaveUnitsData();

        ReloadGame();
    }

    public bool _isLevelStarted;
    public bool IsLevelStarted
    {
        get => _isLevelStarted;
        set
        {
            if (value != _isLevelStarted)
            {
                _isLevelStarted = value;
            }
        }
    }
    public void AddMoney()
    {
        ShowVideoAd("192if3b93qo6991ed0",
            (bol) => {
                if (bol)
                {

                    balanceController.AddCash(100);


                    clickid = "";
                    getClickid();
                    apiSend("game_addiction", clickid);
                    apiSend("lt_roi", clickid);


                }
                else
                {
                    StarkSDKSpace.AndroidUIManager.ShowToast("观看完整视频才能获取奖励哦！");
                }
            },
            (it, str) => {
                Debug.LogError("Error->" + str);
                //AndroidUIManager.ShowToast("广告加载异常，请重新看广告！");
            });
        
    }
    public void OnEnemyDied(int reward, Vector3 unitPosition)
    {
        _gameModel.EnemiesKilledBuilding++;
        _gameModel.EnemiesKilledLevel++;

        balanceController.AddCash((long)reward);
        popupWindowController.NotificationCash(reward, unitPosition);

        if (CheckIfLevelCleared())
        {
            ON_LEVEL_CLEARED?.Invoke();

            Debug.Log("GameController. Level Cleared");

            IsLevelStarted = false;

            OpenPopupLevelCleared(app.model.GetDelayOpenPopupLevelComplete());

        }
        else if (CheckIfBuildingCleared())
        {
            ON_BUILDING_CLEARED?.Invoke();

            Debug.Log("GameController. Building Cleared");

            IsLevelStarted = false;

            OpenPopupBuildingCleared(app.model.GetDelayOpenPopupLevelComplete());
        }
        UpdateProgressBars();
    }


    bool CheckIfBuildingCleared()
    {
        int _enemiesLeftToKillBuilding = levelController.levelView.currentBuilding.GetEnemiesNeedToKill() - _gameModel.EnemiesKilledBuilding;
        if (_enemiesLeftToKillBuilding <= 0) return true;
        return false;
    }

    bool CheckIfLevelCleared()
    {
        int _enemiesNeedToKillLevel = _gameModel.EnemiesNeedToKillLevel - _gameModel.EnemiesKilledLevel;
        if (_enemiesNeedToKillLevel <= 0) return true;
        return false;
    }

    private void UpdateLevelIcon()
    {
        app.view.UpdateLevelIcon(levelController.levelView.model.GetIcon());
    }

    private void UpdateProgressBars()
    {
        int _enemiesToKillBuilding = levelController.levelView.currentBuilding.GetEnemiesNeedToKill();
        int _enemiesKilledBuilding = _gameModel.EnemiesKilledBuilding;
        int _enemiesLeftToKillBuilding = _enemiesToKillBuilding - _enemiesKilledBuilding;
        float _valueBuilding = ((float)_enemiesKilledBuilding / (float)_enemiesToKillBuilding);
        app.view.UpdateBuildingProgressBar(_valueBuilding);
        app.view.UpdateBuildingProgressBarText(_enemiesLeftToKillBuilding);

        int _enemiesToKillLevel = _gameModel.EnemiesNeedToKillLevel;
        int _enemiesKilledLevel = _gameModel.EnemiesKilledLevel;
        int _enemiesLeftToKillLevel = _enemiesToKillLevel - _enemiesKilledLevel;
        float _valueLevel = ((float)_enemiesKilledLevel / (float)_enemiesToKillLevel);
        app.view.UpdateLevelProgressBar(_valueLevel);
        app.view.UpdateLevelProgressBarText(levelController.levelView.model.GetID(), _enemiesLeftToKillLevel);
    }

    public void TryToHire(UnitModel unit)
    {
        if (app.controller.IsTutorial)
        {
            app.controller.IsTutorial = false;
            app.controller.OpenPopupStartLevel(app.model.GetDelayOpenPopupStartLevel());
        }

        if (balanceController.CanPurchase(unit.GetPrice()))
        {
            UnitPlaceController placeController = app.controller.levelController.SelectPlayerPlaceConrtroller();
            if (placeController)
            {
                CreatePlayer(unit.Config);
                balanceController.MinusCash(unit.GetPrice());
            }
            else
            {
                popupWindowController.NotificationSquadFull();
            }
        }
        else
        {
            popupWindowController.NotificationNotEnoughCash();
        } 
    }

    void CreatePlayer(UnitConfig config)
    {
        UnitPlaceController placeController = app.controller.levelController.SelectPlayerPlaceConrtroller();

        if (placeController)
        {
            UnitModel model = new UnitModel();
            model.Config = config;

            PlayerView view = Instantiate(app.view.GetPlayerBase()).GetComponent<PlayerView>();
            Vector3 birthPlace = app.controller.levelController.SelectPlayerBirthPlace().position;

            view.ClearPath();
            view.AddPointToPath(birthPlace);
            view.AddPointToPath(placeController.GetPlacePosition());

            view.SetPosition(birthPlace);

            view.SetPlaceController(placeController);
            view.SetModel(model);
        }
        else
        {
            popupWindowController.NotificationSquadFull();
        }
    }

    void CreateEnemy()
    {
        CreateEnemy(app.globalConfigs.enemyConfigs[Random.Range(0, app.globalConfigs.enemyConfigs.Count)]);
    }

    void CreateEnemy(UnitConfig config)
    {
        UnitPlaceController placeController = app.controller.levelController.SelectEnemyPlaceConrtroller();

        if (placeController)
        {
            UnitModel model = new UnitModel();
            model.Config = config;

            EnemyView view = Instantiate(app.view.GetEnemyBase()).GetComponent<EnemyView>();

            view.ClearPath();
            view.AddPointToPath(placeController.GetPlacePosition());
            view.AddPointToPath(placeController.GetPlacePosition());

            view.SetPosition(placeController.GetPlacePosition());

            view.SetPlaceController(placeController);
            view.SetModel(model);

            _gameModel.EnemiesSpawned++;
        }
    }

    public void OpenPopupShop()
    {
        popupWindowController.ShowWindow("popup_shop");
    }

    public void OpenPopupBuildingCleared(float delay)
    {
        StartCoroutine(CoroutineOpenPopupBuildingCleared(delay));
    }

    IEnumerator CoroutineOpenPopupBuildingCleared(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupWindowController.ShowWindow("popup_building_cleared");
    }

    public void OpenPopupLevelCleared(float delay)
    {
        StartCoroutine(CoroutineOpenPopupLevelCleared(delay));
    }

    IEnumerator CoroutineOpenPopupLevelCleared(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupWindowController.ShowWindow("popup_level_complete");
    }

    public void OpenPopupStartLevel(float delay)
    {
        StartCoroutine(CoroutineOpenPopupStartLevel(delay));
    }

    IEnumerator CoroutineOpenPopupStartLevel(float delay)
    {
        yield return new WaitForSeconds(delay);
        popupWindowController.ShowWindow("popup_start_level");
    }

    public void OnAddCashButton()
    {
        balanceController.AddCash(1000);
    }

    private bool _isTutorial;
    public bool IsTutorial
    {
        get => _isTutorial = (PlayerPrefs.GetInt("is_tutorial", 1) == 1);
        set
        {
            if (value != _isTutorial)
            {
                _isTutorial = value;
                Debug.Log("GameController. Tutorial Finished");
                PlayerPrefs.SetInt("is_tutorial", 0);
            }
        }
    }

    private float _timeCounterCash;
    private float _timeCounterReset;
    private float _timeCounterEnemies = Mathf.Infinity;

    bool CanSpawnEnemy()
    {
        if (_gameModel.EnemiesSpawned < levelController.levelView.currentBuilding.GetEnemiesNeedToKill()) return true;
        else return false;
    }

    private void Update()
    {
        _timeCounterEnemies += Time.deltaTime;
        if (_timeCounterEnemies >= levelController.levelView.currentBuilding.GetSpawnEnemiesRate())
        {
            if (app.gameSettings.spawnEnemies && CanSpawnEnemy())
            {
                _timeCounterEnemies = 0f;
                CreateEnemy();
            } 
        }

        if (app.gameSettings.isCheating)
        {
#if UNITY_EDITOR
            if (Input.GetMouseButton(0))
#else
            if (Input.touchCount == 3)
#endif
            {
                _timeCounterCash += Time.deltaTime;
                if (_timeCounterCash >= 3f)
                {
                    OnAddCashButton();
                    _timeCounterCash = 0f;
                }
            }
            else if (Input.touchCount == 4)
            {
                _timeCounterReset += Time.deltaTime;
                if (_timeCounterReset >= 3f)
                {
                    ResetGame();
                    _timeCounterReset = 0f;
                }
            }
            else
            {
                _timeCounterCash = 0f;
                _timeCounterReset = 0f;
            } 
        }
    }

    public void ResetGame()
    {
        PlayerPrefs.DeleteAll();
        ReloadGame();
    }

    void ReloadGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    void CheckIfLiftUI()
    {
        float screenRatio = (1f * Screen.height) / (1f * Screen.width);

        if (screenRatio < 1.8f && screenRatio > 1.75f)
        {
            LiftTopScreenElements();
        }
        else if (screenRatio < 2.2f && screenRatio > 2f)
        {

        }
        else
        {
            LiftTopScreenElements();
        }
    }

    public void LiftTopScreenElements()
    {
        app.view.gameHud.topScreenElements.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }

    private void OnApplicationFocus(bool focus)
    {
        if (!focus)
        {
            saveSystem.SaveUnitsData();
        }
    }
    public void getClickid()
    {
        var launchOpt = StarkSDK.API.GetLaunchOptionsSync();
        if (launchOpt.Query != null)
        {
            foreach (KeyValuePair<string, string> kv in launchOpt.Query)
                if (kv.Value != null)
                {
                    Debug.Log(kv.Key + "<-参数-> " + kv.Value);
                    if (kv.Key.ToString() == "clickid")
                    {
                        clickid = kv.Value.ToString();
                    }
                }
                else
                {
                    Debug.Log(kv.Key + "<-参数-> " + "null ");
                }
        }
    }

    public void apiSend(string eventname, string clickid)
    {
        TTRequest.InnerOptions options = new TTRequest.InnerOptions();
        options.Header["content-type"] = "application/json";
        options.Method = "POST";

        JsonData data1 = new JsonData();

        data1["event_type"] = eventname;
        data1["context"] = new JsonData();
        data1["context"]["ad"] = new JsonData();
        data1["context"]["ad"]["callback"] = clickid;

        Debug.Log("<-data1-> " + data1.ToJson());

        options.Data = data1.ToJson();

        TT.Request("https://analytics.oceanengine.com/api/v2/conversion", options,
           response => { Debug.Log(response); },
           response => { Debug.Log(response); });
    }


    /// <summary>
    /// </summary>
    /// <param name="adId"></param>
    /// <param name="closeCallBack"></param>
    /// <param name="errorCallBack"></param>
    public void ShowVideoAd(string adId, System.Action<bool> closeCallBack, System.Action<int, string> errorCallBack)
    {
        starkAdManager = StarkSDK.API.GetStarkAdManager();
        if (starkAdManager != null)
        {
            starkAdManager.ShowVideoAdWithId(adId, closeCallBack, errorCallBack);
        }
    }
}