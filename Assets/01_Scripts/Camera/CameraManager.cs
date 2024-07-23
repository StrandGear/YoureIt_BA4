using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton
{
    [SerializeField] private List<GameObject> layerCameras = new List<GameObject>();
    public GameObject MainPlayingCam;
    public GameObject IngameUIMenuCam;
    //public GameObject LayerLookCam;

    [SerializeField] private GameObject currentCam;

    bool gameStartedFirstTime = true;

    private void OnValidate()
    {
        if (gameStartedFirstTime && IngameUIMenuCam != null)
            currentCam = IngameUIMenuCam;
        else
            currentCam = MainPlayingCam;
    }

    private void Awake()
    {
        if (gameStartedFirstTime && IngameUIMenuCam != null)
            currentCam = IngameUIMenuCam;
        else
            currentCam = MainPlayingCam;

        /*        if (gameStartedFirstTime)
                    currentCam = IngameUIMenuCam;
                else
                    currentCam = MainPlayingCam;*/
        //cameras.Add(MainPlayingCam);
        //cameras.Add(LayerLookCam);

        for (int i = 0; i < layerCameras.Count; i++)
        {
            if (layerCameras[i] == currentCam)
            {
                layerCameras[i].SetActive(true);
            }
            else
            {
                layerCameras[i].SetActive(false);
            }
        }
        currentCam.SetActive(true);

        if (currentCam == null)
            currentCam = MainPlayingCam;
    }

    public void SwitchCamera(GameObject newCam)
    {
        if (currentCam == null)
            currentCam = MainPlayingCam;

        ResetCamerasPriority();

        if (newCam == null)
            newCam = MainPlayingCam;

        //assigning new camera 
        currentCam = newCam;

        if (currentCam.GetComponent<CinemachineVirtualCamera>() != null)
            currentCam.GetComponent<CinemachineVirtualCamera>().Priority = 50;

        currentCam.SetActive(true);

        if (currentCam != MainPlayingCam)
            MainPlayingCam.SetActive(false);

        for (int i = 0; i < layerCameras.Count; i++)
        {
            if (layerCameras[i] != currentCam)
            {
                layerCameras[i].SetActive(false);
            }
        }
    }

    private void ResetCamerasPriority()
    {
        if (currentCam.GetComponent<CinemachineVirtualCamera>() != null)
        {
            if (currentCam == MainPlayingCam || currentCam == IngameUIMenuCam)
                currentCam.GetComponent<CinemachineVirtualCamera>().Priority = 20;
            else
                currentCam.GetComponent<CinemachineVirtualCamera>().Priority = 10;
        }
    }

    public void SetActiveClosestCamera(Transform closestToThisGameObject)
    {
        float shortestDistance = float.MaxValue;

        GameObject closestCamera = null;

        foreach (GameObject elem in layerCameras)
        {
            float distanceToPlayer = Vector3.Distance(closestToThisGameObject.position, elem.transform.position);

            if (shortestDistance > distanceToPlayer)
            {
                shortestDistance = distanceToPlayer;
                closestCamera = elem;
            }
        }

        SwitchCamera(closestCamera);
    }

    public void SetActiveIngameUIMenuCamera()
    {
        SwitchCamera(IngameUIMenuCam);
    }
}
