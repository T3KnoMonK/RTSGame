using UnityEngine;

public interface IClickContext
{
    void OnRightClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift);
    void OnLeftClickUp(RaycastHit hitObj, Vector3 mousePos, bool isShift);
    //void OnRightClickDown(RaycastHit hitObj, Vector3 mousePos, bool isShift);
    //void OnLeftClickDown(RaycastHit hitObj, Vector3 mousePos, bool isShift);
}
