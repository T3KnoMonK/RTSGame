using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private WaypointScript _WaypointObject;

    public WaypointScript GetWaypoint() { return _WaypointObject; }

    private void Awake()
    {
        ToggleVisibility(false);
    }

    public void ToggleVisibility(bool isVisible)
    {
        _WaypointObject.gameObject.SetActive(isVisible);
    }
}
