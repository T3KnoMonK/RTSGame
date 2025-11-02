using UnityEngine;

public class SizeToGround : MonoBehaviour
{
    [SerializeField] private GameObject _Ground;
    [SerializeField] private Camera _MinimapCamera;

    private Vector3 _GroundSize;
    private int _CameraSize; //Camera is orthographic so only has a single value for the frustrum size.

    private void SizeCameraFrustrumToGroundSize()
    {
        if (_Ground.GetComponent<Terrain>())
        {
            _GroundSize = _Ground.GetComponent<Terrain>().terrainData.bounds.extents;
        }
        else if (_Ground.GetComponent<MeshFilter>())
        {
            _GroundSize = _Ground.GetComponent<MeshFilter>().mesh.bounds.extents;
            Vector3 scaledGround = new Vector3(_GroundSize.x * _Ground.transform.localScale.x, _GroundSize.y, _GroundSize.z * _Ground.transform.localScale.z);
            _GroundSize = scaledGround;
        }

        //Orthographic camera size is from center to extents so 
        //Terrain bounds are (width, hight, length) from center; i.e. (500, 12.5, 500);
        _CameraSize = (int)(_GroundSize.x > _GroundSize.z ? _GroundSize.x : _GroundSize.z);
        //Take the largest extent so the minimap always fits inside the camera

        _MinimapCamera.orthographicSize = _GroundSize.x;
        _MinimapCamera.transform.SetPositionAndRotation(new Vector3 (_CameraSize, _MinimapCamera.transform.position.y, _CameraSize), _MinimapCamera.transform.rotation);
    }

    private void Start()
    {
        SizeCameraFrustrumToGroundSize();
    }
}
