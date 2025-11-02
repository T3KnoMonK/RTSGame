using UnityEngine;

public class CameraControls : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private Vector3 cameraStartingAngle = new Vector3(0.0f, 0.0f, 0.0f);

    [SerializeField] private Camera _MinimapCamera;
    [SerializeField] private GameObject _Ground;

    private Vector3 _GroundMin;
    private Vector3 _GroundMax;

    float CameraPanMovementScale = 1.0f;
    float CameraZoomMovementScale = 0.5f;

    private void Start()
    {
        mainCam = Camera.main;
        mainCam.transform.rotation = Quaternion.Euler(cameraStartingAngle);

        if (_Ground.GetComponent<Terrain>())
        {
            _GroundMin = _Ground.GetComponent<Terrain>().terrainData.bounds.min;
            _GroundMax = _Ground.GetComponent<Terrain>().terrainData.bounds.max;
        }
        else if (_Ground.GetComponent<MeshFilter>())
        {
            _GroundMin = _Ground.GetComponent<MeshFilter>().mesh.bounds.min;
            _GroundMax = _Ground.GetComponent<MeshFilter>().mesh.bounds.max;
        }
    }

    private void Update()
    {
        CameraControlInput();
    }

    private void CameraControlInput()
    {
        //Keyboard Controls

        //Camera Pan
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            mainCam.transform.position += Vector3.back * CameraPanMovementScale;
        }

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            mainCam.transform.position += Vector3.right * CameraPanMovementScale;
        }

        if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            mainCam.transform.position += Vector3.left * CameraPanMovementScale;
        }

        if (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W))
        {
            mainCam.transform.position += Vector3.forward * CameraPanMovementScale;
        }

        //TODO: Camera move constraints. If Cam.pos.min < map.min Cam.pos.min = map.min; Ditto for max

        //Mouse Controls

        //Camera Pan
        if (Input.GetMouseButton(2))
        {
            float H = Input.GetAxis("Mouse X");
            float V = Input.GetAxis("Mouse Y");

            if (H > 0.0f || H < 0.0f) { mainCam.transform.position += Vector3.left * H * CameraPanMovementScale; }
            if (V > 0.0f || V < 0.0f) { mainCam.transform.position += Vector3.back * V * CameraPanMovementScale; }
        }

        //Camera Zoom
        if (Input.mouseScrollDelta.y > 0.0f)
        {
            if (transform.position.y >= 40) { mainCam.transform.position += Vector3.down * CameraZoomMovementScale; }
        }
        if (Input.mouseScrollDelta.y < 0.0f)
        {
            if (transform.position.y <= 100) { mainCam.transform.position += Vector3.up * CameraZoomMovementScale; }
        }

        ////mainCam offsets x(0), z(-50)
        //Vector3 camOffset = new Vector3(0, 0, -50); //Determined through visual testing while running

        ////Keep Camera in map bounds
        //Vector3 camPosFix = new Vector3();

        //camPosFix.y = mainCam.transform.position.y;

        //if (mainCam.transform.position.x < _GroundMin.x)
        //{
        //    camPosFix.x = _GroundMin.x;
        //}

        //if (mainCam.transform.position.z < _GroundMin.z)
        //{
        //    camPosFix.z = _GroundMin.z;
        //}

        //if(mainCam.transform.position.x > _GroundMax.x)
        //{
        //    camPosFix.x = _GroundMax.x;
        //}

        //if(mainCam.transform.position.z > _GroundMax.z)
        //{
        //    camPosFix.z = _GroundMax.z;
        //}

        //mainCam.transform.position = camPosFix+camOffset;
    }
}
