using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaypointScript : MonoBehaviour
{
    public Transform SpawnTransform;
    private void Start()
    {
        SpawnTransform = transform;
    }
}
