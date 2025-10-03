using UnityEngine;
using System.Collections.Generic;
using System;
using System.Net.NetworkInformation;
using UnityEngine.UI;

public class ActionContainer : MonoBehaviour
{
    [SerializeField] private List<SO_Action> _ActionData;
    public List<SO_Action> GetActionData() {  return _ActionData; }

    private Camera _MainCam;
    private Camera _MinimapCamera;
    private Texture _MinimapTexture;

    private void Start()
    {
        _MainCam = Camera.main;
        _MinimapCamera = GameObject.Find("MiniMapCamera").GetComponent<Camera>();
        _MinimapTexture = GameObject.Find("MinimapRender").GetComponent<RawImage>().texture;



        foreach (SO_Action action in GetActionData())
        {
            action.SetOwner(gameObject);
        }
    }

    private void Update()
    {
        foreach(SO_Action action in _ActionData)
        {
            if (action.IsOnCooldown)
            {
                action.CurrentCooldown -= Time.deltaTime;
                Debug.Log($"{action.name} is cooling down: {action.CurrentCooldown} left.");
                if (gameObject.GetComponent<Selectable>().IsSelected)
                {
                    action.GetButton().GetComponent<FillMaskHelper>().GetFillMask().fillAmount = action.CurrentCooldown / action.CooldownTime;
                }
                if(action.CurrentCooldown <= 0.0f)
                {
                    Debug.Log($"{action.name} is off cooldown;");
                    action.CurrentCooldown = 0.0f;
                    action.IsOnCooldown = false;
                    if (action.GetButton()) { action.GetButton().GetComponent<FillMaskHelper>().GetFillMask().fillAmount = 0.0f; }
                }
            }
        }

        GetMinimapViewboxPoints();
    }

    private void GetMinimapViewboxPoints()
    {
        RaycastHit botLeftHit;
        RaycastHit botRightHit;
        RaycastHit topLeftHit;
        RaycastHit topRightHit;

        //Bottom Left
        if (Physics.Raycast(_MainCam.ViewportPointToRay(new Vector3(0, 0, _MainCam.nearClipPlane)), out botLeftHit))
        {
            Debug.Log($"Bottom left viewport world coordinates: {botLeftHit.point}");
        }

        //Bottom Right
        if (Physics.Raycast(_MainCam.ViewportPointToRay(new Vector3(1, 0, _MainCam.nearClipPlane)), out botRightHit))
        {
            Debug.Log($"Bottom right viewport world coordinates: {botLeftHit.point}");
        }

        //Top Left
        if (Physics.Raycast(_MainCam.ViewportPointToRay(new Vector3(0, 1, _MainCam.nearClipPlane)), out topLeftHit))
        {
            Debug.Log($"Top left viewport world coordinates: {botLeftHit.point}");
        }

        //Top Right
        if (Physics.Raycast(_MainCam.ViewportPointToRay(new Vector3(1, 1, _MainCam.nearClipPlane)), out topRightHit))
        {
            Debug.Log($"Top right viewport world coordinates: {botLeftHit.point}");
        }

        /*********** Minimap Camera **************/

        //TODO: Need this eventually. Not priority at the moment.

        //RaycastHit botLeftMinimapHit;
        //RaycastHit botRightMinimapHit;
        //RaycastHit topLeftMinimapHit;
        //RaycastHit topRightMinimapHit;

        ////Bottom Left
        //if (Physics.Raycast(_MinimapCamera.ViewportPointToRay(new Vector3(0, 0, _MinimapCamera.nearClipPlane)), out botLeftMinimapHit))
        //{
        //    Debug.Log($"Bottom left Minimap viewport world coordinates: {botLeftMinimapHit.point}");
        //    Debug.LogWarning("1");
        //}
        //else { Debug.LogWarning("No 1"); }

        ////Bottom Right
        //if (Physics.Raycast(_MinimapCamera.ViewportPointToRay(new Vector3(1, 0, _MinimapCamera.nearClipPlane)), out botRightMinimapHit))
        //{
        //    Debug.LogWarning("2");
        //    Debug.Log($"Bottom right Minimap viewport world coordinates: {botRightMinimapHit.point}");
        //}
        //else { Debug.LogWarning("No 2"); }


        ////Top Left
        //if (Physics.Raycast(_MinimapCamera.ViewportPointToRay(new Vector3(0, 1, _MinimapCamera.nearClipPlane)), out topLeftMinimapHit))
        //{
        //    Debug.LogWarning("3");
        //    Debug.Log($"Top Left Minimap viewport world coordinates: {topLeftMinimapHit.point}");
        //}
        //else { Debug.LogWarning("No 3"); }


        ////Top Right
        //if (Physics.Raycast(_MinimapCamera.ViewportPointToRay(new Vector3(1, 1, _MinimapCamera.nearClipPlane)), out topRightMinimapHit))
        //{
        //    Debug.LogWarning("4");
        //    Debug.Log($"Top Right Minimap viewport world coordinates: {topRightMinimapHit.point}");
        //}
        //else { Debug.LogWarning("No 4"); }

    }

    private void ConvertViewportBoundsToMinimapTextureCoords()
    {
        
    }
}
/*
 * viewport(0,0), viewport(0,1), viewport(1,0), viewport(1,1)
 * Ray ray = Camera.ViewportPointToRay(viewport(x,y,z));
 * Raycast Hit;
 * if(Physics.Raycast(ray, out hit)){}
 * 
 * gameobject.Transform.x / Ground.MaxX
 * 
 * 
 */