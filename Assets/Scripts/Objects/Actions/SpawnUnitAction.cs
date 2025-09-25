using System.Threading;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

[CreateAssetMenu(menuName = "Actions/New Spawn Unit", fileName = "New Spawn Unit")]
public class SpawnUnitAction : SO_Action
{
    [SerializeField] private GameObject UnitToSpawn;

    public override void DoAction()
    {
        int supply = ((SO_Unit)UnitToSpawn.GetComponent<Unit>().GetSO()).supplyCost;
        if (Player.Instance.GetCurrentTotalSupply() - Player.Instance.GetCurrentSupplyInUse() < supply) { Debug.Log("Not enough supply to create unit!"); return; }
        base.DoAction();
        if (Cost > Player.Instance.GetCurrentResource()) //This return will need to be in every action that has a resource cost as this was easier than trying to put it in Action.DoAction();
            return;
        GameObject newUnit = Instantiate(UnitToSpawn, _ActionOwner.GetComponentInChildren<Waypoint>().GetWaypoint().Spawn.position, Quaternion.identity); //The only child transform should be the Waypoint
        newUnit.GetComponent<Unit>().SetMoveToWaypointOrder(_ActionOwner.GetComponent<Waypoint>().GetWaypoint().Flag.position);
    }
}
