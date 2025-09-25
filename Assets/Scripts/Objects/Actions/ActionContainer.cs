using UnityEngine;
using System.Collections.Generic;

public class ActionContainer : MonoBehaviour
{
    [SerializeField] private List<SO_Action> _ActionData;
    public List<SO_Action> GetActionData() {  return _ActionData; }

    public void StartCooldown(int ActionListIndex)
    {
        _ActionData[ActionListIndex].IsOnCooldown = true;
        _ActionData[ActionListIndex].CurrentCooldown = _ActionData[ActionListIndex].CooldownTime;
    }

    private void Start()
    {
        GameObject owner = gameObject;
        foreach (SO_Action action in GetActionData())
        {
            action.SetOwner(owner);
        }
    }

    private void Update()
    {
        foreach(SO_Action action in _ActionData)
        {
            if (action.IsOnCooldown)
            {
                action.CurrentCooldown -= Time.deltaTime;
                if(action.CurrentCooldown <= 0.0f)
                {
                    action.CurrentCooldown = 0.0f;
                    action.IsOnCooldown = false;
                }
            }
        }
    }
}