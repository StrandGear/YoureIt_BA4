using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableUseKeyTrigger : InteractableObject
{
    public override void Interact()
    {
        if (!PlayerInventory.Instance.hasKey)
        {
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UIDialogue_NeedKey);
            //StopInteraction();
        }
        else
            Open();
    }

    private void Open() 
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UI_puzzleSuccess, gameObject.transform.position);
    }
}
