using System;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private static CursorManager instance;
    public static CursorManager Instance {  get { return instance; } }

    private Cursor currentCursor;
    private Cursor defaultCursor;

    public enum CursorState { Default, Placeholder }
    private CursorState cursorState = CursorState.Default;
    public CursorState GetCursorState() {  return cursorState; }

    public bool IsCursorDefault() { return cursorState == CursorState.Default ? true : false; }
    public void SetCursorIsDefault() { cursorState = CursorState.Default; }

    public bool IsCursorPlaceholder() { return cursorState == CursorState.Placeholder ? true : false; }
    public void SetCursorIsPlaceholder() { cursorState = CursorState.Placeholder; }

    public Vector3 GetPlaceholderPos() { return _PlaceholderObj.transform.position!; }

    private GameObject _PlaceholderObj;
    public GameObject GetPlaceholder() {  return _PlaceholderObj; }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private void OnEnable()
    {
        Unit.PlacedBuildingEvent += SetBuilding;
        Unit.CancelBuildEvent += CancelBuild;
        Unit.StartBuildEvent += SetPlaceholder;
    }

    private void OnDisable()
    {
        Unit.PlacedBuildingEvent -= SetBuilding;
        Unit.CancelBuildEvent -= CancelBuild;
        Unit.StartBuildEvent -= SetPlaceholder;
    }

    private void SetPlaceholder(GameObject placeholder)
    {
        if (_PlaceholderObj != null) { DestroyPlaceholder(); } //Need this so the current placeholder does not exist erroneously if you set the placeholder (left click while it's active) then start another place building action.
        _PlaceholderObj = placeholder;
        SetCursorIsPlaceholder();
    }

    private void CancelBuild()
    {
        DestroyPlaceholder();
        SetCursorIsDefault();
    }

    private void DestroyPlaceholder()
    {
        if(_PlaceholderObj != null) { Destroy(_PlaceholderObj); }
    }

    private void SetBuilding()
    {
        DestroyPlaceholder();
        SetCursorIsDefault();
    }
}
