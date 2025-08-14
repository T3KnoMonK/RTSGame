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
        Parent.GetComponent<Unit>().SetActionCaller(this); //Passing reference to calling action because the event chain is passed to the player.
        Parent.GetComponent<Unit>().SetPlayerBuildingPlaceholder(Placeholder, Building); //Just gives the prefabs to the Player object because it controls event flow from here.
    }
}
