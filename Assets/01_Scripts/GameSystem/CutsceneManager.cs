using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class CutsceneManager : Singleton
{
    /* public List<GameObject> cutsceneObjects;

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

     public void PlayNextCutscene(GameObject gameObject)
     {
         if (!isPlaying)
         {
             StartCoroutine(PlayCutscene(gameObject));
         }
     }

     public void PlayCutsceneByIndex(int index)
     {
         if (!isPlaying && index >= 0 && index < cutsceneObjects.Count)
         {
             print("playing cutscene");
             cutsceneQueue.Clear(); // Clear the queue to prevent any overlap
             cutsceneQueue.Enqueue(cutsceneObjects[index]);
             PlayNextCutscene();
         }
     }

     private IEnumerator PlayCutscene(GameObject cutsceneObject)
     {
         Singleton.GetInstance<GameStates>().SetGameState(GameState.Cutscenemode);

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
         print("playing cutscene2");
         isPlaying = false;

         Singleton.GetInstance<GameStates>().SetGameState(GameState.Playmode);

         // Check if there's another cutscene to play
         PlayNextCutscene();
     }*/

    public List<GameObject> cutsceneObjects;

    private bool isPlaying = false;
    private UnityAction onCutsceneEnd;
    private Coroutine currentCoroutine;

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
        Singleton.GetInstance<GameStates>().SetGameState(GameState.Cutscenemode);

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

        Singleton.GetInstance<GameStates>().SetGameState(GameState.Playmode);

        // Call the callback function if it is specified
        onCutsceneEnd?.Invoke();
    }

}

/*[System.Serializable]
public class CutsceneData
{
    public GameObject CutsceneObject;

    public UnityEvent unityEvent;
}*/
