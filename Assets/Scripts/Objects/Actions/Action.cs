using UnityEngine;
using UnityEngine.UI;
//using static SO_Action;

public class Action
{
    private SO_Action _ActionData;
    public SO_Action GetActionData() { return _ActionData; }

    private GameObject _Owner;
    public GameObject GetOwner() {  return _Owner; }

    private float _CooldownTimer = 0f;
    private bool _IsOnCooldown = false;
    public bool IsOnCooldown() { return _IsOnCooldown; }
    public float GetCurrentCooldown() { return _CooldownTimer; }

    private bool _IsActive;
    public bool GetActive() { return _IsActive; }
    public void SetActive(bool value) { _IsActive = value; }

    private Image _CooldownMaskRef;

    public void SetCooldownMaskRef(Image mask) {  _CooldownMaskRef = mask; }

    public Action(SO_Action actionData, GameObject owner)
    {
        _ActionData = actionData;
        _Owner = owner;
    }

    public void DoAction(GameObject target)
    {
        //Debug.Log("Calling DoAction");
        if (_IsOnCooldown) { return; }
        //Debug.Log("DoAction is off cooldown");
        switch (_ActionData.Type)
        {
            case ActionType.Immediate:
                //Debug.Log("Calling Calling Immediate abiligy");
                _ActionData.DoAction(target);
                StartCooldown();
                break;
            case ActionType.Placement:
                //Debug.Log("Calling Placement ability");
                target.GetComponent<Unit>().SetActionCaller(this);
                _ActionData.DoAction(target);
                //StartCooldown is called at the end of the WaitForWorker coroutine, when the building is instantiated
                break;
            default:
                break;
        }
    }

    public void StartCooldown()
    {
        _CooldownTimer = _ActionData.CooldownTime;
        _IsOnCooldown = true;
        //Debug.Log($"{_ActionData.name} should be on cooldown: {_IsOnCooldown}");
    }

    public void UpdateCooldown()
    {
        //Debug.Log($"{_ActionData.name} cooldown: {_IsOnCooldown}/ active: {_IsActive}");
        //Debug.Log($"{_ActionData.name}");
        if (_IsOnCooldown)
        {
            //Debug.Log($"{_ActionData.Name} passed cooldown check");
            _CooldownTimer -= Time.deltaTime;
            if (_CooldownTimer <= 0.0f)
            {
                _CooldownTimer = 0.0f;
                _IsOnCooldown = false;
            }
            if (_IsActive)
            {
                _CooldownMaskRef.fillAmount = _CooldownTimer / _ActionData.CooldownTime;
            }
        }
    }
}