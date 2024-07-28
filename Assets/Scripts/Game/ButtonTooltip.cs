using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonTooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private string _TooltipText;
    private TooltipPanel _TooltipPanel;

    private void Start()
    {
        //_TooltipPanel = GameObject.FindGameObjectWithTag("Tooltip").GetComponent<TooltipPanel>();
    }

    public void SetTooltipText(string text) { _TooltipText = text; }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //Debug.Log("Pointer entered " + gameObject.name);
        //_TooltipPanel.ShowTooltip(_TooltipText);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        //Debug.Log("Pointer left " + gameObject.name);
        //_TooltipPanel.HideTooltip();
    }
}
