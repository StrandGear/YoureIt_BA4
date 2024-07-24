using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private InputActionReference interactionButton;

    private InteractableObject currentInteractable;

    private bool interactionButtonIsPressed = false;

    private int interactionButtonPressedTimes = 0;

    // Update is called once per frame
    void Update()
    {
        if (interactionButton.action.IsPressed() && currentInteractable != null && !interactionButtonIsPressed)
        {
            if (interactionButtonPressedTimes == 1)
            {
                interactionButtonIsPressed = true;
                currentInteractable.Interact();
            }
             else if (interactionButtonPressedTimes == 2)
            {
                interactionButtonIsPressed = true;

                interactionButtonPressedTimes = 0;

                currentInteractable.StopInteraction();
            }
        }

        if (interactionButton.action.WasPressedThisFrame())
        {
            interactionButtonPressedTimes++;

            interactionButtonIsPressed = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<InteractableObject>() != null || other.GetComponentInChildren<InteractableObject>() != null)
        {
            currentInteractable = other.GetComponent<InteractableObject>();

            currentInteractable.CanInteract(true);
            
        }
    }

/*    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<InteractableObject>() != null || other.GetComponentInChildren<InteractableObject>() != null)
        {
            currentInteractable = other.GetComponent<InteractableObject>();

            currentInteractable.CanInteract(true);

        }
    }*/

    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<InteractableObject>() != null || other.GetComponentInChildren<InteractableObject>() != null)
        {
            currentInteractable.CanInteract(false);

            currentInteractable = null;
        }
    }
}
