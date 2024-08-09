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

    [SerializeField] private Image SelectedImg;
    [SerializeField] private TextMeshProUGUI SelectedStats;
    [SerializeField] private TextMeshProUGUI SelectedDescription;

    private void OnEnable()
    {
        PlayerObjects.PopulateSelectedInfoEvent += DisplayInfo;
        Resource.UpdateResourceEvent += UpdateResourceCount;
    }

    private void OnDisable()
    {
        PlayerObjects.PopulateSelectedInfoEvent -= DisplayInfo;
        Resource.UpdateResourceEvent -= UpdateResourceCount;
    }

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

    private void GetSelectedInfo(Resource obj)
    {
        currentlySelected = obj as Resource;
        SO_Resource so = currentlySelected.GetSO() as SO_Resource;
        img = so.Card;
        description = so.Description;
    }

    private enum SelectedType { UNIT, STRUCTURE, RESOURCE }
    private SelectedType _CurrentlySelectedType;

    private void DisplayInfo(Selectable obj)
    {
        if(obj == null) return;

        if (obj.GetComponent<Unit>()) { GetSelectedInfo(obj as Unit); _CurrentlySelectedType = SelectedType.UNIT; }
        else if (obj.GetComponent<Structure>()) { GetSelectedInfo(obj as Structure); _CurrentlySelectedType = SelectedType.STRUCTURE; }
        else if (obj.GetComponent<Resource>()) { GetSelectedInfo(obj as Resource); _CurrentlySelectedType = SelectedType.RESOURCE; }

        GetSelectedInfo(obj);
        SelectedImg.sprite = img;
        SelectedStats.text = UpdateStatsText();
        SelectedDescription.text = description;
    }

    private void UpdateResourceCount()
    {
        //This is just to update only the stats text when a Resource is selected
        SelectedStats!.text = UpdateStatsText();
    }

    private string UpdateStatsText()
    {
        //TODO: Make event call to update stats for Currently Selected Object to update stats in real time
        //Just health for now

        switch (_CurrentlySelectedType)
        {
            case SelectedType.UNIT:
            case SelectedType.STRUCTURE:
                return string.Format("Health: {0}/{1}", currentlySelected.GetHealth(), currentlySelected.GetMaxHealth());
            case SelectedType.RESOURCE:
                Resource tmp = currentlySelected as Resource;
                return string.Format("Resource: {0}/{1}", tmp.GetResource(), tmp.GetMaxResource());
            default:
                break;
        }

        return "Currently selected type was probably null somehow!";
    }
}

