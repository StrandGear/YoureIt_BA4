using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.UI;
using TMPro;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
public class InteractableObject : MonoBehaviour
{
    private GameObject interactionCanvasElement;

    public string interactionText = "Interact(E)";

    private bool canInteract = false;
    public BoxCollider InteractionRadius { get; set; }

    [Tooltip("Its a popup showed in the game")]
    public Sprite InGameSprite;

    private GameObject inGameUiGameObjectToRenderPicture; //small gmae in UI
    private void Awake()
    {
        InteractionRadius = GetComponent<BoxCollider>();
        if (InteractionRadius != null)
            InteractionRadius.isTrigger = true;

        interactionCanvasElement = transform.Find("GameCanvas").gameObject;
        interactionCanvasElement.GetComponentInChildren<TMP_Text>().text = interactionText;

    }
    private void Start()
    {
        if (InGameSprite != null)
        {
            //inGameUiGameObjectToRenderPicture = interactionCanvasElement.GetComponentInChildren<Image>().gameObject;
            interactionCanvasElement.GetComponentInChildren<Image>().sprite = InGameSprite;

            //clearing text if we have Image
            interactionCanvasElement.GetComponentInChildren<TMP_Text>().text = "";
        }
        else
        {
            if (interactionCanvasElement.GetComponentInChildren<Image>().gameObject != null)
                interactionCanvasElement.GetComponentInChildren<Image>().gameObject.SetActive(false);
        }
        interactionCanvasElement.SetActive(false);

        StopInteraction();
    }

    public void CanInteract(bool value)
    {
        print("Can Interact");
        canInteract = value;

        interactionCanvasElement.SetActive(canInteract);

        if (!canInteract)
            StopInteraction();
    }

    public virtual void Interact()
    {
        if (!canInteract)
            return;
    }

    public virtual void StopInteraction() {
        interactionCanvasElement.SetActive(false);
    }
}
