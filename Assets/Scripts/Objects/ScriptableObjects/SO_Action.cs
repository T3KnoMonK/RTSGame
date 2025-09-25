using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "Actions/New Action", fileName = "New Action")]
public class 
    SO_Action : ScriptableObject
{


    protected GameObject _ActionOwner;
    public void SetOwner(GameObject actionOwner) { _ActionOwner = actionOwner; }
    protected Button _AssociatedButton;
    public void AssociateButton(Button button) { _AssociatedButton = button; }

    public Sprite Image;
    public string Name;
    public string Description;
    public int Cost;
    public float ReadyTime;
    public float CooldownTime;
    public float CurrentCooldown;
    public bool IsOnCooldown;

    //protected GameObject _Owner; //Just for SetActionCaller() and SetPlayerBuildingPlaceholder() until I can architect a better solution
    //public void SetOwner(GameObject Owner) { _Owner = Owner; }

    public virtual void DoAction() {}
}
