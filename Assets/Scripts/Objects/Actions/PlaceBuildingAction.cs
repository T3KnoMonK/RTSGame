using UnityEngine;

[CreateAssetMenu(menuName = "Actions/New Place Building", fileName = "New Place Building")]
public class PlaceBuildingAction : SO_Action
{
    [SerializeField] private GameObject Placeholder; //Should be a transparent mesh with no collision
    [SerializeField] private GameObject Building; //Should be the actual building prefab

    //Target is the calling object that needs to hold the placeholder and building
    public override int DoAction(GameObject target) 
    {
        if (Cost > Player.Instance.GetCurrentResource())
            return 0;

        target.GetComponent<Unit>().StartBuild(Placeholder, Building);
        //Debug.Log("Place Buildling action called");
        return 1;
    }
}