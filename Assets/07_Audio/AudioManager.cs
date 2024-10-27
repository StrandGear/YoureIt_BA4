using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public bool stopSound= false;

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    private bool isAmbiencePlaying = false;
    private bool isMusicPlaying = false;
    public static AudioManager instance { get; private set; }

    public GameObject player;

    // Use EventReference struct instead of EventRef attribute
    public EventReference uiPaperUnfoldingEvent;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);

        if (player == null)
            player = FindObjectOfType<CharacterController>().gameObject;
    }

    private void Start()
    {
       //SceneManager.activeSceneChanged += ChangedActiveScene;

        // Only initialize if not already playing to avoid duplicates
/*        if (!isAmbiencePlaying) InitializeAmbience();
        if (!isMusicPlaying) InitializeMusic();*/
    }

    private void Update()
    {
        // Check for the F key press
        if (Input.GetKeyDown(KeyCode.F))
        {
            PlayOneShotAtPlayerPosition(uiPaperUnfoldingEvent);
        }
        if (stopSound)
        {
            StopAmbience();
            StopMusic();
        }    
    }

    public void InitializeAmbience()
    {
        if (!isAmbiencePlaying)  // Ensure ambience isn't already playing
        {
            ambienceEventInstance = CreateEventInstance(FMODEvents.instance.ambience);
            ambienceEventInstance.start();
            isAmbiencePlaying = true;
        }
    }

    public void InitializeMusic()
    {
        if (!isMusicPlaying)  // Ensure music isn't already playing
        {
            musicEventInstance = CreateEventInstance(FMODEvents.instance.music);
            musicEventInstance.start();
            isMusicPlaying = true;
        }
    }

    public void PlayOneShot(EventReference sound, Vector3 worldPos)
    {
        RuntimeManager.PlayOneShot(sound, worldPos);
    }

    public void PlayOneShotAtPlayerPosition(EventReference sound)
    {
        Vector3 playerPos = player.transform.position;
        RuntimeManager.PlayOneShot(sound, playerPos);
    }

    public void PlayRandomShotFromList(List<EventReference> listOfSounds, Vector3 worldPos)
    {
        int rand = Random.Range(0, listOfSounds.Count);
        PlayOneShot(listOfSounds[rand], worldPos);
    }

    public EventInstance CreateEventInstance(EventReference eventReference)
    {
        EventInstance eventInstance = RuntimeManager.CreateInstance(eventReference);
        return eventInstance;
    }

    public void StopAmbience()
    {
        if (isAmbiencePlaying)
        {
            ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            ambienceEventInstance.release();
            isAmbiencePlaying = false;
        }
    }
    public void StopMusic()
    {
        if (isMusicPlaying)
        {
            musicEventInstance.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
            musicEventInstance.release(); //clearing instance
            isMusicPlaying = false;
        }
    }

/*    private void ChangedActiveScene(Scene current, Scene next)
    {
        print("CHANGING SCENE");
        StopAmbience();
        StopMusic();
    }*/
}