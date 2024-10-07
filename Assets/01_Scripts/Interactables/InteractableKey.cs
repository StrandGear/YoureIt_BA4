using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : InteractableObject
{
    public override void Interact()
    {
        print("Interact with key");

        base.Interact();

        AudioManager.instance.PlayOneShot(FMODEvents.instance.Keys, gameObject.transform.position);

        PlayerInventory.Instance.AddKey();

        Destroy(gameObject, 0.5f);
    }
}
