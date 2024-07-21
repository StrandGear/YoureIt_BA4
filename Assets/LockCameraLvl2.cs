using UnityEngine;
using Cinemachine;

public class LockCameraAxis : MonoBehaviour
{
    public CinemachineVirtualCamera virtualCamera;
    public Vector3 offset = new Vector3(0, 5, -10); // Default offset

    private Transform playerTransform;

    void Start()
    {
        if (virtualCamera != null)
        {
            playerTransform = virtualCamera.Follow;
            if (playerTransform == null)
            {
                Debug.LogError("No Follow target assigned to the Cinemachine Virtual Camera.");
            }
        }
        else
        {
            Debug.LogError("Cinemachine Virtual Camera is not assigned.");
        }
    }

    void LateUpdate()
    {
        if (playerTransform != null)
        {
            // Calculate the desired camera position based on the player's position and the offset
            Vector3 desiredPosition = new Vector3(
                playerTransform.position.x + offset.x, // Follow X-axis
                offset.y, // Fixed Y position
                offset.z // Fixed Z position
            );

            // Apply the desired position to the camera
            virtualCamera.transform.position = desiredPosition;
        }
    }
}
