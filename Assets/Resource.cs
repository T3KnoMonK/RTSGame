using UnityEngine;

public class Resource : Selectable
{
    public delegate void UpdateResourceDelegate();
    public static event UpdateResourceDelegate UpdateResourceEvent;


    private SO_Resource resourceSO;

    private int Resources;
    private int MaxResources;

    public int GetMaxResource() {  return MaxResources; }
    public int GetResource() { return Resources;}
    
    private void Start()
    {
        resourceSO = SelectedSO as SO_Resource;
        MaxResources = resourceSO.MaxResources;
        Resources = resourceSO.MaxResources;
    }

    public void AdjustResources(int amount){
        Resources += amount;
        if(Resources < 0) { Resources = 0; }
        //Update details panel if resource is currently selected
        UpdateResourceEvent?.Invoke();
        if (IsSelected) { Debug.Log("Resource is selected and has " + Resources + " resources."); }
    }
}
