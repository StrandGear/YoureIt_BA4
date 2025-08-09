using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerScan : MonoBehaviour
{
    private bool isScanning = false;

    public bool IsScanning { get => isScanning; }

    public bool NoObjectsToScan = false;

    private int scanningButtonPressed = 0;

    public bool playerScannedMoreThanOnce = false;

    [SerializeField] private InputActionReference scanControl;

    [SerializeField] private LayerObjectsVisibilityRadius layerObjectsVisibilityRadius;

    public static PlayerScan Instance;

    public bool eyePulsateAnimOn = false;
    [SerializeField] Animator eyeAnimator = null;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        playerScannedMoreThanOnce = false;
    }

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
        if (eyePulsateAnimOn)
        {
            if (layerObjectsVisibilityRadius.VisibleObjects.Count > 0)
            {
                eyeAnimator?.SetBool("StartPulsating", true);
            }
            else
            {
                eyeAnimator?.SetBool("StartPulsating", false);
            }
        }

        if (scanControl.action.WasPressedThisFrame())
        {
            scanningButtonPressed++;
            isScanning = false;
        }

        if (scanControl.action.IsPressed() && !isScanning )
        {
            if (scanningButtonPressed == 1 && layerObjectsVisibilityRadius.VisibleObjects.Count > 0) // scanning
            {
                ScanArea();
                GameStates.Instance.SetGameState(GameState.Puzzlemode);
            }
            else if (scanningButtonPressed >= 2 || layerObjectsVisibilityRadius.VisibleObjects.Count <= 0) // not scanning
            {
                print($"scanningBtnPressed {scanningButtonPressed}; visible objects {layerObjectsVisibilityRadius.VisibleObjects.Count}");
                //StopScanning(true);
                GameStates.Instance.SetGameState(GameState.Playmode);
            }
        }
        
        if (scanningButtonPressed < 0 || scanningButtonPressed > 2)
            scanningButtonPressed = 0;

/*        if (layerObjectsVisibilityRadius.VisibleObjects.Count == 0) //get out of Puzzle mode when no olayerobjects nearby
        {
            print("STOP SCANNIN, NO VISIBLE OBJECTS");
            NoObjectsToScan = true;
            //GameStates.Instance.SetGameState(GameState.Playmode);
            StopScanning();
        }*/
    }

    private void ScanArea()
    {
        isScanning = true;
        playerScannedMoreThanOnce = true;

        foreach (LayerObject elem in layerObjectsVisibilityRadius.VisibleObjects)
        {
            if (!elem.IsUsed)
            {
                LayerManager.Instance.AddLayer(elem);
                //elem.SetShaderActive(true);
            }
        }
    }

    public void StopScanning(bool resetButtonPress = true)
    {
        print("StopScanning method");

        if (resetButtonPress)
            scanningButtonPressed = 0;

        isScanning = false;

        foreach (LayerObject elem in layerObjectsVisibilityRadius.VisibleObjects)
        {
                elem.SetShaderActive(false);
        }

        LayerManager.Instance.ClearLayerList();

        eyeAnimator?.SetBool("StartPulsating", false);

        /*        if (GameStates.Instance.GetCurrentGameState() != GameState.Playmode)
                    GameStates.Instance.SetGameState(GameState.Playmode);*/
    }

    public void ResetScanningButtonPress()
    {
        foreach (LayerObject elem in layerObjectsVisibilityRadius.VisibleObjects)
        {
            elem.SetShaderActive(false);
        }

        LayerManager.Instance.ClearLayerList();

        scanningButtonPressed = 0;
        isScanning = false;
    }
}
