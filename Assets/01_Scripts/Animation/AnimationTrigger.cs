using UnityEngine;
using Cinemachine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator component
    public CinemachineVirtualCamera virtualCamera;  // Reference to the Cinemachine Virtual Camera
    private CinemachineBasicMultiChannelPerlin noiseSettings;  // Reference to the noise settings

    // Values for noise settings
    public float amplitudeGain = 1.0f;
    public float frequencyGain = 1.0f;

    void Start()
    {
        // Get the noise settings component from the virtual camera
        if (virtualCamera != null)
        {
            noiseSettings = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

            // Ensure the initial noise settings are zero to avoid always shaking
            if (noiseSettings != null)
            {
                noiseSettings.m_AmplitudeGain = 0.0f;
                noiseSettings.m_FrequencyGain = 0.0f;
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            // Set the trigger to play the animation
            animator.SetTrigger("PlayAnimation");
            // Set the boolean to true
            animator.SetBool("IsInTriggerZone", true);

            // Activate noise settings
            if (noiseSettings != null)
            {
                noiseSettings.m_AmplitudeGain = amplitudeGain;
                noiseSettings.m_FrequencyGain = frequencyGain;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Check if the player exits the trigger zone
        if (other.CompareTag("Player"))
        {
            // Reset the trigger
            animator.ResetTrigger("PlayAnimation");
            // Set the boolean to false
            animator.SetBool("IsInTriggerZone", false);

            // Deactivate noise settings
            if (noiseSettings != null)
            {
                noiseSettings.m_AmplitudeGain = 0.0f;
                noiseSettings.m_FrequencyGain = 0.0f;
            }
        }
    }
}
