using UnityEngine;
using UnityEngine.UI;

public class Action
{
    private SO_Action _ActionData;
    public SO_Action GetActionData() { return _ActionData; }

    private GameObject _Owner;
    public GameObject GetOwner() {  return _Owner; }

    private float _CooldownTimer = 0f;
    private bool _IsOnCooldown = false;

    private Image _CooldownMaskRef;
    public void SetCooldownMaskRef(Image mask) {  _CooldownMaskRef = mask; }

    public Action(SO_Action actionData, GameObject owner)
    {
        _ActionData = actionData;
        _Owner = owner;
    }

    public void StartCooldown()
    {
        if(_IsOnCooldown) { return; }
        _CooldownTimer = _ActionData.CooldownTime;
        _IsOnCooldown = true;
    }

    public void UpdateCooldown()
    {
        if (_IsOnCooldown)
        {
            _CooldownTimer -= Time.deltaTime;
            Debug.Log($"{_ActionData.name} has {_CooldownTimer} left on cooldown");

            if (_CooldownMaskRef != null)
            {
                _CooldownMaskRef.fillAmount = _CooldownTimer / _ActionData.CooldownTime;
            }
        }


        if(_CooldownTimer <= 0.0f)
        {
            _CooldownTimer = 0.0f;
            _IsOnCooldown = false;
            if (_CooldownMaskRef != null)
            {
                _CooldownMaskRef.fillAmount = 0.0f;
            }
        }

    }


}