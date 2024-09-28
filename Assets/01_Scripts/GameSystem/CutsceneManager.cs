using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : Singleton
{
    public List<GameObject> cutsceneObjects;

    private bool isPlaying = false;
    private UnityAction onCutsceneEnd;
    private Coroutine currentCoroutine;

    public bool CutSceneIsDone = false; 

    private void Awake()
    {
        // Ensure all cutscene objects are initially inactive
        foreach (GameObject cutsceneObject in cutsceneObjects)
        {
            cutsceneObject.SetActive(false);
        }
    }

    public void PlayCutsceneByIndex(int index, UnityAction callback = null)
    {
        if (!isPlaying && index >= 0 && index < cutsceneObjects.Count)
        {
            onCutsceneEnd = callback; // Store the callback function
            if (currentCoroutine != null)
            {
                StopCoroutine(currentCoroutine);
            }
            currentCoroutine = StartCoroutine(PlayCutscene(cutsceneObjects[index]));
        }
    }

    private IEnumerator PlayCutscene(GameObject cutsceneObject)
    {
        GameStates.Instance.SetGameState(GameState.Cutscenemode);

        isPlaying = true;

        // Activate the cutscene object
        cutsceneObject.SetActive(true);

        // Get the PlayableDirector component and play the cutscene
        PlayableDirector director = cutsceneObject.GetComponentInChildren<PlayableDirector>();
        if (director != null)
        {
            director.Play();
            print(director.duration);
            if (director.duration > 0)
                yield return new WaitForSeconds((float)director.duration);
            else
                yield return new WaitForSeconds(5f);
        }

        // Deactivate the cutscene object
        cutsceneObject.SetActive(false);

        isPlaying = false;

        if (cutsceneObject == cutsceneObjects[1])
            CutSceneIsDone = true;

        // Call the callback if it is specified, otherwise return to Playmode
        if (onCutsceneEnd != null)
        {
            onCutsceneEnd?.Invoke();
        }
        else
        {
            GameStates.Instance.SetGameState(GameState.Playmode);
        }

    }

}

/*[System.Serializable]
public class CutsceneData
{
    public GameObject CutsceneObject;

    public UnityEvent unityEvent;
}*/
