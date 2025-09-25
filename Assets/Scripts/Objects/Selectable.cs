using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool IsSelected { get; set; }

    [SerializeField] protected SO_Selectable SelectedSO;
    public SO_Selectable GetSO() { return SelectedSO; }

    protected int _MaxHealth;
    protected string _Name;
    protected string _Description;
    protected float _Health;
    protected int ID;

    public int GetID() { return ID; }
    public float GetMaxHealth() { return _MaxHealth; }
    public float GetHealth() { return _Health; }

    void Start()
    {
        _MaxHealth = SelectedSO.MaxHealth;
        _Name = SelectedSO.Name; ;
        _Description = SelectedSO.Description;

        _Health = _MaxHealth;
    }

    public void SendWaypointEnable(bool enable)
    {
        SendMessage("ToggleVisibility", enable, SendMessageOptions.DontRequireReceiver);
    }

    private Renderer rend;
    private Rect boundingBox;

    //Having trouble getting a single pixel border boundingBox, Texture method also not working for me
    public void DrawSelectionBorder()
    {
        rend = GetComponent<Renderer>();
        Vector3 worldMin = rend.bounds.min;
        Vector3 worldMax = rend.bounds.max;
        Vector2 screenMin = Camera.main.WorldToScreenPoint(worldMin);
        Vector2 screenMax = Camera.main.WorldToScreenPoint(worldMax);

        Vector2 boxPosition = new Vector2(screenMin.x, Screen.height - screenMax.y);

        boundingBox = new Rect(boxPosition, new Vector2(screenMax.x - screenMin.x, screenMax.y - screenMin.y));

        Texture2D img = Resources.Load("Art/Texture/9pixelWhiteBorder.png") as Texture2D;

        GUI.Box(boundingBox, img); // Need health bar above bounding box. Don't need object name
    }
}
