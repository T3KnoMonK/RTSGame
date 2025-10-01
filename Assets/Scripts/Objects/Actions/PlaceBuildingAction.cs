using UnityEngine;

[CreateAssetMenu(menuName = "Actions/New Place Building", fileName = "New Place Building")]
public class PlaceBuildingAction : SO_Action
{
    [SerializeField] private GameObject Placeholder; //Should be a transparent mesh with no collision
    [SerializeField] private GameObject Building; //Should be the actual building prefab
    

    public override void DoAction() 
    {
        if (IsOnCooldown) { return; }
        base.DoAction();
        if (Cost > Player.Instance.GetCurrentResource()) //This return will need to be in every action that has a resource cost as this was easier than trying to put it in Action.DoAction();
            return;
        _ActionOwner.GetComponent<Unit>().SetActionCaller(this); 
        _ActionOwner.GetComponent<Unit>().SetPlayerBuildingPlaceholder(Placeholder, Building);
        StartCooldown();
    }
}
