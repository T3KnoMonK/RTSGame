using System.Threading;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

[CreateAssetMenu(menuName = "Actions/New Spawn Unit", fileName = "New Spawn Unit")]
public class SpawnUnitAction : SO_Action
{
    [SerializeField] private GameObject UnitToSpawn;

    //Target is the building that is spawning the unit
    public override void DoAction(GameObject target)
    {
        int supply = ((SO_Unit)UnitToSpawn.GetComponent<Unit>().GetSO()).supplyCost;
        if (Player.Instance.GetCurrentTotalSupply() - Player.Instance.GetCurrentSupplyInUse() < supply) { Debug.Log("Not enough supply to create unit!"); return; }
        if (Cost > Player.Instance.GetCurrentResource()) //This return will need to be in every action that has a resource cost as this was easier than trying to put it in Action.DoAction();
            return;
        GameObject newUnit = Instantiate(UnitToSpawn, target.GetComponentInChildren<Waypoint>().GetWaypoint().Spawn.position, Quaternion.identity); //The only child transform should be the Waypoint
        newUnit.GetComponent<Unit>().SetMoveToWaypointOrder(target.GetComponent<Waypoint>().GetWaypoint().Flag.position);
    }
}