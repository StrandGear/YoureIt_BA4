using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerObjectsVisibilityRadius : MonoBehaviour //getting specific objects withing collider
{
    [SerializeField] private List<LayerObject> visibleObjects = new List<LayerObject>();

    public List<LayerObject> VisibleObjects { get => visibleObjects; }

    [SerializeField] private GameObject visibleCamera = null;

    public GameObject VisibleCamera { get => visibleCamera; }

    private void OnTriggerEnter(Collider other)
    {
        //getting all layer objects
        other.gameObject.TryGetComponent(out LayerObject layerObject);
        if (layerObject != null)
        {
            visibleObjects.Add(layerObject);
        }

        //getting camera if there is one


        // Check for CinemachineVirtualCamera
        if (other.TryGetComponent(out CinemachineVirtualCamera camera))
        {
            if (camera != null)
            {
                visibleCamera = camera.gameObject;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent(out LayerObject layerObject);
        if (layerObject != null)
        {
            visibleObjects.Remove(layerObject);
        }

        //removing camera if there is one
        if (other.TryGetComponent(out CinemachineVirtualCamera camera))
        {
            if (camera != null)
            {
                visibleCamera = null;
            }
        }
    }
}
