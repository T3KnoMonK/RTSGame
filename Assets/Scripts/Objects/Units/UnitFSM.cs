using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class UnitFSM : FSM, IClickContext
{
    public Unit Parent { get; private set; }
    public SO_Unit parentSO { get; private set; }
    public void SetParent(Unit parent) { Parent = parent; }

    public GameObject Target { get; set; }
    public GameObject CurrentBuildTarget { get; set; } //Only for constructors, i.e. Worker, so you can destroy the placeholder if a new order is issued
    public Vector3 ClickPos { get; set; }
    public bool ManualMoveAction = false;

    [HideInInspector] public Vector3 CurrentPos, MouseLastPos;

    [SerializeField] private List<GameObject> actionButtons;

    public override void Awake()
    {
        IdleState idle = new IdleState(this);
        MoveState move = new MoveState(this);
        CollectState collect = new CollectState(this);
        AttackState attack = new AttackState(this);

        states.Add("IDLE", idle);
        states.Add("MOVE", move);
        states.Add("COLLECT", collect);
        states.Add("ATTACK", attack);
        defaultState = states["IDLE"];
        currentState = defaultState;
    }

    public override void Start()
    {
        parentSO = Parent.GetSO() as SO_Unit;
    }

    private void ChangeStateOnNewTarget()
    {
        if (Parent.IsSelected)
        {
            if (Target != null)
            {
                if(ManualMoveAction == true)
                {
                    ManualMoveAction = false;
                    Player.Instance.StopAllCoroutines(); //The only coroutine on Player at this time is WaitForWorkerCoroutine. This stops coroutines from piling up when you interrupt the build order
                }
                if (CurrentBuildTarget != null) { Destroy(CurrentBuildTarget); }

                switch (Target.tag)
                {
                    case "Ground":
                        ChangeState(GetState("MOVE"));
                        break;
                    case "Selectable":
                        ChangeState(GetState("ATTACK"));
                        break;
                    case "Resource":
                        if(parentSO.unitType == 0)
                        {
                            ChangeState(GetState("COLLECT"));
                        }
                        break;
                    default:
                        break;
                }
            }
        }
    }


    public new void Update()
    {
        base.Update();
    }

    public override void ChangeState(State newState)
    {
        base.ChangeState(newState);
    }

    public State GetState(string stateName)
    {
        return states.ContainsKey(stateName) ? states[stateName] : null;
    }

    public void OnEnable()
    {
        InputManager.RightClickUpEvent += OnRightClickUp;
        InputManager.LeftClickUpEvent += OnLeftClickUp;
    }

    public void OnDisable()
    {
        InputManager.RightClickUpEvent -= OnRightClickUp;
        InputManager.LeftClickUpEvent -= OnLeftClickUp;
    }

    public void OnRightClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift)
    {
        if (Parent.IsSelected && hitObj.transform != null)
        {
            Target = hitObj.transform.gameObject;
            ClickPos = hitObj.point;
            MouseLastPos = mousePos;
            ChangeStateOnNewTarget();
        }
    }

    public void OnLeftClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift)
    {

    }

    public void DoCoroutine(IEnumerator routine)
    {
        Parent.StartCoroutine(routine);
    }

}
