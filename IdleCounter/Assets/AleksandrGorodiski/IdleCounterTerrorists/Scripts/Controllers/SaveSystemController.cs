using UnityEngine;
using System;

public class SaveSystemController: GameElement
{
    public Action<SaveData> ON_LOAD_UNIT_DATA_FINISHED;

    public string unitsDataString;

    public void SaveUnitsData()
    {
        SaveData unitsData = new SaveData();
        unitsData.SetUnitsData(app.view.playersTransforms);
        unitsDataString = JsonUtility.ToJson(unitsData);
        PlayerPrefs.SetString("unitsData", unitsDataString);
    }

    public void LoadUnitsData()
    {
        unitsDataString = PlayerPrefs.GetString("unitsData");
        SaveData unitsData = JsonUtility.FromJson<SaveData>(unitsDataString);
        ON_LOAD_UNIT_DATA_FINISHED?.Invoke(unitsData);
    }
}