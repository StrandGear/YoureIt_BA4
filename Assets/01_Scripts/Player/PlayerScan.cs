using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScan : Singleton
{
    private bool isScanning = false;

    public bool IsScanning { get => isScanning; }

    private int scanningButtonPressed = 0;

    [SerializeField] private InputActionReference scanControl;

    [SerializeField] private LayerObjectsVisibilityRadius layerObjectsVisibilityRadius;

    [SerializeField]  public GameObject EyeUIElement;
    [SerializeField]  public GameObject LayersUIElement;

    private void OnEnable()
    {
        scanControl.action.Enable();

        if (layerObjectsVisibilityRadius == null)
        {
            layerObjectsVisibilityRadius = FindObjectOfType<LayerObjectsVisibilityRadius>();
        }

        LayersUIElement.SetActive(false);
        EyeUIElement.SetActive(true);
    }

    private void OnDisable()
    {
        scanControl.action.Disable();
    }

    private void Update()
    {
        if (scanControl.action.IsPressed() && !isScanning)
        {
            if (/*PlayerInventory.Instance.NumberOfEyes > 0 && */ scanningButtonPressed < 2) // scanning
            {
                StartScanningLogic();
            }
            else if (scanningButtonPressed == 2) // not scanning
            {
                StopScanning(false);
            }
        }
        
        if (scanControl.action.WasPressedThisFrame())
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

        //PlayerInventory.Instance.EyeUsed();

        foreach (LayerObject elem in layerObjectsVisibilityRadius.VisibleObjects)
        {
            if (!elem.IsUsed)
                LayerManager.Instance.AddLayer(elem);
        }
    }

    public void StartScanningLogic()
    {
        //switching camera perspective
        if (layerObjectsVisibilityRadius.VisibleCamera != null)
            GetInstance<CameraManager>().SwitchCamera(layerObjectsVisibilityRadius.VisibleCamera);
        else
            GetInstance<CameraManager>().SwitchCamera(Singleton.GetInstance<CameraManager>().LayerLookCam);

        //showing layer UI 
        EyeUIElement.SetActive(false);
        LayersUIElement.SetActive(true);

        ScanArea();
    }

    public void StopScanning(bool resetButtonPress = true)
    {
        if (resetButtonPress)
            scanningButtonPressed = 0;

        isScanning = false;

        //hide layer UI 
        LayersUIElement.SetActive(false);
        EyeUIElement.SetActive(true);

        LayerManager.Instance.ClearLayerList();

        GetInstance<CameraManager>().SwitchCamera(Singleton.GetInstance<CameraManager>().MainPlayingCam);
    }
}
