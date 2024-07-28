using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class TooltipPanel : MonoBehaviour
{
    private string _TooltipText;

    private void Start()
    {
        _TooltipText = GetComponentInChildren<Text>().text;
    }

    public void ShowTooltip(string text)
    {
        _TooltipText = text;
        gameObject.SetActive(true);
    }

    public void HideTooltip()
    {
        _TooltipText = "";
        gameObject.SetActive(false);
    }
}
