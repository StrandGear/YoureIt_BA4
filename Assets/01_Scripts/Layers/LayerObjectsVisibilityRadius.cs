using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerObjectsVisibilityRadius : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.TryGetComponent(out LayerObject layerObject);
        if (layerObject != null)
        {
            LayerManager.Instance.AddLayer(layerObject);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        print("OnTriggerExit");
        other.gameObject.TryGetComponent(out LayerObject layerObject);
        if (layerObject != null)
        {
            LayerManager.Instance.RemoveLayer(layerObject);
        }
    }
}
