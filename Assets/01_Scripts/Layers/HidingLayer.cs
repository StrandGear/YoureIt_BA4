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
                if (meshRenderers[i] != null)
                    meshRenderers[i].enabled = false;
            }
            for (int i = 0; i < colliders.Count; i++)
            {
                MeshCollider meshCollider = colliders[i] as MeshCollider;
                if (meshCollider != null)
                {
                    meshCollider.convex = true;
                }
                if (colliders[i] != null)
                colliders[i].isTrigger = true;
            }
            gameObject.GetComponent<LayerObject>().SetShaderActive(false);
        }
        else
        {
            for (int i = 0; i < meshRenderers.Count; i++)
            {
                if (meshRenderers[i] != null)
                    meshRenderers[i].enabled = true;
            }
            for (int i = 0; i < colliders.Count; i++)
            {
                MeshCollider meshCollider = colliders[i] as MeshCollider;
                if (meshCollider != null)
                {
                    meshCollider.convex = true;
                }

                else if (colliders[i] != null)
                    colliders[i].isTrigger = false;
            }
            //gameObject.GetComponent<LayerObject>().SetShaderActive(true);
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
