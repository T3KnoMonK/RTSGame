using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class DisplayActions : MonoBehaviour
{
    [SerializeField] private Button[] _ChildButtons;

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

    public void SetSelectedActions(List<Action> actions, GameObject owner)
    {
        for (int i = 0; i < actions.Count; i++)
        {
            Debug.Log($"Set actions index {i}");
            int currentIndex = i; // Create local variable to capture the correct index for the lambda
            _ChildButtons[i].GetComponent<Image>().sprite = actions[i].GetActionData().Image;
            actions[i].SetCooldownMaskRef(_ChildButtons[i].GetComponent<FillMaskHelper>().GetFillMask());
            _ChildButtons[i].onClick.RemoveAllListeners();
            _ChildButtons[i].onClick.AddListener(delegate { actions[currentIndex].DoAction(owner); }); // delegate for the callback because I have to pass a parameter
            _ChildButtons[i].gameObject.SetActive(true);
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