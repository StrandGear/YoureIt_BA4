using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerObjectsVisibilityRadius : MonoBehaviour
{
    private List<LayerObject> visibleObjects = new List<LayerObject>();

    public List<LayerObject> VisibleObjects { get => visibleObjects; }

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out LayerObject layerObject);
        if (layerObject != null)
        {
            visibleObjects.Add(layerObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("OnTriggerExit");
        other.gameObject.TryGetComponent(out LayerObject layerObject);

        if (layerObject != null)
        {
            visibleObjects.Remove(layerObject);

            //change later if we dont want to upd list of interactable when they out of FOW
            LayerManager.Instance.RemoveLayer(layerObject);
        }
    }
}
