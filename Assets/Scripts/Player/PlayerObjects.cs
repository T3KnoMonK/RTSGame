using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class PlayerObjects
{
    public delegate void PopulateSelectedActionsDelegate(List<Action> actions, Selectable ParentObject);
    public static event PopulateSelectedActionsDelegate PopulateSelectedActionsEvent;
    public delegate void RemoveSelectedActionsDelegate();
    public static event RemoveSelectedActionsDelegate RemoveSelectedActionsEvent;

    public delegate void PopulateSelectedInfoDelegate(Selectable unit);
    public static event PopulateSelectedInfoDelegate PopulateSelectedInfoEvent;

    private List<Selectable> selectedObjects = new List<Selectable>();

    [SerializeField]
    private DisplayUnitCards unitCardPanel = null; public DisplayUnitCards GetUnitCardPanel() { return unitCardPanel; }
    [SerializeField]
    private DisplayActions actionsPanel = null;
    [SerializeField]
    private GameObject unitInfoPanel = null;

    public PlayerObjects()
    {
        unitCardPanel = GameObject.FindGameObjectWithTag("UnitCardPanel").GetComponent<DisplayUnitCards>();
        actionsPanel = GameObject.FindGameObjectWithTag("ActionsPanel").GetComponent<DisplayActions>();
        unitInfoPanel = GameObject.Find("UnitInfo");
        unitInfoPanel.SetActive(false);
    }

    public List<Selectable> GetPlayerSelectedObjects()
    {
        return selectedObjects;
    }

    public int GetSelectedObjectCount()
    {
        return selectedObjects.Count;
    }

    public void ToggleIsSelected(bool set)
    {
        foreach (Selectable obj in selectedObjects)
        {
            if(obj != null)
            {
                obj.IsSelected = set;
                obj.GetComponent<HealthBarScript>()?.SetUnitCanvas(set);
                obj.SendWaypointEnable(set);
            }
        }
    }

    public void ClearSelectedObjects()
    {
        unitCardPanel.RemoveUnitCardsFromUI();
        unitInfoPanel?.SetActive(false);
        ToggleIsSelected(false);
        SignalRemoveActions();
        selectedObjects.Clear();
    }
    //TODO: Implement actions panel for multiple selected units, only actions that apply to all selected units will be available (Move,Attack,Stop...)

    public void AddSingleObjectToSelected(Selectable go)
    {
        ClearSelectedObjects();
        selectedObjects.Add(go);
        ToggleIsSelected(true);
        unitInfoPanel.SetActive(true);
        SignalDisplayUnitInfo();
        //if (go.gameObject.GetComponent<Unit>()) { unitCardPanel.AddUnitCardsToUI(); } //Do not display card if only one object is selcted, Display SelectedInfo
        SignalPopulateActions();
    }

    private void SignalRemoveActions()
    {
        RemoveSelectedActionsEvent?.Invoke();
    }

    private void SignalPopulateActions()
    {
        PopulateSelectedActionsEvent?.Invoke(selectedObjects[0].GetActions(), selectedObjects[0]);
    }

    private void SignalDisplayUnitInfo() //Displays when there is only one Selectable object selected. If multiple are selected the Details panel displays the unit cards of the selected units.
    {
        PopulateSelectedInfoEvent?.Invoke(selectedObjects[0]);
    }

    public void AddMultipleObjectsToSelected(List<Selectable> list)
    {
        if (list.Count == 0) return;

        ClearSelectedObjects();
        list.ForEach(x => { if (x.gameObject.GetComponent<Unit>()) { selectedObjects.Add(x);} });
        ToggleIsSelected(true);
        unitCardPanel.AddUnitCardsToUI();
    }

    private string PrintSelectedObjects(List<GameObject> list)
    {
        string so = "";

        list.ForEach(x => so += x.name.ToString() + ", ");

        return so;
    }
}
