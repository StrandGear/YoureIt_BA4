using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider))]
public class CutsceneTrigger : MonoBehaviour
{
    public int cutsceneIndex = -1; // Set this to the index of the cutscene to play

    public int nextSceneToLoad = -1;

    private bool triggerIsActivated = false;

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null && !triggerIsActivated)
        {
            triggerIsActivated = true;
            if (cutsceneIndex >= 0)
            {
                if (nextSceneToLoad >= 0)
                    Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(cutsceneIndex, (() => SceneManager.LoadScene(nextSceneToLoad)));
                else
                    Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(cutsceneIndex);
            }
/*            else
            {
                Singleton.GetInstance<CutsceneManager>().PlayNextCutscene();
            }*/
        }
    }
}
