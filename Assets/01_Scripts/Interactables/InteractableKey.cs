using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableKey : InteractableObject
{
    public RegionalPuzzle regionalPuzzle;

    public override void Interact()
    {
        print("Interact with key");

        base.Interact();

        LayerManager.Instance.SetAllObjectsAsUsed();
        LayerObjectsVisibilityRadius.Instance.SetAllVisibleObjectsAsUsed();
        LayerManager.Instance.ClearLayerList();
        regionalPuzzle.interactive = false;

        AudioManager.instance.PlayOneShot(FMODEvents.instance.Keys, gameObject.transform.position);

        PlayerInventory.Instance.AddKey();

        Destroy(gameObject, 0.5f);
    }
}
