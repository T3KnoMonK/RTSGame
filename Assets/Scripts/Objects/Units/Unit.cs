using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class Unit : Selectable
{
    public delegate void DestroyUnitCardDelegate(GameObject card, GameObject unit); //need to pass unit so DisplayUnitCards can call for the Unit to be destroyed after destroying the card
    public static DestroyUnitCardDelegate DestroyUnitCardEvent;

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

    private void Awake()
    {
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

    private void Start()
    {
        ID = GetInstanceID(); //When this is in Selectable all IDs are zero (0)
        Debug.Log(gameObject.name + " id: " + ID);
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

    public void SetBuildMoveOrder(Vector3 pos, GameObject buildTarget/*Building PLaceholder*/)
    {
        _UnitFSM.CurrentBuildTarget = buildTarget;
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
}
