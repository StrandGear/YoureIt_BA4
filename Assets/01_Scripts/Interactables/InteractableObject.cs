using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class InteractableObject : MonoBehaviour
{
    private GameObject interactionTextElement;

    public string interactionText = "Interact(E)";

    private bool canInteract = false;
    public BoxCollider InteractionRadius { get; set; }

    private void Awake()
    {
        InteractionRadius = GetComponent<BoxCollider>();
        InteractionRadius.isTrigger = true;

        interactionTextElement = transform.Find("GameCanvas").gameObject;
        interactionTextElement.GetComponentInChildren<TMP_Text>().text = interactionText;
        interactionTextElement.SetActive(false);

        StopInteraction();
    }

    public void CanInteract(bool value)
    {
        canInteract = value;

        interactionTextElement.SetActive(canInteract);

        if (!canInteract)
            StopInteraction();
    }

    public virtual void Interact()
    {
        if (!canInteract)
            return;
    }

    public virtual void StopInteraction() { }
}
