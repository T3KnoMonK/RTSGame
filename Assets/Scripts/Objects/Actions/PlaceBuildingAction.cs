using UnityEngine;

[CreateAssetMenu(menuName = "Actions/New Place Building", fileName = "New Place Building")]
public class PlaceBuildingAction : SO_Action
{
    [SerializeField] private GameObject Placeholder; //Should be a transparent mesh with no collision
    [SerializeField] private GameObject Building; //Should be the actual building prefab

    //Target is the calling object that needs to hold the placeholder and building
    public override void DoAction(GameObject target) 
    {
        if (Cost > Player.Instance.GetCurrentResource())
            return;
        
        target.GetComponent<Unit>().SetPlayerBuildingPlaceholder(Placeholder, Building);
        target.GetComponent<Unit>().IsActivePlaceholder(true);
    }
}