using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerObjectsVisibilityRadius : MonoBehaviour //getting specific objects withing collider
{
    [SerializeField] private List<LayerObject> visibleObjects = new List<LayerObject>();

    public List<LayerObject> VisibleObjects { get => visibleObjects; }

    /*    [SerializeField] private GameObject visibleCamera = null;

        public GameObject VisibleCamera { get => visibleCamera; }*/
    public static LayerObjectsVisibilityRadius Instance;
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        //getting all layer objects
        other.gameObject.TryGetComponent(out LayerObject layerObject);
        if (layerObject != null && !layerObject.IsUsed && !layerObject.IsRegionalPuzzle)
        {
            if (layerObject.IsRegionalPuzzle)
                return;

            visibleObjects.Add(layerObject);
        }

        other.gameObject.TryGetComponent(out RegionalPuzzle regionalPuzzle);

        if (regionalPuzzle != null && regionalPuzzle.interactive)
        {   
            foreach (LayerObject elem in regionalPuzzle.LayerObjects)
            {
                visibleObjects.Add(elem);
            }
        }

        //getting camera if there is one


        // Check for CinemachineVirtualCamera
        /*        if (other.TryGetComponent(out CinemachineVirtualCamera camera))
                {
                    if (camera != null)
                    {
                        visibleCamera = camera.gameObject;
                    }
                }*/
    }

    private void OnTriggerExit(Collider other)
    {
        other.gameObject.TryGetComponent(out LayerObject layerObject);
        if (layerObject != null)
        {
            visibleObjects.Remove(layerObject);
            layerObject.SetShaderActive(false);
        }

        other.gameObject.TryGetComponent(out RegionalPuzzle regionalPuzzle);
        if (regionalPuzzle != null)
        {
            foreach (LayerObject elem in regionalPuzzle.LayerObjects)
                elem.SetShaderActive(false);

            visibleObjects.Clear();
        }

        if (visibleObjects.Count <= 0)
        {
            PlayerScan.Instance.ResetScanningButtonPress();

            if (PlayerScan.Instance.playerScannedMoreThanOnce)
                GameStates.Instance.SetGameState(GameState.Playmode);
        }

    }

    private void OnTriggerStay(Collider other)
    {
        other.gameObject.TryGetComponent(out RegionalPuzzle regionalPuzzle);

        if (regionalPuzzle != null && regionalPuzzle.interactive)
        {
            foreach (LayerObject elem in regionalPuzzle.LayerObjects)
            {
                visibleObjects.Add(elem);
            }
        }
    }

    public void SetAllVisibleObjectsAsUsed()
    {
        foreach (LayerObject elem in visibleObjects)
        {
            elem.IsUsed = true;
        }
        visibleObjects.Clear();
        PlayerScan.Instance.ResetScanningButtonPress();
    }
}
