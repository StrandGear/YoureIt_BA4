using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defining changes when trigger difeerent game mechanics
public class GameStates : MonoBehaviour
{
    GameState gameState;

    bool gameStartedFirstTime = true;

    public bool muteMusicOnFirstLevel = false; //should be true only in the first lvl

    public bool DisablePlayerInCutscene = true;

    public GameState initialLevelState;

    public GameState GetCurrentGameState()
    {
        return gameState;
    }

    [SerializeField] private Transform player;

    public GameObject Enemy = null;

    //DEBUG
    //public GameState currentGameState;
    private static GameStates instance = null;
    public static GameStates Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<GameStates>();
                if (instance == null)
                {
                    GameObject go = new GameObject("GameStates");
                    instance = go.AddComponent<GameStates>();
                }
            }

            return instance;
        }
    }

    //private EventInstance UI_selectObject_sound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        
    }

    private void Start()
    {
        if (muteMusicOnFirstLevel)
        {
            AudioManager.instance.StopMusic();
            AudioManager.instance.StopAmbience();
        }
        AudioManager.instance.stopSound = false;
        if (player == null)
            player = FindFirstObjectByType<CharacterController>().gameObject.transform;

        SetGameState(initialLevelState);
    }

    private void Update()
    { 
/*        if (player.GetComponentInChildren<PlayerScan>() != null)
        {
            if (player.GetComponentInChildren<PlayerScan>().NoObjectsToScan == true && gameState == GameState.Puzzlemode)
                SetGameState(GameState.Playmode);
        }*/
    }

    public void SetGameState(GameState state)
    {
        switch (state)
        {
            case GameState.Playmode:
                if (gameState != state)
                {
                    gameState = state;
                    PlaymodeGameStateOn();
                }
                break;

            case GameState.Puzzlemode:
                if (gameState != state)
                {
                    gameState = state;
                    PuzzleGameStateOn();
                }
                break;

            case GameState.Cutscenemode:
                if (gameState != state)
                {
                    gameState = state;
                    CutsceneModeOn();
                }
                break;

            case GameState.IngameUIMenumode:
                if (gameState != state)
                {
                    gameState = state;
                    IngameUIMenumode();
                }
                break;

            default:
                if (gameState != state)
                {
                    gameState = state;
                    PlaymodeGameStateOn();
                }
                break;
        }
    }

    private void PlaymodeGameStateOn()
    {
        Time.timeScale = 1;

        //enable player if it wasnt 
        player.gameObject.GetComponent<CharacterController>().enabled = true;

        //enable player mesh renderer 
        //player.Find("0 Iris").gameObject.SetActive(true);
        player.gameObject.SetActive(true);

        AudioManager.instance.InitializeAmbience();
        AudioManager.instance.InitializeMusic();

        //enable enemy if there is one
        if (Enemy != null)
        {
            Enemy.SetActive(true);
        }
        //resetting layers in PlayerScan

        //disable cursor
        Cursor.lockState = CursorLockMode.Locked;

        //turning on game UIs in case they were off
        UIManager.Instance.SetAllGameUIActive(true);

        //turning on layer UI 
        UIManager.Instance.CloseLayerUI();

        Singleton.GetInstance<CameraManager>().SwitchCamera(Singleton.GetInstance<CameraManager>().MainPlayingCam); //switching to main camera view
    }

    private void PuzzleGameStateOn()
    {
        //return if no objects to scan are available 

        print("PuzzleGameStateOn");
        //enable player if it wasnt 
        player.gameObject.GetComponent<CharacterController>().enabled = true;

        //adding layers in PlayerScan

        //enable cursor
        Cursor.lockState = CursorLockMode.Confined;

        //turning on game UIs in case they were off
        UIManager.Instance.SetAllGameUIActive(true);

        //turning on layer UI 
        UIManager.Instance.OpenLayerUI();

        Singleton.GetInstance<CameraManager>().SetActiveClosestCamera(player); //switching to closest layer camera 
    }

    private void CutsceneModeOn()
    {
        print("CutsceneModeOn");
        //disable cursor
        Cursor.lockState = CursorLockMode.Locked;

        if (DisablePlayerInCutscene)
        {
            print("Disabling character  ");
            //player.Find("0 Iris").gameObject?.SetActive(false);

            player.gameObject.SetActive(false);
        }

        //stop character controller 
        if (player.gameObject.GetComponent<CharacterController>() != null)
            player.gameObject.GetComponent<CharacterController>().enabled = false;

        //disable Enemy object
        if (Enemy != null)
            Enemy.SetActive(false);

        //disable all UI 
        UIManager.Instance.SetAllGameUIActive(false);

        AudioManager.instance.StopAmbience();
        AudioManager.instance.StopMusic();
        //prolly time scale = 0
        //set active camera
    }

    private void IngameUIMenumode()
    {
        print("IngameUIMenumode");
        //stop character controller 
        player.gameObject.GetComponent<CharacterController>().enabled = false;
        Cursor.lockState = CursorLockMode.Confined;
        //disable camera
        //disable all other UIs
        UIManager.Instance.SetAllGameUIActive(false);
        //switch camera
        Singleton.GetInstance<CameraManager>().SetActiveIngameUIMenuCamera();
    }
}

[SerializeField]
public enum GameState
{
    Playmode,
    Puzzlemode,
    Cutscenemode,
    IngameUIMenumode
}
