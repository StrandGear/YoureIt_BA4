using UnityEngine;
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
            interactionCanvasElement.GetComponentInChildren<Image>().sprite = InGameSprite;
            interactionCanvasElement.GetComponentInChildren<TMP_Text>().text = "";
        }
        interactionCanvasElement.SetActive(false);
    }

    public void CanInteract(bool value)
    {
        canInteract = value;
        interactionCanvasElement.SetActive(canInteract);

        if (!canInteract)
            StopInteraction();
    }

    public virtual void Interact()
    {
        if (!canInteract)
            return;

        // Custom interaction logic
    }

    public virtual void StopInteraction()
    {
        interactionCanvasElement.SetActive(false);
    }
}
