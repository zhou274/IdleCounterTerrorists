using UnityEngine;

public class EnemyView : UnitView
{
    public override void OnDied()
    {
        base.OnDied();
        app.controller.OnEnemyDied(model.GetReward(), transform.position + Vector3.up * 2f);
    }
}
