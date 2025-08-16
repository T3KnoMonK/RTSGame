using UnityEngine;

[CreateAssetMenu(menuName = "Actions/New Place Building", fileName = "New Place Building")]
public class PlaceBuildingAction : Action
{
    [SerializeField] private GameObject Placeholder; //Should be a transparent mesh with no collision
    [SerializeField] private GameObject Building; //Should be the actual building prefab

    public override void DoAction() 
    {
        base.DoAction();
        if (Cost > Player.Instance.GetCurrentResource()) //This return will need to be in every action that has a resource cost as this was easier than trying to put it in Action.DoAction();
            return;
        Parent.GetComponent<Unit>().SetActionCaller(this); 
        Parent.GetComponent<Unit>().SetPlayerBuildingPlaceholder(Placeholder, Building);
        AttachedButton.GetComponent<ActionCooldown>().TriggerCooldown();
    }
}
