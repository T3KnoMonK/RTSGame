using UnityEngine;

[CreateAssetMenu(menuName = "Resources/Resource", fileName = "New Resource")]
public class SO_Resource : SO_Selectable
{
    public enum ResourceType { Money, Energy }
    public ResourceType Type;
    public int Resources;
    public int MaxResources;

}
