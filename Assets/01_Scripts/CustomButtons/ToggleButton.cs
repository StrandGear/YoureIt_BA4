using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleButton : Button
{
    private bool isSelected = false;
    private LayerManager layerManager;

    protected override void Start()
    {
        base.Start();
        layerManager = FindObjectOfType<LayerManager>();
        onClick.AddListener(ToggleSelection);
    }

    private void ToggleSelection()
    {
        if (isSelected)
        {
            Deselect();
        }
        else
        {
            Select();
        }
    }

    public override void Select()
    {
        isSelected = true;
        layerManager.SetActiveLayer(this);
    }

    public void Deselect()
    {
        layerManager.ClearActiveLayer();

        isSelected = false;
    }

    public void Highlight(bool highlight)
    {
        GetComponent<Image>().color = highlight ? ColorBlock.defaultColorBlock.pressedColor : ColorBlock.defaultColorBlock.normalColor;
    }

    public bool IsSelected()
    {
        return isSelected;
    }
}
