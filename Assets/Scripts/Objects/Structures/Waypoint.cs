using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private Transform _WaypointObject;

    public Transform GetWaypoint() { return _WaypointObject; }

    private void OnEnable()
    {
        ToggleVisibility(false);
    }

    public void ToggleVisibility(bool isVisible)
    {
        _WaypointObject.gameObject.SetActive(isVisible);
    }
}
