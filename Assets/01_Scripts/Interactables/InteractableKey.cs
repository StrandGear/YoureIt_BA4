using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : InteractableObject
{
    public override void Interact()
    {
        base.Interact();

        AudioManager.instance.PlayOneShot(FMODEvents.instance.Keys, gameObject.transform.position);

        PlayerInventory.Instance.AddKey();

        StopInteraction();
    }

    public override void StopInteraction()
    {
        base.StopInteraction();
        Destroy(gameObject, 0.5f);
    }
}
