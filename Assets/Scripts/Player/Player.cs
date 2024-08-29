using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro.EditorUtilities;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Player : MonoBehaviour, IClickContext
{
    public delegate void PlacedBuildingDelegate();
    public static PlacedBuildingDelegate PlacedBuildingEvent;

    private static Player instance;
    public static Player Instance {  get { return instance; } }

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

        _CurrentResource = _StartingResources;
        _CurrentSupply = _StartingSupply; //This is in Awake instead of Start because there was a race condition between Player.Start and Supply.Start which was causing the CurrentSupply to remain zero
    }

    private int _PlayerNumber;
    private string _PlayerName;
    private Vector3 _PlayerColour;

    private Vector3 p1BoxSelect = Vector3.zero;
    private Vector3 p2BoxSelect = Vector3.zero;

    private bool isMouseDragging = false;
    private bool isLeftClickDown = false;

    private Rect selectBox = new Rect();

    private float dragDistanceThreshold = 0.5f;
    private float currentMouseDistanceFromClick;

    private float _CurrentResource = 0;
    [SerializeField] private int _StartingResources;
    public float GetResources() { return _CurrentResource; }
    [SerializeField] Text supplyCounter;
    [SerializeField] Text resourceCounter;

    private int _CurrentSupply;
    private int _SupplyInUse;
    [SerializeField] private int _StartingSupply;
    [SerializeField] private int _MaxSupply;
    public int GetCurrentTotalSupply() {  return _CurrentSupply; }
    public int GetCurrentSupplyInUse() {  return _SupplyInUse; }

    private Vector3 _LastHitPoint;

    private Cursor currentCursor;
    private Cursor defaultCursor;
    private GameObject currentStructurePlaceholder;
    private GameObject currentBuildingToPlace;
    private enum CursorState { Default, Placeholder }
    private CursorState cursorState = CursorState.Default;

    private PlayerObjects army;
    public PlayerObjects Army { get => army; set => army = value; }

    private void OnEnable()
    {
        UnitFSM.CollectState.DepositResourceEvent += AddResource;
        InputManager.LeftClickUpEvent += OnLeftClickUp;
        InputManager.RightClickUpEvent += OnRightClickUp;
        InputManager.LeftClickDownEvent += OnLeftClickDown;
        InputManager.RightClickDownEvent += OnRightClickDown;
        Action.ActionPayCostEvent += RemoveResource;
        //Supply.ChangeSupplyEvent += AdjustTotalSupply;
    }


    private void OnDisable()
    {
        UnitFSM.CollectState.DepositResourceEvent -= AddResource;
        InputManager.LeftClickUpEvent -= OnLeftClickUp;
        InputManager.RightClickUpEvent -= OnRightClickUp;
        InputManager.LeftClickDownEvent -= OnLeftClickDown;
        InputManager.RightClickDownEvent -= OnRightClickDown;
        Action.ActionPayCostEvent -= RemoveResource;
        //Supply.ChangeSupplyEvent -= AdjustTotalSupply;
    }

    private void Start()
    {
        army = new PlayerObjects();
        resourceCounter.text = _CurrentResource.ToString();
    }

    private void Update()
    {
        Debug.LogWarning(_CurrentSupply);
        if (cursorState == CursorState.Default) MouseDrag();

        if (Input.GetButtonDown("Left Click"))
        {
            isLeftClickDown = true;
            p1BoxSelect = Input.mousePosition;
        }
        if (Input.GetButtonUp("Left Click"))
        {
            if (isMouseDragging)
            {
                List<Selectable> selected = GetAllObjectsSelected();

                //If multiple objects are caught in the bounding box then only Untis are selected.
                //Otherwise if there's only selected then any object that inherits from Selectable can be selected.
                if(selected.Count == 1) 
                {
                    Army.AddSingleObjectToSelected(selected[0]); 
                }
                else
                {
                    Army.AddMultipleObjectsToSelected(selected);
                }
            }
            else
            {
                GameObject go = GetObjectOnClick();
                if (go != null)
                {
                    if (go.tag == "Selectable" || go.tag == "Structure" || go.tag == "Depot" || go.tag == "Resource")
                        Army.AddSingleObjectToSelected(go.GetComponent<Selectable>());
                    else if (go.tag == "Ground")
                        Army.ClearSelectedObjects();
                }
            }
            ClearBoxSelect();
            isMouseDragging = false;
            isLeftClickDown = false;
        }
    }

    private void MouseDrag()
    {
        if (isLeftClickDown)
        {
            currentMouseDistanceFromClick = Vector3.Distance(Input.mousePosition, p1BoxSelect);
        }
        if (currentMouseDistanceFromClick > dragDistanceThreshold)
        {
            isMouseDragging = true;
        }
        if (isMouseDragging)
        {
            p2BoxSelect = Input.mousePosition;
        }
    }

    private void ClearBoxSelect()
    {
        p1BoxSelect = Vector3.zero;
        p2BoxSelect = Vector3.zero;
        currentMouseDistanceFromClick = 0f;
    }

    private void OnGUI()
    {
        if (isMouseDragging)
        {
            selectBox = new Rect(p1BoxSelect.x,
                Screen.height - p1BoxSelect.y,
                p2BoxSelect.x - p1BoxSelect.x,
                -(p2BoxSelect.y - p1BoxSelect.y));
            GUI.Box(selectBox, "");
        }
    }

    private bool IsPointerOverUIObject()
    {
        List<RaycastResult> result = GetPointerRaycastResult();
        return result.Count > 0;
    }

    /// <summary>
    /// Returns list of UI elements that get hit by the raycast
    /// </summary>
    public List<RaycastResult> GetPointerRaycastResult()
    {
        PointerEventData currentEventData = new PointerEventData(EventSystem.current);
        currentEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(currentEventData, result);
        return result;
    }

    public GameObject GetObjectOnClick()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            if (!IsPointerOverUIObject())
                return hit.transform.gameObject;
        }
        return null;
    }

    //TODO: Make overloaded function that returns single unit if only one unit is captured by mouse drag
    public List<Selectable> GetAllObjectsSelected()
    {
        List<Selectable> rtnList = new List<Selectable>();

        //If units are caught in the bounding box then they get selected first
        Selectable[] tmp = GameObject.FindObjectsByType<Selectable>(FindObjectsSortMode.None);

        CheckSelectedInBoundingBox(rtnList, tmp);
        return rtnList;
    }

    private void CheckSelectedInBoundingBox(List<Selectable> rtnList, Selectable[] tmp)
    {
        
        for (int i = 0; i < tmp.Length; i++)
        {
            Vector3 loc = Camera.main.WorldToScreenPoint(tmp[i].transform.position);
            if (selectBox.Contains(new Vector3(loc.x, Screen.height - loc.y, loc.z), true))
                rtnList.Add(tmp[i].GetComponent<Selectable>());
        }
    }

    public void AddResource(int amount)
    {
        _CurrentResource += amount;
        resourceCounter.text = _CurrentResource.ToString();
    }

    public void RemoveResource(int amount)
    {
        Debug.Log("Took " +  amount + " resources");
        _CurrentResource -= amount;
        resourceCounter.text = _CurrentResource.ToString();
    }

    public int GetCurrentResource()
    {
        return (int)_CurrentResource;
    }

    public void OnRightClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift){
        if (cursorState == CursorState.Placeholder)
        {
            SetCursorDefault();
            Destroy(currentStructurePlaceholder);
        }
    }

    public void OnLeftClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift)
    {
        //Running PlaceBuilding() here causes it to fire immediately after you set the placeholder since the mouse up event is directly after; This made me add the separate down/up click events and methods.
    }


    private void OnRightClickDown(RaycastHit target, Vector3 mouseWorldPos, bool shift)
    {
    }

    public void OnLeftClickDown(RaycastHit hitObj, Vector3 mousePos, bool isShift)
    {
        //Because the mouse down event already fired when you clicked the action button this should be the next mouse down event
        if (cursorState == CursorState.Placeholder)
        {
            PlaceBuilding(currentBuildingToPlace, currentStructurePlaceholder.transform.position);
        }
    }

    private Action currentActionCaller;

    public void SetActionCaller(Action actionCaller) { currentActionCaller = actionCaller; }

    public void SetPlayerBuildingPlaceholder(GameObject placeholder, GameObject building) {
        if(currentStructurePlaceholder != null) { Destroy(currentStructurePlaceholder); Kill_WFW_Coroutine(); }
        
        cursorState = CursorState.Placeholder;
        currentBuildingToPlace = building;
        //currentStructurePlaceholder = placeholder;
        currentStructurePlaceholder = Instantiate(placeholder, Camera.main.ScreenToWorldPoint(Input.mousePosition), Quaternion.identity);

        //if(currentStructurePlaceholder != null) { Destroy(currentStructurePlaceholder); } //Don't destroy, replace.
        //if (currentStructurePlaceholder != null)
        //{
        //    currentStructurePlaceholder = placeholder;
        //}
    }

    public void SetCursorDefault()
    {
        cursorState = CursorState.Default;
    }

    public void PlaceBuilding(GameObject building, Vector3 worldPos)
    {
        if(currentStructurePlaceholder.GetComponent<CheckObstruction>()!.IsObsructed() == false)
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
        foreach(Selectable unit in army.GetPlayerSelectedObjects())
        {
            Unit tmp = unit as Unit;
            if(tmp.GetUnitFSM().parentSO.unitType == 0)
            { 
                tmp.SetBuildMoveOrder(currentStructurePlaceholder.transform.position, currentStructurePlaceholder);
            }
        }
    }

    public Coroutine WaitForWorkerCoroutine;

    private IEnumerator WaitForWorker(GameObject building, Vector3 worldPos)
    {
        HasWorkerArrived = false;
        if (cursorState != CursorState.Default) { SetCursorDefault(); } //Allows the player to box select while the worker is moving
        yield return new WaitUntil(() => HasWorkerArrived);
        Destroy(currentStructurePlaceholder);
        GameObject newBuilding = Instantiate(building, worldPos, Quaternion.identity);
        RemoveResource(currentActionCaller.ActionCost());
        Debug.Log(WaitForWorkerCoroutine.ToString());
    }

    private bool HasWorkerArrived;

    public void WorkerHasArrived()
    {
        HasWorkerArrived = true;
    }

    public void AdjustTotalSupply(int amount)
    {
        if(_CurrentSupply + amount > _MaxSupply)
        {
            _CurrentSupply = _MaxSupply;
        }
        else if(_CurrentSupply + amount < 0)
        {
            _CurrentSupply = 0;
        }
        else
        {
            _CurrentSupply += amount;
        }
        supplyCounter.text = _SupplyInUse.ToString() + " / " + _CurrentSupply.ToString();

    }

    public void AdjustSupplyInUse(int amount) //Supply in use is determined by units created. The SpawnUnitAction should be the one to check bounds when affecting this value.
    {
        _SupplyInUse += amount;
        if (supplyCounter.text != null) { supplyCounter.text = _SupplyInUse.ToString() + " / " + _CurrentSupply.ToString(); }
        else { Debug.Log("No Supply Counter found"); }
    }

}
