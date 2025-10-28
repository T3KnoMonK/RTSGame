using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class DisplayActions : MonoBehaviour
{
    [SerializeField] private Button[] _ChildButtons;

    private void Start()
    {
        _ChildButtons = GetComponentsInChildren<Button>();
        ClearButtons();
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

    public void SetSelectedActions(List<Action> actions, GameObject owner)
    {

        for (int i = 0; i < actions.Count; i++)
        {
            _ChildButtons[i].gameObject.SetActive(true);

            int currentIndex = i; // Create local variable to capture the correct index for AddListener(). Not having this causes index out of bounds

            _ChildButtons[i].GetComponent<Image>().sprite = actions[i].GetActionData().Image;
            actions[i].SetCooldownMaskRef(_ChildButtons[i].GetComponent<FillMaskHelper>().GetFillMask());
            _ChildButtons[i].GetComponent<FillMaskHelper>().GetFillMask().fillAmount = actions[i].GetCurrentCooldown();

            _ChildButtons[i].onClick.RemoveAllListeners();
            _ChildButtons[i].onClick.AddListener(() => actions[currentIndex].DoAction(owner)); // lamdba so I can pass a parameter

            actions[i].SetActive(true);
            //Debug.Log($"The {actions[i].GetActionData().Name} action is set to true: {actions[i].GetActive()}");
        }
    }

    private void DisableActionButtons(List<Action> actions, GameObject owner)
    {
        foreach (Action action in actions)
        {
            if (action.GetActive())
            {
                action.SetActive(false);
            }
        }

        ClearButtons();
    }

    private void ClearButtons()
    {
        for (int i = 0; i < _ChildButtons.Length; i++)
        {
            if (_ChildButtons[i].gameObject.activeSelf == true)
            {
                _ChildButtons[i].gameObject.SetActive(false);
            }
        }
    }
}