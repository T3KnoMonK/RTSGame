using UnityEngine;

public class Structure : Selectable
{
     //public SO_Structure structureSO;

    private Waypoint _Waypoint;

    [SerializeField] private GameObject _InitialUnitToSpawn;
    [SerializeField] private bool _DeployUnit;

    void Start()
    {
        _Waypoint = GetComponent<Waypoint>();

        if(_DeployUnit)
            DeployInitialUnit();
    }

    private void DeployInitialUnit()
    {
        if (_InitialUnitToSpawn != null)
        {
            Instantiate(_InitialUnitToSpawn, _Waypoint.GetWaypoint().position, Quaternion.identity);
        }
        else
        {
            Debug.LogError("No Initial Unit reference from " + gameObject.name);
        }
    }
}
