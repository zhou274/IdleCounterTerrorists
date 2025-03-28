using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

public class PlayerView : UnitView
{
    public NavMeshAgent navMeshAgent;
    Vector3 _destination;

    protected override void MoveUnit()
    {
        if (Vector3.Distance(_destination, path[CurrentWayPointID]) > 0.05)
        {
            _destination = path[CurrentWayPointID];
            navMeshAgent.SetDestination(_destination);
        }
    }

    protected override void StopUnit()
    {
        navMeshAgent.Stop();
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.CompareTag("BulletEnemy"))
        {
            OnBulletHit(collider);
        }
    }

    public override Transform GetClosestUnit(List<Transform> unitTransforms)
    {
        return base.GetClosestUnit(app.view.enemiesTransforms);
    }

    public override void AddUnitTransform()
    {
        app.view.AddPlayerTransform(transform);
    }

    public override void RemoveUnitTransform()
    {
        app.view.RemovePlayerTransform(transform);
    }

    protected override void GetNavMeshAgent()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = speedWalk;
    }

    public override void OnBuildingCleared()
    {
        if (!model.GetIsDead())
        {
            SetUnitRotation(180f);
            unitSkinSettings.unitAnimation.PlayDanceAnimation();
            weaponController.WeaponVisibility(false);
        }
    }

    public override void OnBuildingChanged()
    {
        if (!model.GetIsDead())
        {
            PlayAnimationOnStart();
            weaponController.WeaponVisibility(true);

            ClearPath();
            AddPointToPath(transform.position);
            AddPointToPath(placeController.GetPlacePosition());

            CurrentWayPointID--;
            isArrived = false;
        }
    }
}