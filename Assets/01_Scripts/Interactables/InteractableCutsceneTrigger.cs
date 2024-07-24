using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InteractableCutsceneTrigger : InteractableObject
{
    public int cutsceneIndex;

    public int SceneToLoad = -1;

    public GameObject blackGlobalVolume = null;
    public override void Interact()
    {
            Open();
    }

    private void Open()
    {
        if (SceneToLoad >=0 )
            Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(cutsceneIndex, (() => { blackGlobalVolume.SetActive(true); SceneManager.LoadScene(SceneToLoad); }));
        else
            Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(cutsceneIndex);

        StopInteraction();
    }
}
