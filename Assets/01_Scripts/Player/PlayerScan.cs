using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScan : MonoBehaviour
{

    private bool isScanning = false;
    public bool IsScanning { get => isScanning; }

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
        if (scanControl.action.IsPressed() && !IsScanning)
        {
            if (PlayerInventory.Instance.NumberOfEyes > 0)
                ScanArea();
        }
        else
            isScanning = false;
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
