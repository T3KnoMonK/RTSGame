using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;
using UnityEngine.UI;
using TMPro;

public class DisplayUnitInfo : MonoBehaviour
{
    //private Unit currentSelectedUnit;
    private Selectable currentlySelected;
    //private SO_Unit info;
    private SO_Selectable _info;
    private Sprite img;
    private string description;
    private string stats;

    [SerializeField] private Image UnitImage;
    [SerializeField] private TextMeshProUGUI UnitStats;
    [SerializeField] private TextMeshProUGUI UnitDescription;

    private void OnEnable()
    {
        PlayerObjects.PopulateSelectedInfoEvent += DisplayInfo;
    }

    private void OnDisable()
    {
        PlayerObjects.PopulateSelectedInfoEvent -= DisplayInfo;
    }

    //private void GetUnitInfo(Selectable obj)
    //{
    //    currentSelectedUnit = obj as Unit;
    //    info = currentSelectedUnit.unitSO;
    //    img = info.unitCard;
    //    description = info.unitDescription;
    //}

    private void GetSelectedInfo(Selectable obj)
    {
        currentlySelected = obj;
        _info = currentlySelected.GetSO();
        img = _info.Card;
        description = _info.Description;
    }

    private void GetSelectedInfo(Unit obj)
    {
        currentlySelected = obj as Unit;
        SO_Unit so = currentlySelected.GetSO() as SO_Unit;
        img = so.Card;
        description = so.Description;
    }

    private void GetSelectedInfo(Structure obj)
    {
        currentlySelected = obj as Structure;
        SO_Structure so = currentlySelected.GetSO() as SO_Structure;
        img = so.Card;
        description = so.Description;
    }

    private void DisplayInfo(Selectable obj)
    {
        if(obj == null) return;

        if (obj.GetComponent<Unit>()) { GetSelectedInfo(obj as Unit); }
        else if (obj.GetComponent<Structure>()) { GetSelectedInfo(obj as Structure); }

        GetSelectedInfo(obj);
        UnitImage.sprite = img;
        UnitStats.text = UpdateUnitStatsText();
        UnitDescription.text = description;
    }

    private string UpdateUnitStatsText()
    {
        //Just health for now
        return string.Format("Health: {0}/{1}", currentlySelected.GetHealth(), currentlySelected.GetMaxHealth());
    }
}

