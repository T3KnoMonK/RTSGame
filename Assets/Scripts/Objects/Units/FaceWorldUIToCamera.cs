using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceWorldUIToCamera : MonoBehaviour
{
    private Camera mainCam;
    private Canvas uiCanvas;

    private void Start()
    {
        mainCam = Camera.main;
        uiCanvas = GetComponent<Canvas>();
    }

    private void FaceUIToCamera()
    {
        uiCanvas.transform.LookAt(mainCam.transform);
    }

    private void OnGUI()
    {
        //FaceUIToCamera();
    }
}
