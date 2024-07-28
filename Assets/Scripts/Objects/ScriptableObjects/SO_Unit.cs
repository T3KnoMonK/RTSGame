using UnityEngine;


[CreateAssetMenu(fileName = "New Unit", menuName = "Units/Unit")]
public class SO_Unit : SO_Selectable
{
    //Unit Prefab
    public GameObject unit;

    //Unit Stats
    public float speed;
    public float damage;
    public float attackDistance;
    public float attackSpeed;
    public int maxCargo;
    public int gatherRate;
    public int supplyCost;

    //Unit Type
    public UnitType unitType;
    public enum UnitType { Worker, Assault, Tank };

    //Appearance
    public Material unitSkin;

}
