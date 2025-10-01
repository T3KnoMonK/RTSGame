using UnityEngine;
using System.Collections.Generic;

public class ActionContainer : MonoBehaviour
{
    [SerializeField] private List<SO_Action> _ActionData;
    public List<SO_Action> GetActionData() {  return _ActionData; }

    private void Start()
    {
        foreach (SO_Action action in GetActionData())
        {
            action.SetOwner(gameObject);
        }
    }

    private void Update()
    {
        foreach(SO_Action action in _ActionData)
        {
            if (action.IsOnCooldown)
            {
                action.CurrentCooldown -= Time.deltaTime;
                Debug.Log($"{action.name} is cooling down: {action.CurrentCooldown} left.");
                if (gameObject.GetComponent<Selectable>().IsSelected)
                {
                    action.GetButton().GetComponent<FillMaskHelper>().GetFillMask().fillAmount = action.CurrentCooldown / action.CooldownTime;
                }
                if(action.CurrentCooldown <= 0.0f)
                {
                    Debug.Log($"{action.name} is off cooldown;");
                    action.CurrentCooldown = 0.0f;
                    action.IsOnCooldown = false;
                    action.GetButton().GetComponent<FillMaskHelper>().GetFillMask().fillAmount = 0.0f;
                }
            }
        }
    }
}