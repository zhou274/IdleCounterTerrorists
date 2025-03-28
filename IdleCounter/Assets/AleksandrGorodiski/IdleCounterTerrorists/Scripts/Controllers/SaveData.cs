using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<UnitsData> unitsDatas;

    public void SetUnitsData(List<Transform> players)
    {
        unitsDatas = new List<UnitsData>();
        for (int i = 0; i < players.Count; i++)
        {
            UnitView view = players[i].GetComponent<UnitView>();

            var unitsData = new UnitsData
            {
                unitID = view.model.GetID(),
                health = view.model.Health
            };
            unitsDatas.Add(unitsData);
        }
    }
}

[Serializable]
public class UnitsData
{
    public string unitID;
    public float health;
}
