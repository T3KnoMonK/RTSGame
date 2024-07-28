using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Supply : MonoBehaviour
{
    //public delegate void ChangeSupplyDelegate(int amount);
    //public static ChangeSupplyDelegate ChangeSupplyEvent;

    [SerializeField] private int _Supply; //The amount of supply that this building adds

    public void Start()
    {
        Player.Instance.AdjustTotalSupply(_Supply);
    }

    public void OnDestroy()
    {
        Player.Instance.AdjustTotalSupply(_Supply * -1);
    }
}
