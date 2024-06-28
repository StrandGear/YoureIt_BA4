using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LayerObject))]
public class HidingLayer : MonoBehaviour, IHiding
{
    [SerializeField] bool isHidden = false;

    public bool IsHidden {
        get => isHidden;
        set {
            isHidden = value;
            UpdateHidingProperty();
        }
    }

    public Renderer ObjectRenderer
    {
        get;
        set;
    }

    private void OnValidate()
    {
        ObjectRenderer = gameObject.GetComponent<Renderer>();
        UpdateHidingProperty();
    }

    public void UpdateHidingProperty()
    {
        if (isHidden)
            ObjectRenderer.enabled = false;
        else
            ObjectRenderer.enabled = true;
    }
}
