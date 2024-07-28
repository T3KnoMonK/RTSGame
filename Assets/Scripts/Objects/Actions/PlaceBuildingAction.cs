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
        Player.Instance.SetActionCaller(this); //Passing reference to calling action because the event chain is passed to the player.
        Player.Instance.SetPlayerBuildingPlaceholder(Placeholder, Building); //Just gives the prefabs to the Player object because it controls event flow from here.
    }

    //private void OnEnable()
    //{

    //    if (!IsSubscribed())
    //    {
    //        Player.PlacedBuildingEvent += PayActionCost;
    //    }
    //}

    //private void OnDisable()
    //{
    //    Player.PlacedBuildingEvent -= PayActionCost;
    //}

    //private bool IsSubscribed()
    //{
    //    if (Player.PlacedBuildingEvent == null) { return false; }
    //    System.Delegate[] d = Player.PlacedBuildingEvent.GetInvocationList();
    //    if (d.Length == 0) { return false; }
    //    for (int i = 0; i < d.Length; i++)
    //    {
    //        if (d[i].Method.Name == "PayActionCost") { return true; }
    //    }
    //    return false;
    //}


}
