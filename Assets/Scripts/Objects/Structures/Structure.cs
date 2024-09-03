using System;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class Structure : Selectable
{
     //public SO_Structure structureSO;

    private Waypoint _Waypoint;

    [SerializeField] private GameObject _InitialUnitToSpawn;
    [SerializeField] private bool _DeployUnit;

    void Start()
    {
        _Waypoint = GetComponent<Waypoint>();

        if (_DeployUnit)
        {
            DeployInitialUnit(_Waypoint.GetWaypoint().Spawn.position, _Waypoint.GetWaypoint().Flag.position);
        }
    }

    private void OnEnable()
    {
        InputManager.RightClickUpEvent += MoveWaypointFlag;
    }

    private void OnDisable()
    {
        InputManager.RightClickUpEvent -= MoveWaypointFlag;
    }

    private void MoveWaypointFlag(RaycastHit target, Vector3 mouseWorldPos, bool shift)
    {
        if (IsSelected)
        {
            Transform flagT = GetComponent<Waypoint>().GetWaypoint().Flag;
            flagT.position = new Vector3(target.point.x, flagT.position.y, target.point.z);
        }
    }

    private void DeployInitialUnit(Vector3 spawn, Vector3 waypoint)
    {
        GameObject unit = Instantiate(_InitialUnitToSpawn, spawn, Quaternion.identity);
        MoveToWaypoint(unit, waypoint);
    }

    private void MoveToWaypoint(GameObject unit, Vector3 waypoint)
    {
        unit.GetComponent<Unit>().SetMoveToWaypointOrder(GetComponent<Waypoint>().GetWaypoint().Flag.position);
    }
}
