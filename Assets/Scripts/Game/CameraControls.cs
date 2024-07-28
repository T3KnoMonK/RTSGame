using UnityEngine;

public class CameraControls : MonoBehaviour
{
    private Camera mainCam;
    [SerializeField] private Vector3 cameraStartingAngle = new Vector3(0.0f, 0.0f, 0.0f);

    float CameraPanMovementScale = 1.0f;
    float CameraZoomMovementScale = 0.5f;

    private void Start()
    {
        mainCam = Camera.main;
        mainCam.transform.rotation = Quaternion.Euler(cameraStartingAngle);
    }

    private void Update()
    {
        CameraControlInput();
    }

    private void CameraControlInput()
    {
        //Keyboard Controls

        //Camera Pan
        if (Input.GetKey(KeyCode.DownArrow))
        {
            if (transform.position.y >= MapManager.Instance.GetMapMinBounds().y) { mainCam.transform.position += Vector3.back * CameraPanMovementScale; }
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            if (transform.position.x <= MapManager.Instance.GetMapMaxBounds().x) { mainCam.transform.position += Vector3.right * CameraPanMovementScale; }
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            if (transform.position.x >= MapManager.Instance.GetMapMinBounds().x) { mainCam.transform.position += Vector3.left * CameraPanMovementScale; }
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if (transform.position.y <= MapManager.Instance.GetMapMaxBounds().y) { mainCam.transform.position += Vector3.forward * CameraPanMovementScale; }
        }

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
            if (transform.position.y >=40) { mainCam.transform.position += Vector3.down * CameraZoomMovementScale; }
        }
        if (Input.mouseScrollDelta.y < 0.0f)
        {
            if (transform.position.y <=100) { mainCam.transform.position += Vector3.up * CameraZoomMovementScale; }
        }
    }
}
