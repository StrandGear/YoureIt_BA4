using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;
using FMOD.Studio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    //debug for the last level
    public bool dontNeedPlayerController = false;

    public bool stopSound= false;

    private EventInstance ambienceEventInstance;
    private EventInstance musicEventInstance;

    private bool isAmbiencePlaying = false;
    private bool isMusicPlaying = false;
    public static AudioManager instance { get; private set; }

    public GameObject player;

    // Use EventReference struct instead of EventRef attribute
    public EventReference uiPaperUnfoldingEvent;

    float initPitch;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one Audio Manager in the scene.");
        }
        instance = this;
        //DontDestroyOnLoad(gameObject);

        if (dontNeedPlayerController)
            return;
        else
        {
            if (player == null)
                player = FindObjectOfType<CharacterController>().gameObject;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        stopSound = true;
    }

    private void Start()
    {
        musicEventInstance.getPitch(out initPitch);
        stopSound = false;
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
            ambienceEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
            ambienceEventInstance.release();
            isAmbiencePlaying = false;
        }
    }
    public void StopMusic()
    {
        musicEventInstance.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        musicEventInstance.release(); //clearing instance
        isMusicPlaying = false;
    }
    public void PauseAmbience()
    {
        if (isAmbiencePlaying)
        {
            ambienceEventInstance.setPaused(true);

            isAmbiencePlaying = false;
        }
    }
    public void PauseMusic()
    {
        if (isMusicPlaying)
        {
            musicEventInstance.setPaused(true);

            isMusicPlaying = false;
        }
    }
    public void UnpauseAmbience()
    {
        if (isAmbiencePlaying)
        {
            ambienceEventInstance.setPaused(false);

            isAmbiencePlaying = true;
        }
    }
    public void UnpauseMusic()
    {
        if (isMusicPlaying)
        {
            musicEventInstance.setPaused(false);

            isMusicPlaying = true;
        }
    }

    public void LowerMusicVolume()
    {
        if (isAmbiencePlaying)
        {
            float pitchMultiplier = 0.1f;
            musicEventInstance.setPitch(initPitch * pitchMultiplier);
        }
    }
    public void IncreaseMusicVolume()
    {
        if (isAmbiencePlaying)
        {
            musicEventInstance.setPitch(initPitch);
        }
    }
    /*    private void ChangedActiveScene(Scene current, Scene next)
        {
            print("CHANGING SCENE");
            StopAmbience();
            StopMusic();
        }*/
}