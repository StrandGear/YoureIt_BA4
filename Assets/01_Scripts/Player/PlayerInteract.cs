using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private InputActionReference interactionButton;

    [SerializeField]  private InteractableObject currentInteractable;

    private bool interactionActive = false;

    // Update is called once per frame
    void Update()
    {
        if (interactionButton.action.WasPressedThisFrame() && currentInteractable != null)
        {
            if (interactionActive)
            {
                currentInteractable.StopInteraction();
                interactionActive = false;
            }
            else
            {
                currentInteractable.Interact();
                interactionActive = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>() ?? other.GetComponentInChildren<InteractableObject>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            currentInteractable.CanInteract(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>() ?? other.GetComponentInChildren<InteractableObject>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable.CanInteract(false);
            if (interactionActive)
            {
                currentInteractable.StopInteraction();
                interactionActive = false;
            }
            currentInteractable = null;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        InteractableObject interactable = other.GetComponent<InteractableObject>() ?? other.GetComponentInChildren<InteractableObject>();
        if (interactable != null && interactable == currentInteractable)
        {
            currentInteractable.CanInteract(true);
        }
    }
}
