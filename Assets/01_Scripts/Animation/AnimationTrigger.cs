using UnityEngine;

public class TriggerAnimation : MonoBehaviour
{
    public Animator animator;  // Reference to the Animator component

    void OnTriggerEnter(Collider other)
    {
        // Check if the player enters the trigger zone
        if (other.CompareTag("Player"))
        {
            // Set the trigger to play the animation
            animator.SetTrigger("PlayAnimation");
            // Set the boolean to true
            animator.SetBool("IsInTriggerZone", true);
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
        }
    }
}
