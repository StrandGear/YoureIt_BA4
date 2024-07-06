using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton
{
    private List<GameObject> cameras = new List<GameObject>();

    public GameObject MainPlayingCam;
    public GameObject LayerLookCam;

    private GameObject currentCam;

    private void Start()
    {
        currentCam = MainPlayingCam;

        cameras.Add(MainPlayingCam);
        cameras.Add(LayerLookCam);

        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] == currentCam)
            {
                cameras[i].SetActive(true);
            }
            else
            {
                cameras[i].SetActive(false);
            }
        }
    }
    public void SwitchCamera(GameObject newCam)
    {
        currentCam = newCam;

        currentCam.SetActive(true);

        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] != currentCam)
            {
                cameras[i].SetActive(false);
            }
            //print(cameras[i].Priority);
        }
        //print(currentCam);
    }
}
