using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Selectable, IClickContext
{
    public delegate void StartBuildEventDelegate(GameObject placeholder);
    public static StartBuildEventDelegate StartBuildEvent;

    public delegate void PlaceBuildingEventDelegate();
    public static PlaceBuildingEventDelegate PlacedBuildingEvent;

    public delegate void CancelBuildEventDelegate();
    public static CancelBuildEventDelegate CancelBuildEvent;

    public delegate void DestroyUnitCardDelegate(GameObject card, GameObject unit); //need to pass unit so DisplayUnitCards can call for the Unit to be destroyed after destroying the card
    public static DestroyUnitCardDelegate DestroyUnitCardEvent;

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
        base.Update();
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

    public void SetBuildMoveOrder(GameObject placeholder)
    {
        _UnitFSM.CurrentBuildTarget = placeholder;
        _UnitFSM.ManualMoveAction = true;
        _UnitFSM.ClickPos = placeholder.transform.position;
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
        gameObject.GetComponent<Collider>().enabled = false;
        foreach(MeshRenderer rend in gameObject.GetComponentsInChildren<MeshRenderer>())
        {
            rend.enabled = false;
        }
    }

    public void Appear()
    {
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

        if (CursorManager.Instance.IsCursorPlaceholder())
        {
            CancelBuildEvent.Invoke();
            Kill_WFW_Coroutine();
        }
    }

    public void OnLeftClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift){}

    public void OnRightClickDown(RaycastHit target, Vector3 mouseWorldPos, bool shift){}

    public void OnLeftClickDown(RaycastHit hitObj, Vector3 mousePos, bool isShift)
    {
        if (!IsSelected) { return; }
        //Because the mouse down event already fired when you clicked the action button this should be the next mouse down event
        if (CursorManager.Instance.IsCursorPlaceholder())
        {
            PutDownPlaceholder(currentBuildingToPlace, CursorManager.Instance.GetPlaceholderPos());
        }
    }


    // StartBuild signals the placeholder to change. Anything that happens at the start of the action triggering should happen here. This includes handling a placeholder already existing and the unit being in the middle of an action already.
    public void StartBuild(GameObject placeholder, GameObject building)
    {
        StartBuildEvent.Invoke(Instantiate(placeholder, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity));
        Kill_WFW_Coroutine();
        currentBuildingToPlace = building;
    }

    public void PutDownPlaceholder(GameObject building, Vector3 worldPos)
    {
        if (CursorManager.Instance.GetPlaceholder().GetComponent<CheckObstruction>().IsObsructed() == false)
        {
            CursorManager.Instance.GetPlaceholder().GetComponent<FollowCursor>().StopFollowing();
            CallWorkerMoveOrder();
        }
    }

    public void Kill_WFW_Coroutine()
    {
        if(WaitForWorkerCoroutine != null)
        {
            StopCoroutine(WaitForWorkerCoroutine);
        }
    }

    private void CallWorkerMoveOrder()
    {
        if(Player.Instance.Army.GetPlayerSelectedObjects().Count > 0)
        {
            Unit tmp;

            if (Player.Instance.Army.GetPlayerSelectedObjects()[0].GetComponent<Unit>() != null)
            {
                tmp = Player.Instance.Army.GetPlayerSelectedObjects()[0] as Unit;

                if (tmp.GetUnitFSM().parentSO.unitType == 0)
                {
                    tmp.SetBuildMoveOrder(CursorManager.Instance.GetPlaceholder());
                    WaitForWorkerCoroutine = StartCoroutine("WaitForWorker");
                }
            }
        }
    }

    private Action currentActionCaller;

    public void SetActionCaller(Action actionCaller) { currentActionCaller = actionCaller; }

    private bool HasWorkerArrived;

    public void WorkerHasArrived()
    {
        HasWorkerArrived = true;
    }

    public Coroutine WaitForWorkerCoroutine;

    private IEnumerator WaitForWorker()
    {
        HasWorkerArrived = false;
        if (CursorManager.Instance.IsCursorDefault() == false) { CursorManager.Instance.SetCursorIsDefault(); } //Allows the player to box select while the worker is moving
        yield return new WaitUntil(() => HasWorkerArrived);
        GameObject newBuilding = Instantiate(currentBuildingToPlace, CursorManager.Instance.GetPlaceholderPos(), Quaternion.identity);
        PlacedBuildingEvent.Invoke();
        //Debug.Log($"Calling StartCooldown on {currentActionCaller.GetActionData().name}");
        currentActionCaller.StartCooldown();
    }

}
