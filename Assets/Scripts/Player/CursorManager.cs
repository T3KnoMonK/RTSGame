using System;
using System.Collections.Generic;
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

    private Dictionary<int, GameObject> _PlaceholderList = new Dictionary<int, GameObject>();

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

    public void SetPlaceholder(GameObject placeholder, int WorkerID)
    {
        //There should only be one placeholder associated with any worker at any given time. That single placeholder gets destroyed and removed before associating a new placeholder to that worker
        if (_PlaceholderList.Count > 0)
        {
            DestroyPlaceholder(WorkerID); //Need this so the current placeholder does not exist erroneously if you set the placeholder (left click while it's active) then start another place building action.
        }
        _PlaceholderList.Add(WorkerID, placeholder);
        SetCursorIsPlaceholder();
    }

    public void CancelBuild(int WorkerID)
    {
        DestroyPlaceholder(WorkerID);
        SetCursorIsDefault();
    }

    public void SetBuilding(int WorkerID)
    {
        DestroyPlaceholder(WorkerID);
        SetCursorIsDefault();
    }

    private void DestroyPlaceholder(int WorkerID)
    {
        if (_PlaceholderList.Count == 0) { return; }

        if (_PlaceholderList.ContainsKey(WorkerID))
        {
            Destroy(_PlaceholderList[WorkerID]);
            _PlaceholderList.Remove(WorkerID);
        }
    }

    public GameObject GetPlaceHolderForWorker(int workerID)
    {
        if (_PlaceholderList.Count == 0) { return null; }

        return _PlaceholderList.ContainsKey(workerID) ? _PlaceholderList[workerID] : null;
    }
}
