using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToggleImageButton : Toggle
{
    private LayerManager layerManager;

    protected override void Start()
    {
        base.Start();
        layerManager = FindObjectOfType<LayerManager>();

        onValueChanged.AddListener(OnToggleValueChange);
    }
    
    private void OnToggleValueChange(bool value)
    {
        layerManager.UpdateLockToggleValue(value);
    }
}
