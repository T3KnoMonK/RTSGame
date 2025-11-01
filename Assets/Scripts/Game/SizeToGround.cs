using UnityEngine;

public class SizeToGround : MonoBehaviour
{
    [SerializeField] private GameObject _Ground;
    [SerializeField] private Camera _MinimapCamera;

    private Vector3 _GroundSize;
    private int _CameraSize; //Camera is orthographic so only has a single value for the frustrum size.

    private void SizeCameraFrustrumToGroundSize()
    {
        _GroundSize = _Ground.GetComponent<Terrain>().terrainData.bounds.extents;
        //Orthographic camera size is from center to extents so 
        //Terrain bounds are (width, hight, length) from center; i.e. (500, 12.5, 500);
        Debug.Log($"Ground size is: {_GroundSize}");
        _CameraSize = (int)(_GroundSize.x > _GroundSize.z ? _GroundSize.x : _GroundSize.z);
        //Take the largest extent so the minimap always fits inside the camera
        _MinimapCamera.orthographicSize = _GroundSize.x;
        _MinimapCamera.transform.SetPositionAndRotation(
            new Vector3
                (_CameraSize, _MinimapCamera.transform.position.y, _CameraSize), 
                _MinimapCamera.transform.rotation);
    }

    private void Start()
    {
        SizeCameraFrustrumToGroundSize();
    }
}
