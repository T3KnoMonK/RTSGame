using UnityEngine;

[CreateAssetMenu(menuName = "Actions/New Action", fileName = "New Action")]
public class SO_Action : ScriptableObject
{
    public Sprite Image;
    public string Name;
    public string Description;
    public int Cost;
    public float ReadyTime;
    public float CooldownTime;

    public enum ActionType { Immediate, Placement }
    public ActionType Type;

    public virtual void DoAction(GameObject target) { }
}
