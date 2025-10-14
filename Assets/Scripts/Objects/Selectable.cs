using System.Collections.Generic;
using Unity.Multiplayer.Center.Common;
using UnityEngine;

public class Selectable : MonoBehaviour
{
    public bool IsSelected { get; set; }

    [SerializeField] protected SO_Selectable SelectedSO;
    public SO_Selectable GetSO() { return SelectedSO; }

    protected float _Health;
    protected int ID;

    public int GetID() { return ID; }
    public float GetMaxHealth() { return SelectedSO.MaxHealth; }
    public float GetHealth() { return _Health; }

    [SerializeField] protected List<SO_Action> _ActionData = new List<SO_Action>();

    protected List<Action> _Actions = new List<Action>();
    public List<Action> GetActions() { return _Actions; }

    void Awake()
    {
        _Health = SelectedSO.MaxHealth;
        GenereateActions();
        //if(_ActionData != null && _ActionData.Count > 0)
        //{
        //    foreach (SO_Action action in _ActionData)
        //    {
        //        _Actions.Add(new Action(action, gameObject));
        //    }
        //}
    }

    private void Update()
    {
        if (_Actions != null && _Actions.Count > 0)
        {
            foreach (Action action in _Actions)
            {
                action.UpdateCooldown();
            }
        }
    }

    private void GenereateActions()
    {
        if (_ActionData == null) { return; }
        if (_ActionData.Count > 0)
        {
            if (_Actions == null) { _Actions = new List<Action>(); }

            foreach (SO_Action action in _ActionData)
            {
                _Actions.Add(new Action(action, gameObject));
            }
        }
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
