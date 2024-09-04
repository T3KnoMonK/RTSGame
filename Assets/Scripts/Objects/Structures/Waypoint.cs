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
        Debug.LogWarning(gameObject.name + " is calling ToggleVisibility on it's Waypoint script");
        _WaypointObject.gameObject.SetActive(isVisible);
    }
}
