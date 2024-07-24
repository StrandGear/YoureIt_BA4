using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defining changes when trigger difeerent game mechanics
public class GameStates : MonoBehaviour
{
    public GameState gameState;

    bool gameStartedFirstTime = true;

    public bool DisablePlayerInCutscene = false;

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
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

        private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<CharacterController>().gameObject.transform;

        
            SetGameState(initialLevelState);
        //else
           // SetGameState(GameState.Playmode);
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
        print("Setting state- " + state);
        if (state == GameState.Playmode)
        {
            if (gameState != state)
            {
                gameState = state;
                PlaymodeGameStateOn();
            }
            else
                return;
        }
        else if (state == GameState.Puzzlemode)
        {
            if (gameState != state)
            {
                gameState = state;
                PuzzleGameStateOn();
            }
            else
                return;
        }
        else if (state == GameState.Cutscenemode)
        {
            if (gameState != state)
            {
                gameState = state;
                CutsceneModeOn();
            }
            else
                return;
        }
        else if (state == GameState.IngameUIMenumode)
        {
            if (gameState != state)
            {
                gameState = state;
                IngameUIMenumode();
            }
            else
                return;
        }
    }

    private void PlaymodeGameStateOn()
    {
        //enable player if it wasnt 
        player.gameObject.GetComponent<CharacterController>().enabled = true;

        //enable player mesh renderer 
        player.Find("0 Iris").gameObject.SetActive(true);

        //enable enemy if there is one
        if (Enemy != null)
        {
            Enemy.SetActive(true);
        }

        print("PlaymodeGameStateOn");
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

        //stop character controller 
        player.gameObject.GetComponent<CharacterController>().enabled = false;
        if (DisablePlayerInCutscene)
            player.Find("0 Iris").gameObject.SetActive(false);

        //disable Enemy object
        if (Enemy != null)
            Enemy.SetActive(false);

        //disable all UI 
        UIManager.Instance.SetAllGameUIActive(false);
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
