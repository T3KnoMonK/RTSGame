using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarScript : MonoBehaviour
{
    [SerializeField] private Image greenHealth;
    [SerializeField] private Image redHealth;
    private Canvas uiCanvas;

    private float maxHealth;
    private float health;

    private void Start()
    {
        if (gameObject.GetComponent<Unit>())
        {
            maxHealth = gameObject.GetComponent<Unit>().GetMaxHealth();
        }
        else if(gameObject.GetComponent<Structure>()) 
        { 
            maxHealth = gameObject.GetComponent<Structure>().GetMaxHealth();
        }
        health = maxHealth;
        uiCanvas = GetComponentInChildren<Canvas>();
        if (uiCanvas) { uiCanvas.enabled = false; }
    }

    private void OnGUI()
    {
        if (uiCanvas)
        {
            if (health < maxHealth) { uiCanvas.enabled = true; }
        }
    }

    private float GetCurrentHealthPercentage()
    {
        return health/maxHealth;
    }

    public void UpdateHealth(float currentHealth)
    {
        health = currentHealth;
    }

    public void SetUnitCanvas(bool set)
    {
        uiCanvas.enabled = set;
    }

    public void SetHealthDisplay()
    {
        greenHealth.fillAmount = GetCurrentHealthPercentage();
    }
}
