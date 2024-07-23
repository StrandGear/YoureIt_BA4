using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableCutsceneTrigger : InteractableObject
{
    public int cutsceneIndex;

    public int SceneToLoad = -1;
    public override void Interact()
    {
            Open();
    }

    private void Open()
    {
        print("Interacting with key");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.UI_puzzleSuccess, gameObject.transform.position);
        if (SceneToLoad >=0 )
            Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(cutsceneIndex, (() => SceneManager.LoadScene(SceneToLoad)));
        else
            Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(cutsceneIndex);
    }
}
