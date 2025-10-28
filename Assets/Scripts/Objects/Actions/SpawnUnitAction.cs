using UnityEngine;

[CreateAssetMenu(menuName = "Actions/New Spawn Unit", fileName = "New Spawn Unit")]
public class SpawnUnitAction : SO_Action
{
    [SerializeField] private GameObject UnitToSpawn;

    //Target is the building that is spawning the unit
    public override int DoAction(GameObject target)
    {
        if (Cost > Player.Instance.GetCurrentResource()) //This return will need to be in every action that has a resource cost as this was easier than trying to put it in Action.DoAction();
            return 0;

        int SupplyNeed = (UnitToSpawn.GetComponent<Unit>().GetSO() as SO_Unit).supplyCost;
        if (SupplyNeed > Player.Instance.GetAvailableSupply()) 
        { 
            Debug.Log("Not enough supply to create unit!"); 
            return 0; 
        }

        GameObject newUnit = Instantiate(UnitToSpawn, target.GetComponentInChildren<Waypoint>().GetWaypoint().Spawn.position, Quaternion.identity);
        newUnit.GetComponent<Unit>().SetMoveToWaypointOrder(target.GetComponent<Waypoint>().GetWaypoint().Flag.position);
        return 1;
    }
}