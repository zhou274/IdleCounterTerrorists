using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameView : MonoBehaviour
{
    public GameHud gameHud;

    [SerializeField]
    private GameObject _playerBase;
    [SerializeField]
    private GameObject _enemyBase;
    [HideInInspector]
    public List<Transform> playersTransforms;
    [HideInInspector]
    public List<Transform> enemiesTransforms;

    IEnumerator CoroutineCountTo(Image _fillImage, float _prevValue, float _newValue)
    {
        float time = 0;
        float duration = 0.1f;
        float startValue = _prevValue;
        while (time < duration)
        {
            float t = time / duration;
            t = 1 - Mathf.Pow(1-t, 3);
            _fillImage.fillAmount = Mathf.Lerp(startValue, _newValue, t);
            time += Time.deltaTime;
            yield return null;
        }
        _fillImage.fillAmount = _newValue;
    }

    public void UpdateBuildingProgressBarText(int enemiesLeftToKill)
    {
        string _text = "该地点已清除";
        if (enemiesLeftToKill > 0)
        {
            string _enemies = "<color=red>" + enemiesLeftToKill.ToString() + "</color>";
            _text = "建筑物中剩余的恐怖分子: " + _enemies;
        }
        gameHud.buildingText.text = _text;
    }

    public void UpdateLevelProgressBarText(string levelName, int enemiesLeftToKill)
    {
        string _text = levelName + " Cleared";
        if (enemiesLeftToKill > 0)
        {
            string _enemies = "<color=red>" + enemiesLeftToKill.ToString() + "</color>";
            _text = "恐怖分子在 " + levelName + ": " + _enemies;
        }
        gameHud.levelText.text = _text;
    }

    public void UpdateBuildingIcon(Sprite icon)
    {
        gameHud.buildingIcon.sprite = icon;
    }

    public void UpdateLevelIcon(Sprite icon)
    {
        gameHud.levelIcon.sprite = icon;
    }

    public void UpdateBuildingProgressBar(float value)
    {
        StartCoroutine(CoroutineCountTo(gameHud.buildingProgressBar, gameHud.buildingProgressBar.fillAmount, value));
    }

    public void UpdateLevelProgressBar(float value)
    {
        StartCoroutine(CoroutineCountTo(gameHud.levelProgressBar, gameHud.levelProgressBar.fillAmount, value));
    }

    public GameObject GetPlayerBase()
    {
        return _playerBase;
    }
    public GameObject GetEnemyBase()
    {
        return _enemyBase;
    }

    public void AddEnemyTransform(Transform _enemy)
    {
        if (!enemiesTransforms.Contains(_enemy))
        {
            enemiesTransforms.Add(_enemy);
        } 
    }

    public void RemoveEnemyTransform(Transform _enemy)
    {
        if (enemiesTransforms.Contains(_enemy))
        {
            enemiesTransforms.Remove(_enemy);
        }
    }

    public void AddPlayerTransform(Transform _player)
    {
        if (!playersTransforms.Contains(_player))
        {
            playersTransforms.Add(_player);
        }
    }

    public void RemovePlayerTransform(Transform _player)
    {
        if (playersTransforms.Contains(_player))
        {
            playersTransforms.Remove(_player);
        }
    }

    public void RemoveAndDestroyPlayers()
    {
        foreach (Transform player in playersTransforms)
        {
            player.GetComponent<PlayerView>().DestroyUnit(0f);
        }
    }
}