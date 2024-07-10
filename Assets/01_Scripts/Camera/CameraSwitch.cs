using UnityEngine;
using Cinemachine;

public class CameraSwitch : MonoBehaviour
{
    public CinemachineVirtualCamera fixedCamera;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fixedCamera.Priority = 10; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fixedCamera.Priority = 0; 
        }
    }
}