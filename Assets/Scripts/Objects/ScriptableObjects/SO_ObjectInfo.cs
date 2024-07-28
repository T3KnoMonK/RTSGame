using UnityEngine;

[CreateAssetMenu(menuName = "Units/UnitInfo", fileName = "New Unit Info")]
public class SO_ObjectInfo : ScriptableObject
{
    public Sprite UnitImage;
    public int UnitMaxHealth;
    public int UnitMaxEnergy;
    public string UnitName;
    public string UnitDescription;
    public string OwningPlayer;
}
