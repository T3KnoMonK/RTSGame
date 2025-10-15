using System.Collections.Generic;
using UnityEngine;

public class MinimapHelper : MonoBehaviour
{
    public delegate void RegisterWithMinimapDelegate(GameObject mapObj);
    public static RegisterWithMinimapDelegate RegisterWithMinimap;

    private List<GameObject> _MapObjects = new List<GameObject>();

    private Vector2 _MapSize;

    private void OnEnable()
    {
        RegisterWithMinimap += AddMapObject;
    }

    private void OnDisable()
    {
        RegisterWithMinimap -= AddMapObject;
    }

    private void AddMapObject(GameObject mapObj)
    {
        _MapObjects.Add(mapObj);
    }

    private void FixedUpdate()
    {
        
    }
}
