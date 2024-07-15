using UnityEngine;

public class MatryoshkaLightTrigger : MonoBehaviour
{
    public GameObject lightParent;  // Reference to the parent object containing the lights

    // Start is called before the first frame update
    void Start()
    {
        // Disable all lights at the start of the game
        foreach (Light light in lightParent.GetComponentsInChildren<Light>())
        {
            light.enabled = false;
        }
    }

    // This function is called when another collider enters the trigger zone
    private void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger zone is tagged as "Player"
        if (other.CompareTag("Player"))
        {
            // Enable all lights
            foreach (Light light in lightParent.GetComponentsInChildren<Light>())
            {
                light.enabled = true;
            }
        }
    }
}
