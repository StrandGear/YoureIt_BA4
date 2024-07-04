using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraManager : Singleton
{
    private List<CinemachineFreeLook> cameras = new List<CinemachineFreeLook>();

    public CinemachineFreeLook MainPlayingCam;
    public CinemachineFreeLook LayerLookCam;

    public CinemachineFreeLook startCamera;
    private CinemachineFreeLook currentCam;

    private void Start()
    {
        currentCam = startCamera;

        cameras.Add(MainPlayingCam);
        cameras.Add(LayerLookCam);

        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] == currentCam)
            {
                cameras[i].Priority = 20;
            }
            else
            {
                cameras[i].Priority = 10;
            }

        }
    }
    public void SwitchCamera(CinemachineFreeLook newCam)
    {
/*        print("++++++++++++++++++++");
        print(currentCam);*/
        currentCam = newCam;

        currentCam.Priority = 20;

        for (int i = 0; i < cameras.Count; i++)
        {
            if (cameras[i] != currentCam)
            {
                cameras[i].Priority = 10;
            }
            //print(cameras[i].Priority);
        }
        //print(currentCam);
    }
}
