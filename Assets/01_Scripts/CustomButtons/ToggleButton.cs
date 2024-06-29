using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ToggleButton : Button
{
    private bool isSelected = false;

    protected override void Start()
    {
        base.Start();
        
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
        //LayerManager.Instance.SetActiveLayer(this);
    }

    public void Deselect()
    {
        LayerManager.Instance.ClearActiveLayer();

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
