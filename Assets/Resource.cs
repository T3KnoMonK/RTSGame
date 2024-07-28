using UnityEngine;

public class Resource : Selectable
{
    private SO_Resource resourceSO;

    private int Resources;
    private int MaxResources;
    
    private void Start()
    {
        resourceSO = SelectedSO as SO_Resource;
        MaxResources = resourceSO.MaxResources;
        Resources = resourceSO.MaxResources;
    }

    public void AdjustResources(int amount){
        Resources += amount;
        if(Resources < 0) { Resources = 0; }
    }
}
