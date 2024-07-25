using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggeringKey_f : MonoBehaviour
{
    public GameObject blackGlobalVolume = null;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            if (!PlayerInventory.Instance.hasKey)
            {
                AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UIDialogue_NeedKey);
                //StopInteraction();
            }
            else
                Open();
        }

    }

    private void Open()
    {
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UI_puzzleSuccess, gameObject.transform.position);
        Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(1, (() => { blackGlobalVolume.SetActive(true); SceneManager.LoadScene(1); }));
    }
}
