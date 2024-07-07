using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScan : MonoBehaviour
{

    private bool isScanning = false;
    private int scanningButtonPressed = 0;

    [SerializeField] private InputActionReference scanControl;

    [SerializeField] private LayerObjectsVisibilityRadius layerObjectsVisibilityRadius;

    private void OnEnable()
    {
        scanControl.action.Enable();

        if (layerObjectsVisibilityRadius == null)
        {
            layerObjectsVisibilityRadius = FindObjectOfType<LayerObjectsVisibilityRadius>();
        }
    }

    private void OnDisable()
    {
        scanControl.action.Disable();
    }

    private void Update()
    {
        if (scanControl.action.IsPressed() && !isScanning)
        {
            if (PlayerInventory.Instance.NumberOfEyes > 0 && scanningButtonPressed == 1)
            {
                Singleton.GetInstance<CameraManager>().SwitchCamera(Singleton.GetInstance<CameraManager>().LayerLookCam);

                //show layer UI 
                ScanArea();
            }
            else if (scanningButtonPressed == 2)
            {
                scanningButtonPressed = 0;

                //hide layer UI 

                LayerManager.Instance.ClearLayerList();

                Singleton.GetInstance<CameraManager>().SwitchCamera(Singleton.GetInstance<CameraManager>().MainPlayingCam);
            }
        }
        
        if (scanControl.action.WasReleasedThisFrame())
        {
            scanningButtonPressed++;
            isScanning = false;
        }

        if (scanningButtonPressed < 0 || scanningButtonPressed > 2)
            scanningButtonPressed = 0;
    }

    private void ScanArea()
    {
        isScanning = true;

        PlayerInventory.Instance.EyeUsed();

        foreach (LayerObject elem in layerObjectsVisibilityRadius.VisibleObjects)
        {
            if (!elem.IsUsed)
                LayerManager.Instance.AddLayer(elem);
        }
    }
}
