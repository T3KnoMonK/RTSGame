using UnityEngine;

public class InputManager : MonoBehaviour
{
    public delegate void OnRightClickUpDelegate(RaycastHit target, Vector3 mouseWorldPos, bool shift);
    public static OnRightClickUpDelegate RightClickUpEvent;

    public delegate void OnLeftClickUpDelegate(RaycastHit target, Vector3 mouseWorldPos, bool shift);
    public static OnLeftClickUpDelegate LeftClickUpEvent;

    public delegate void OnRightClickDownDelegate(RaycastHit target, Vector3 mouseWorldPos, bool shift);
    public static OnRightClickDownDelegate RightClickDownEvent;

    public delegate void OnLeftClickDownDelegate(RaycastHit target, Vector3 mouseWorldPos, bool shift);
    public static OnLeftClickDownDelegate LeftClickDownEvent;

    private static InputManager instance;
    public static InputManager Instance { get { return instance; } }

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    private Camera mainCam;

    private void Start()
    {
        mainCam = Camera.main;
        bool y = Input.GetKey(KeyCode.LeftShift);
    }

    private void OnRightClickUp()
    {
        RightClickUpEvent?.Invoke(GetRayHitObj(), Camera.main.ScreenToWorldPoint(Input.mousePosition), Input.GetKey(KeyCode.LeftShift));
    }

    private void OnLeftClickUp()
    {
        LeftClickUpEvent?.Invoke(GetRayHitObj(), Camera.main.ScreenToWorldPoint(Input.mousePosition), Input.GetKey(KeyCode.LeftShift));
    }


    private void OnRightClickDown()
    {
        RightClickDownEvent?.Invoke(GetRayHitObj(), Camera.main.ScreenToWorldPoint(Input.mousePosition), Input.GetKey(KeyCode.LeftShift));
    }

    private void OnLeftClickDown()
    {
        LeftClickDownEvent?.Invoke(GetRayHitObj(), Camera.main.ScreenToWorldPoint(Input.mousePosition), Input.GetKey(KeyCode.LeftShift));
    }

    private void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            OnLeftClickUp();
        }

        if (Input.GetMouseButtonUp(1))
        {
            OnRightClickUp();
        }

        if (Input.GetMouseButtonDown(0))
        {
            OnLeftClickDown();
        }

        if (Input.GetMouseButtonDown(1))
        {
            OnRightClickDown();
        }
    }

    private RaycastHit GetRayHitObj()
    {
        RaycastHit hit;
        Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            return hit;
        }
        return default;
    }
}
