using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;

[RequireComponent(typeof(BoxCollider))]
public class InteractableObject : MonoBehaviour
{
    private GameObject interactionTextElement;

    private bool canInteract = false;
    public BoxCollider InteractionRadius { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private void Awake()
    {
        interactionTextElement = transform.Find("GameCanvas").gameObject;
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
