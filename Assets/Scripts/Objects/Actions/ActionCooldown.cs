using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ActionCooldown : MonoBehaviour
{
    public void SetActionTimers(float cooldown, float ready) { CooldownTime = cooldown; ReadyTime = ready; }
    private float CooldownTime;
    private float CooldownTimeLeft;
    private float ReadyTime;
    private float TimeScale = 1.0f;
    private bool IsOnCooldown;
    private Button Btn;
    [SerializeField] private Image CooldownMask;

    private void Start()
    {
        IsOnCooldown = false;
        Btn = GetComponent<Button>();
        //CooldownMask = GetComponentInChildren<Image>();
        CooldownMask.type = Image.Type.Filled;
        CooldownMask.fillMethod = Image.FillMethod.Radial360;
        CooldownMask.fillOrigin = (int)Image.Origin360.Top;
        CooldownMask.fillAmount = 0.0f;
    }

    private void Update()
    {
        if (IsOnCooldown)
        {
            CooldownTimeLeft -= Time.deltaTime * TimeScale;
            CooldownMask.fillAmount = CooldownTimeLeft / CooldownTime;
            Debug.Log($"Mask on {Btn.name} is {CooldownMask.fillAmount} filled.");
        }
    }


    public IEnumerator RunCooldown()
    {
        IsOnCooldown = true;
        CooldownTimeLeft = CooldownTime;
        CooldownMask.fillAmount = 1.0f;
        DisableButtonInput();
        yield return new WaitForSeconds(CooldownTime);
        IsOnCooldown = false;
        EnableButtonInput();
    }

    private void DisableButtonInput()
    {
        Btn.interactable = false;
        IsOnCooldown = true;
        //Play Cooldown animation for CooldownTime seconds
    }

    private void EnableButtonInput()
    {
        Btn.interactable = true;
        IsOnCooldown = false;
    }

    private Coroutine cd;

    public void TriggerCooldown()
    {
        StartCoroutine("RunCooldown");
    }

}
