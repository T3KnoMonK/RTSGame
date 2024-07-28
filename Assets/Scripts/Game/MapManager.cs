using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapManager : MonoBehaviour
{
    //Unity uses a 1/10 (1 grid square is (10,10) unit scale (default?)
    //Map size will be 100m sq to start for testing
    //Production map sizes will be determined but may be be 1km sq or larger.
    private Vector2 _MapMinBounds = Vector2.zero; //Min Bounds is 0,0
    private Vector2 _MapMaxBounds;
    public Vector2 GetMapMinBounds() { return _MapMinBounds; }
    public Vector2 GetMapMaxBounds() { return _MapMaxBounds; }

    [SerializeField] private float _MapHeight;
    [SerializeField] private float _MapWidth;

    private static MapManager instance;
    public static MapManager Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }

        _MapMaxBounds = _MapMaxBounds + new Vector2(_MapWidth, _MapHeight);
    }

}
