//using System.Collections.Generic;
//using UnityEngine;
//using System.Collections;
//using UnityEngine.UI;

//public class ActionHelper : MonoBehaviour
//{
//    [SerializeField] private List<Action> _Actions = new List<Action>();
//    public List<Action> GetActions() {  return _Actions; }


//    public void SetActionTimers(float cooldown, float ready) { CooldownTime = cooldown; ReadyTime = ready; }

//    private GameObject _Owner;
//    public void SetOwner(GameObject Owner) { _Owner = Owner; }

//    public bool IsOwnerSelected = false;

//    private float StartTimestamp; //StartTimestamp = Time.time;
//    private float EndTimestamp; //EndTimestamp = StartTimestamp + CooldownTime;


//    private float CooldownTime;
//    private float CooldownTimeLeft; //CooldownTimeleft = Time.time / EndTimestamp; If(CooldownTimeleft >= 1.0f){ **Cooldown finished** }
//    private float ReadyTime;
//    private float TimeScale;
//    private bool IsOnCooldown;
//    private Button Btn;
//    [SerializeField] private Image CooldownMask;

//    private GameObject _Parent;

//    private void Start(){
//        _Parent = gameObject;

//        IsOnCooldown = false;
//        Btn = GetComponent<Button>();
//        CooldownMask.type = Image.Type.Filled;
//        CooldownMask.fillMethod = Image.FillMethod.Radial360;
//        CooldownMask.fillOrigin = (int)Image.Origin360.Top;
//        CooldownMask.fillAmount = 0.0f;
//    }


//    private void Update()
//    {
//        if (IsOnCooldown)
//        {
//            CooldownTimeLeft = EndTimestamp - Time.time;
//            CooldownMask.fillAmount = CooldownTimeLeft / EndTimestamp;
//            if (CooldownMask.fillAmount > 1.0f) { CooldownMask.fillAmount = 1.0f; EndCD(); }
//        }
//    }

//    public IEnumerator StartCooldown(Action action)
//    {
//        action.IsOnCooldown = true;
//        yield return new WaitForSeconds(action.CooldownTime);
//        action.IsOnCooldown = false;
//    }

    
//    private void DisableButtonInput()
//    {
//        Btn.interactable = false;
//        IsOnCooldown = true;
//    }

//    private void EnableButtonInput()
//    {
//        Btn.interactable = true;
//        IsOnCooldown = false;
//    }

//    public void ActivateCD()
//    {
//        IsOnCooldown = true;
//        StartTimestamp = Time.time;
//        EndTimestamp = StartTimestamp + CooldownTime;
//    }

//    private void EndCD()
//    {
//        IsOnCooldown = false;
//    }
//}