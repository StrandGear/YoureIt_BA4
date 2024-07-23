using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableUseKeyTrigger : InteractableObject
{
    public GameObject cutsceneGameObject;
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
        print("Interacting with key");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UI_puzzleSuccess, gameObject.transform.position);
        Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(1, (() => SceneManager.LoadScene(1)));
        //Singleton.GetInstance<CutsceneManager>().PlayNextCutscene(cutsceneGameObject);
        //cutsceneGameObject.SetActive(true);
        StopInteraction();
    }
}
