using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : Singleton
{
    public List<GameObject> cutsceneObjects;

    private Queue<GameObject> cutsceneQueue = new Queue<GameObject>();
    private bool isPlaying = false;

    private void Awake()
    {
        // Initialize the cutscene queue
        foreach (GameObject cutsceneObject in cutsceneObjects)
        {
            cutsceneQueue.Enqueue(cutsceneObject);
        }
    }

    public void PlayNextCutscene()
    {
        if (!isPlaying && cutsceneQueue.Count > 0)
        {
            StartCoroutine(PlayCutscene(cutsceneQueue.Dequeue()));
        }
    }

    public void PlayCutsceneByIndex(int index)
    {
        if (!isPlaying && index >= 0 && index < cutsceneObjects.Count)
        {
            cutsceneQueue.Clear(); // Clear the queue to prevent any overlap
            cutsceneQueue.Enqueue(cutsceneObjects[index]);
            PlayNextCutscene();
        }
    }

    private IEnumerator PlayCutscene(GameObject cutsceneObject)
    {
        isPlaying = true;

        // Activate the cutscene object
        cutsceneObject.SetActive(true);

        // Get the PlayableDirector component and play the cutscene
        PlayableDirector director = cutsceneObject.GetComponentInChildren<PlayableDirector>();
        if (director != null)
        {
            //director.Play();
            yield return new WaitForSeconds((float)director.duration);
        }

        // Deactivate the cutscene object
        cutsceneObject.SetActive(false);

        isPlaying = false;

        // Check if there's another cutscene to play
        PlayNextCutscene();
    }

}

/*[System.Serializable]
public class CutsceneData
{
    public GameObject CutsceneObject;

    public UnityEvent unityEvent;
}*/
