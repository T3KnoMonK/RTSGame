using UnityEngine.EventSystems;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveToMinimapClick : MonoBehaviour
{
    private GraphicRaycaster _GRay;
    private PointerEventData _PointerData;
    private EventSystem _EventSystem;
    private List<RaycastResult> _Result;

    private Camera _MinimapCam;
    private Camera _MainCam;

    private void Start()
    {
        _GRay = GetComponent<GraphicRaycaster>();
        _EventSystem = GetComponent<EventSystem>();
        _MinimapCam = GameObject.FindGameObjectWithTag("MiniMapCam").GetComponent<Camera>();
        _MainCam = Camera.main;
    }

    private void OnEnable()
    {
        InputManager.LeftClickUpEvent += GetMinimapPositionOnClick;
    }

    private void OnDisable()
    {
        InputManager.LeftClickUpEvent -= GetMinimapPositionOnClick;
    }

    private void GetMinimapPositionOnClick(RaycastHit target, Vector3 mousePosmousePos, bool shift)
    {

        _PointerData = new PointerEventData(_EventSystem);
        _PointerData.position = Input.mousePosition;

        _Result = new List<RaycastResult>();

        _GRay.Raycast(_PointerData, _Result);

        foreach (RaycastResult result in _Result)
        {
            if (result.gameObject.name == "MinimapRender")
            {
                RectTransform rect = result.gameObject.GetComponent<RectTransform>();

                Vector2 localMousePos;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(rect, _PointerData.position, _PointerData.enterEventCamera, out localMousePos);

                Vector2 viewportPos = _MinimapCam.ScreenToViewportPoint(localMousePos);

                float xRatio = localMousePos.x / rect.rect.width;
                float yRatio = localMousePos.y / rect.rect.height;

                Debug.Log("Minimap position ratio: " + xRatio + ", " + yRatio);

                float newViewportX = _MinimapCam.pixelWidth * xRatio;
                float newViewportY = _MinimapCam.pixelHeight * yRatio;

                Debug.Log("Camera position ratio: " + newViewportX + ", " + newViewportY);

                _MainCam.transform.position = new Vector3(newViewportX*2, _MainCam.transform.position.y, (newViewportY*2)-50.0f);
            }
        }
    }
}


/*
                Debug.Log("Hit :"+result.gameObject.name);
                Debug.Log("Pointer position: " + _PointerData.position);
                Debug.Log("Minimap bounds: " + result.gameObject.GetComponent<RectTransform>().rect.min.ToString() + "; " + result.gameObject.GetComponent<RectTransform>().rect.max.ToString());
                Debug.Log("Raw image: " + result.gameObject.GetComponent<RawImage>().texture.ToString());

                Renderer rend = result.gameObject.transform.GetComponent<Renderer>();
                MeshCollider meshCollider = result.gameObject.GetComponent<Collider>() as MeshCollider;

                if (rend == null || rend.sharedMaterial == null || rend.sharedMaterial.mainTexture == null || meshCollider == null)
                {
                    Debug.Log("Exited minimap click");
                    return;
                }

                Texture2D tex = rend.material.mainTexture as Texture2D;
                Vector2 pixelUV = target.textureCoord;
                pixelUV.x *= tex.width;
                pixelUV.y *= tex.height;

                Debug.Log("Pixel UV: " + pixelUV.ToString());

                tex.SetPixel((int)pixelUV.x, (int)pixelUV.y, Color.black);
                tex.Apply();
*/