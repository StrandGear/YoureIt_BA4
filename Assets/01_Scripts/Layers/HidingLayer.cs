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

    public Renderer ObjectRenderer {get; set;}

    private Collider objectCollider;

    private void OnValidate()
    {
        ObjectRenderer = gameObject.GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
        UpdateHidingProperty();
    }

    private void Awake()
    {
        ObjectRenderer = gameObject.GetComponent<Renderer>();
        objectCollider = GetComponent<Collider>();
    }

    public void UpdateHidingProperty()
    {
        if (isHidden)
        {
            ObjectRenderer.enabled = false;
            objectCollider.isTrigger = true;
        }
        else
        {
            ObjectRenderer.enabled = true;
            objectCollider.isTrigger = false;
        }
    }
}
