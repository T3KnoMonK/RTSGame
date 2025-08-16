using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class DisplayActions : MonoBehaviour
{
    [SerializeField] private Button[] _ChildButtons;
    private List<Action> _SelectedActions;

    private void Start()
    {
        _ChildButtons = GetComponentsInChildren<Button>();
    }

    private void OnEnable()
    {
        PlayerObjects.PopulateSelectedActionsEvent += SetSelectedActions;
        PlayerObjects.RemoveSelectedActionsEvent += DisableActionButtons;
    }

    private void OnDisable()
    {
        PlayerObjects.PopulateSelectedActionsEvent -= SetSelectedActions;
        PlayerObjects.RemoveSelectedActionsEvent -= DisableActionButtons;
    }

    public void SetSelectedActions(List<Action> actions, Selectable parent)
    {
        _SelectedActions = actions;
        for (int i = 0; i < _SelectedActions.Count; i++)
        {
            _ChildButtons[i].GetComponent<Image>().sprite = _SelectedActions[i].Image;
            _ChildButtons[i].onClick.RemoveAllListeners();
            _ChildButtons[i].GetComponent<Button>().onClick.AddListener(_SelectedActions[i].DoAction);
            _SelectedActions[i].SetParent(parent.gameObject);
            _SelectedActions[i].AttachButton(_ChildButtons[i].gameObject);
            ActionCooldown ac = _ChildButtons[i].GetComponent<ActionCooldown>();
            ac.SetActionTimers(actions[i].CooldownTime, actions[i].ReadyTime);
            _ChildButtons[i].gameObject.SetActive(true);
        }
    }

    private void QueueActionToUnit(Action action)
    {
        if (Input.GetKey(KeyCode.RightShift) || Input.GetKey(KeyCode.LeftShift))
        {
            foreach (Unit unit in Player.Instance.Army.GetPlayerSelectedObjects())
            {
                unit.QueueAction(action);
                Debug.Log("Added action: " + action.name + " to unit: " + unit.name + "'s action queue.");
            }
        }
        else
        {
            foreach (Unit unit in Player.Instance.Army.GetPlayerSelectedObjects())
            {
                unit.CleanQueueAction(action);
                Debug.Log("Added action: " + action.name + " to unit: " + unit.name + "'s action queue.");
            }
        }
        
    }

    private void DisableActionButtons()
    {
        for(int i = 0; i < _ChildButtons.Length; i++)
        {
            if (_ChildButtons[i].gameObject.activeSelf == true)
            {
                _ChildButtons[i].gameObject.SetActive(false);
            }
        }
    }


}