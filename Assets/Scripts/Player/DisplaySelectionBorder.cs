using UnityEngine;

public class DisplaySelectionBorder : MonoBehaviour
{
    Camera cam;
    Renderer rend;
    Vector3 extents;
    Vector3 center;

    private void Start()
    {
        cam = Camera.main;
        rend = GetComponent<Renderer>();
        center = rend.bounds.center;
        extents = rend.bounds.extents;
    }

    private Vector3[] screenSpaceCorners = new Vector3[8];
    float screenMinX, screenMinY, screenMaxX, screenMaxY;

    private void Update()
    {
        center = rend.bounds.center;

        screenSpaceCorners[0] = cam.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z - extents.z));
        screenSpaceCorners[1] = cam.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y + extents.y, center.z + extents.z));
        screenSpaceCorners[2] = cam.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z + extents.z));
        screenSpaceCorners[3] = cam.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y + extents.y, center.z - extents.z));
        screenSpaceCorners[4] = cam.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z - extents.z));
        screenSpaceCorners[5] = cam.WorldToScreenPoint(new Vector3(center.x - extents.x, center.y - extents.y, center.z + extents.z));
        screenSpaceCorners[6] = cam.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z + extents.z));
        screenSpaceCorners[7] = cam.WorldToScreenPoint(new Vector3(center.x + extents.x, center.y - extents.y, center.z - extents.z));

        screenMinX = screenMaxX = screenSpaceCorners[0].x;
        screenMinY = screenMaxY = screenSpaceCorners[0].y;

        for (int i = 1; i < 8; i++)
        {
            if (screenSpaceCorners[i].x < screenMinX)
                screenMinX = screenSpaceCorners[i].x;
            if (screenSpaceCorners[i].x > screenMaxX)
                screenMaxX = screenSpaceCorners[i].x;
            if (screenSpaceCorners[i].y < screenMinY)
                screenMinY = screenSpaceCorners[i].y;
            if (screenSpaceCorners[i].y > screenMaxY)
                screenMaxY = screenSpaceCorners[i].y;
        }
    }

    private void OnGUI()
    {
        if (GetComponent<Selectable>().IsSelected)
        {
            GUI.Box(new Rect(screenMinX, Screen.height - screenMinY, screenMaxX - screenMinX, -(screenMaxY - screenMinY)), "");
        }
    }
}
