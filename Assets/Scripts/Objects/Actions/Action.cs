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

    public Sprite Image;
    protected string ActionName;
    protected string ActionDescription;
    [SerializeField]protected int Cost = 0;
    public int ActionCost() { return Cost; }
    protected int SecondsToComplete;

    public virtual void DoAction() {}

    public void PayActionCost()
    {
        Player.Instance.RemoveResource(Cost);
    }
}
