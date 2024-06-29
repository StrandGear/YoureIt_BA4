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

    private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();
    private List<Collider> colliders = new List<Collider>();

    private void OnValidate()
    {
        meshRenderers = FillListWithComponentsInChildren<MeshRenderer>();
        colliders = FillListWithComponentsInChildren<Collider>();

        UpdateHidingProperty();
    }

    private void Awake()
    {
        meshRenderers = FillListWithComponentsInChildren<MeshRenderer>();
        colliders = FillListWithComponentsInChildren<Collider>();
    }

    public void UpdateHidingProperty()
    {
        if (isHidden)
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].enabled = false;
            }
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].isTrigger = true;
            }
        }
        else
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                meshRenderers[i].enabled = true;
            }
            for (int i = 0; i < colliders.Count; i++)
            {
                colliders[i].isTrigger = false;
            }
        }
    }

    private List<T> FillListWithComponentsInChildren<T>()
    {
        List<T> tempList = new();

        tempList.Add(gameObject.GetComponent<T>());

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                tempList.Add(transform.GetChild(i).transform.GetComponent<T>());
            }
        }

        return tempList;
    }
}
