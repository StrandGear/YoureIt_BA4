using UnityEngine;
using Cinemachine;

public class CameraSwitcher : MonoBehaviour
{
    public CinemachineVirtualCamera fixedCamera;
    public CinemachineVirtualCamera mainCamera;
    public Collider confinerCollider;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fixedCamera.Priority = 10; // Higher priority to activate this camera
            mainCamera.Priority = 5; // Lower priority to deactivate this camera

            // Apply confiner to limit the camera movement within the specific area
            var confiner = fixedCamera.GetComponent<CinemachineConfiner>();
            if (confiner != null)
            {
                confiner.m_BoundingVolume = confinerCollider;
                confiner.InvalidatePathCache();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fixedCamera.Priority = 5; // Lower priority to deactivate this camera
            mainCamera.Priority = 10; // Higher priority to activate this camera

            // Remove confiner to allow the main camera to move freely
            var confiner = fixedCamera.GetComponent<CinemachineConfiner>();
            if (confiner != null)
            {
                confiner.m_BoundingVolume = null;
            }
        }
    }
}