using UnityEngine;

public class LevelView : MonoBehaviour
{
    public LevelModel model = new LevelModel();
    public LevelSettings levelSettings;
    public BuildingController currentBuilding;

    public void SetModel(LevelConfig levelConfig)
    {
        model.Config = levelConfig;
        UpdateSkin();
    }

    void UpdateSkin()
    {
        if (model.Config != null)
        {
            levelSettings = Instantiate(model.GetSkin(), transform).GetComponent<LevelSettings>();
        }
    }
}