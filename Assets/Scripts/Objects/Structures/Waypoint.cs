using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Waypoint : MonoBehaviour
{
    [SerializeField]
    private Transform _SpawnWaypoint;

    public Transform GetWaypoint() { return _SpawnWaypoint; }

    private void OnEnable()
    {
        ToggleVisibility(false);
    }

    public void ToggleVisibility(bool isVisible)
    {
        _SpawnWaypoint.gameObject.SetActive(isVisible);
    }
}
