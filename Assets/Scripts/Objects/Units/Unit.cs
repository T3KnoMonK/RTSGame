using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Selectable, IClickContext
{
    public delegate void PlacedBuildingDelegate();
    public static PlacedBuildingDelegate PlacedBuildingEvent;

    public delegate void DestroyUnitCardDelegate(GameObject card, GameObject unit); //need to pass unit so DisplayUnitCards can call for the Unit to be destroyed after destroying the card
    public static DestroyUnitCardDelegate DestroyUnitCardEvent;

    private GameObject currentStructurePlaceholder;
    private GameObject currentBuildingToPlace;

    protected UnitFSM _UnitFSM;
    public UnitFSM GetUnitFSM() { return _UnitFSM; }

    public NavMeshAgent NavAgent;
    public ParticleSystem BulletParticle;
    private List<ParticleCollisionEvent> _CollisionEvents;

    private HealthBarScript _HealthBarScript;
    private GameObject _UnitCardRef;

    private float _Speed;
    private float _MaxSpeed;
    private int _MaxCargo;
    private int _UnitSupply;

    [HideInInspector] public float Damage;
    [HideInInspector] public float AttackSpeed;
    [HideInInspector] public float AttackDistance;

    [HideInInspector] public int Cargo;
    [HideInInspector] public int GatherRate;
    [HideInInspector] public int UnitSupply;

    new private void Awake()
    {
        base.Awake();
        SO_Unit unitSO = SelectedSO as SO_Unit;
        _Speed = unitSO.speed;
        _MaxCargo = unitSO.maxCargo;

        Damage = unitSO.damage;
        GatherRate = unitSO.gatherRate;
        AttackDistance = unitSO.attackDistance;
        AttackSpeed = unitSO.attackSpeed;
        UnitSupply = unitSO.supplyCost;

        Cargo = 0;

        _UnitFSM = gameObject.AddComponent<UnitFSM>();
        _UnitFSM.SetParent(this);
    }

    private void OnEnable()
    {
        InputManager.LeftClickUpEvent += OnLeftClickUp;
        InputManager.RightClickUpEvent += OnRightClickUp;
        InputManager.LeftClickDownEvent += OnLeftClickDown;
        InputManager.RightClickDownEvent += OnRightClickDown;
    }


    private void OnDisable()
    {
        InputManager.LeftClickUpEvent -= OnLeftClickUp;
        InputManager.RightClickUpEvent -= OnRightClickUp;
        InputManager.LeftClickDownEvent -= OnLeftClickDown;
        InputManager.RightClickDownEvent -= OnRightClickDown;
    }

    private void Start()
    {
        ID = GetInstanceID(); //When this is in Selectable all IDs are zero (0)
        NavAgent = gameObject.GetComponent<NavMeshAgent>();
        BulletParticle = gameObject.GetComponent<ParticleSystem>();
        _HealthBarScript = gameObject.GetComponent<HealthBarScript>();
        IsSelected = false;
        Player.Instance.AdjustSupplyInUse(UnitSupply);
    }

    private void OnDestroy()
    {
        Player.Instance.AdjustSupplyInUse(UnitSupply * -1);
    }

    private void Update()
    {
        _UnitFSM.Update();
    }

    private void TakeDamage(int damage)
    {
        _Health -= damage;
        Debug.Log(gameObject.name + " is taking " + damage + " damage!");
        _HealthBarScript.UpdateHealth(_Health);
        _HealthBarScript.SetHealthDisplay();
        if (_Health <= 0)
        {
            Debug.Log(gameObject.name + " died!");
            DestroyUnitCard();
        }
    }

    public void SetBuildMoveOrder(Vector3 pos, GameObject buildingPlaceholder)
    {
        _UnitFSM.CurrentBuildTarget = buildingPlaceholder;
        _UnitFSM.ManualMoveAction = true;
        _UnitFSM.ClickPos = pos;
        _UnitFSM.ChangeState(_UnitFSM.GetState("MOVE"));
    }

    public void SetMoveToWaypointOrder(Vector3 pos)
    {
        _UnitFSM.ClickPos = pos;
        _UnitFSM.ChangeState(_UnitFSM.GetState("MOVE"));
    }

    public void SetUnitCardRef(GameObject card)
    {
        _UnitCardRef = card;
    }

    private void DestroyUnitCard()
    {
        DestroyUnitCardEvent?.Invoke(_UnitCardRef, gameObject);
    }

    public void Disappear()
    {
        //Debug.Log("Unit disappering");
        gameObject.GetComponent<Collider>().enabled = false;
        foreach(MeshRenderer rend in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = false;
        }
    }

    public void Appear()
    {
        //Debug.Log("Unit reappearing");
        gameObject.GetComponent<Collider>().enabled = true;
        foreach (MeshRenderer rend in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = true;
        }
    }

    private void ResumeGathering()
    {
        UnitFSM.CollectState col = (UnitFSM.CollectState)_UnitFSM.GetState("COLLECT");
        col.ResumeGathering();
    }

    public void OnRightClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift)
    {
        if (!IsSelected) { return; }

        if (Player.Instance.IsCursorPlaceholder())
        {
            Player.Instance.SetCursorDefault();
            Destroy(currentStructurePlaceholder);
            
        }
    }

    public void OnLeftClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift){}

    public void OnRightClickDown(RaycastHit target, Vector3 mouseWorldPos, bool shift){}

    public void OnLeftClickDown(RaycastHit hitObj, Vector3 mousePos, bool isShift)
    {
        if (!IsSelected) { return; }
        //Because the mouse down event already fired when you clicked the action button this should be the next mouse down event
        if (Player.Instance.IsCursorPlaceholder())
        {
            PlaceBuilding(currentBuildingToPlace, currentStructurePlaceholder.transform.position);
        }
    }

    public void SetPlayerBuildingPlaceholder(GameObject placeholder, GameObject building)
    {
        if (currentStructurePlaceholder != null)
        {
            Destroy(currentStructurePlaceholder);
            Kill_WFW_Coroutine();
        }

        Player.Instance.SetCursorPlaceholder();
        currentBuildingToPlace = building;
        currentStructurePlaceholder = Instantiate(placeholder, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);
    }

    public void PlaceBuilding(GameObject building, Vector3 worldPos)
    {
        if (currentStructurePlaceholder.GetComponent<CheckObstruction>()!.IsObsructed() == false)
        {
            CallWorkerMoveOrder();
            currentStructurePlaceholder.GetComponent<FollowCursor>().StopFollowing();
            WaitForWorkerCoroutine = StartCoroutine(WaitForWorker(building, worldPos));
        }
    }

    public void Kill_WFW_Coroutine()
    {
        StopCoroutine(WaitForWorkerCoroutine);
    }

    private void CallWorkerMoveOrder()
    {
        foreach (Selectable unit in Player.Instance.Army.GetPlayerSelectedObjects())
        {
            Unit tmp = unit as Unit;
            if (tmp.GetUnitFSM().parentSO.unitType == 0)
            {
                tmp.SetBuildMoveOrder(currentStructurePlaceholder.transform.position, currentStructurePlaceholder);
            }
        }
    }

    public Coroutine WaitForWorkerCoroutine;

    private SO_Action currentActionCaller;

    public void SetActionCaller(SO_Action actionCaller) { currentActionCaller = actionCaller; }

    private IEnumerator WaitForWorker(GameObject building, Vector3 worldPos)
    {
        HasWorkerArrived = false;
        if (Player.Instance.IsCursorDefault() == false) { Player.Instance.SetCursorDefault(); } //Allows the player to box select while the worker is moving
        yield return new WaitUntil(() => HasWorkerArrived);
        Destroy(currentStructurePlaceholder);
        GameObject newBuilding = Instantiate(building, worldPos, Quaternion.identity);
        //Player.Instance.RemoveResource(currentActionCaller.ActionCost());
    }

    private bool HasWorkerArrived;

    public void WorkerHasArrived()
    {
        HasWorkerArrived = true;
    }

}
