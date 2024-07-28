using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FollowCursor : MonoBehaviour
{
    //This is script is only on Structure Placeholders so is only instantiated when the player is placing a building

    private Camera _Camera;
    private Transform _ObjectTransform;

    private bool _IsFollowing;
    public void StopFollowing() { _IsFollowing = false; } //Only stops after placement and will not start following again. The player will either allow the building to be placed or will cancel it.

    private void Start()
    {
        _Camera = Camera.main;
        _ObjectTransform = gameObject.transform;
        _IsFollowing = true; 
    }

    private void Update()
    {
        if (_IsFollowing)
        {
            _ObjectTransform.SetPositionAndRotation(GetMouseGroundPosition(), _ObjectTransform.rotation);
        }
    }

    private Vector3 GetMouseGroundPosition()
    {
        //Need to go through the objects, not not cast if over them
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, float.MaxValue, 1 << LayerMask.NameToLayer("Ground")))
        {
            if (!IsPointerOverUIObject() && hit.collider.tag == "Ground")
            {
                //Debug.Log(hit.point.ToString());
                return hit.point;
            }
        }
        //Debug.Log("No mouse position found"); 
        return Vector3.zero;
    }

    private List<RaycastResult> GetPointerRaycastResult()
    {
        PointerEventData currentEventData = new PointerEventData(EventSystem.current);
        currentEventData.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> result = new List<RaycastResult>();
        EventSystem.current.RaycastAll(currentEventData, result);
        return result;
    }

    private bool IsPointerOverUIObject()
    {
        List<RaycastResult> result = GetPointerRaycastResult();
        return result.Count > 0;
    }

}
