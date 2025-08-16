using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

[CreateAssetMenu(menuName = "Actions/New Action", fileName = "New Action")]
public class Action : ScriptableObject
{
    public delegate void ActionPayCostDelegate(int cost);
    public static ActionPayCostDelegate ActionPayCostEvent;

    protected GameObject Parent;
    public void SetParent(GameObject parent) { Parent = parent; }

    protected GameObject AttachedButton;
    public void AttachButton(GameObject button) { AttachedButton = button; }

    public Sprite Image;
    protected string ActionName;
    protected string ActionDescription;
    [SerializeField] protected int Cost = 0;
    public int ActionCost() { return Cost; }
    public float ReadyTime = 1.0f;
    public float CooldownTime = 1.0f;

    public virtual void DoAction() 
    {
    }

    public void PayActionCost()
    {
        Player.Instance.RemoveResource(Cost);
    }
}
