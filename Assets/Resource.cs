using System.Collections.Generic;
using UnityEngine;

public class Resource : Selectable
{
    public delegate void UpdateResourceDelegate();
    public static event UpdateResourceDelegate UpdateResourceEvent;

    private List<Unit> _WorkerQ = new List<Unit>();
    [SerializeField] private int _MaxWorkerQLen = 5;

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

    private void Update()
    {
        //Debug.Log("Worker gathering: " + _IsWorkerGathering);
        if(_IsWorkerGathering) return;
        else if (!_IsWorkerGathering)
        {
            if(_WorkerQ.Count > 0)
            {
                StartNextWorkerGathering();
            }
        }
    }

    public void AdjustResources(int amount){
        Resources += amount;
        if(Resources < 0) { Resources = 0; }
        UpdateResourceEvent?.Invoke();
        //if (IsSelected) { Debug.Log("Resource is selected and has " + Resources + " resources."); }
    }

    public Unit PopFromQ() {
        if (_WorkerQ[0] != null)
        {
            Unit tmp = _WorkerQ[0];
            //tmp.Appear();
            Debug.Log("Popping " + tmp.name);
            _WorkerQ.RemoveAt(0);
            Debug.Log("Workers in Queue: " + PrintAllWorkers(_WorkerQ));
            _IsWorkerGathering = false;
            //StartNextWorkerGathering(); //Shouldn't need this here if _IsWorkerGathering flag is being set correctly
            return tmp;
        }
        return null;
    }

    private bool _IsWorkerGathering = false;

    public void AddToQ(Unit unit)
    {
        Debug.Log("Adding " + unit.name);
        _WorkerQ.Add(unit);
        Debug.Log("Workers in Queue: " + PrintAllWorkers(_WorkerQ));
    }

    public void StartNextWorkerGathering()
    {
        if (_WorkerQ[0] != null)
        {
            _WorkerQ[0].SendMessage ("ResumeGathering", SendMessageOptions.RequireReceiver);
            _IsWorkerGathering=true;
        }
    }

    private string PrintAllWorkers(List<Unit> list)
    {
        string names = "";
        foreach (Unit u in list) { names += u.name; }
        return names;
    }

    public bool IsQFull()
    {
        return _WorkerQ.Count == _MaxWorkerQLen;
    }

}
