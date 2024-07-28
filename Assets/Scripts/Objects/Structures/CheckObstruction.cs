using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Assertions;

public class CheckObstruction : MonoBehaviour
{
    private BoxCollider _Collider;
    private Material _StartingMaterial;
    private bool _IsObstructed;
    public bool IsObsructed() { return  _IsObstructed; }
    private List<GameObject> _Obstructions = new List<GameObject>();
    private List<Material> _ModelMats = new List<Material>();
    [SerializeField] private Color _UnubstructedColour;
    [SerializeField] private Color _ObstructedColour;


    private void Start()
    {
        _ModelMats.Clear();
        _ModelMats = GetPlaceholderMaterials();
        _Collider = GetComponent<BoxCollider>();
    }

    private List<Material> GetPlaceholderMaterials()
    {
        Transform[] tmp = GetComponentsInChildren<Transform>();
        List<Material> mats = new List<Material>();
        foreach (Transform t in tmp)
        { 
            if (t.GetComponent<MeshRenderer>() != null) 
            { 
                mats.Add(t.GetComponent<MeshRenderer>().material); 
            } 
        }
        return mats;
    }

    private void SetModelMatsColour(Color colour)
    {
        foreach(Material m in _ModelMats)
        {
            m.color = colour;
        }
    }

    private void PrintObstructions()
    {
        if (_IsObstructed)
        {
            string obs = "Current Obstructions: ";
            foreach (var obstruction in _Obstructions)
            {
                obs += obstruction.name + "; ";
            }
            Debug.LogWarning(obs);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Ground")
        {
            _Obstructions.Add(other.gameObject);
            SetModelMatsColour(_ObstructedColour);
            _IsObstructed = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (_Obstructions.Find(x => other.gameObject))
        {
            _Obstructions.Remove(other.gameObject);
        }
        if (_Obstructions.Count <= 0)
        {
            SetModelMatsColour(_UnubstructedColour);
            _IsObstructed = false;
        }
    }

}
