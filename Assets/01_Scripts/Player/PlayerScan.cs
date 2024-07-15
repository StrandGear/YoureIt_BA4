using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScan : MonoBehaviour
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
    }

    private void OnDisable()
    {
        scanControl.action.Disable();
    }

    private void Update()
    {
        if (scanControl.action.WasPressedThisFrame())
        {
            scanningButtonPressed++;
            isScanning = false;
        }

        if (scanControl.action.IsPressed() && !isScanning)
        {
            if (scanningButtonPressed == 1) // scanning
            {
                ScanArea();
                Singleton.GetInstance<GameStates>().SetGameState(GameState.Puzzlemode);
            }
            else if (scanningButtonPressed == 2) // not scanning
            {
                Singleton.GetInstance<GameStates>().SetGameState(GameState.Playmode);
                StopScanning(false);
            }
        }
        
        if (scanningButtonPressed < 0 || scanningButtonPressed > 2)
            scanningButtonPressed = 0;

        if (layerObjectsVisibilityRadius.VisibleObjects.Count == 0)
        {
            Singleton.GetInstance<GameStates>().SetGameState(GameState.Playmode);
            StopScanning(false);
        }
    }

    private void ScanArea()
    {
        isScanning = true;

        foreach (LayerObject elem in layerObjectsVisibilityRadius.VisibleObjects)
        {
            if (!elem.IsUsed)
            {
                LayerManager.Instance.AddLayer(elem);
                elem.SetShaderActive(true);
            }
        }
    }

    public void StopScanning(bool resetButtonPress = true)
    {
        if (resetButtonPress)
            scanningButtonPressed = 0;

        isScanning = false;

        foreach (LayerObject elem in layerObjectsVisibilityRadius.VisibleObjects)
        {
                elem.SetShaderActive(false);
        }

        LayerManager.Instance.ClearLayerList();
    }
}
