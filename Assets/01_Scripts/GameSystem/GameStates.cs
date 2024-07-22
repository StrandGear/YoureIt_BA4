using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defining changes when trigger difeerent game mechanics
public class GameStates : Singleton
{
    GameState gameState;

    bool gameStartedFirstTime = true;

    public GameState GetCurrentGameState()
    {
        return gameState;
    }

    [SerializeField] private Transform player;

    private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<CharacterController>().gameObject.transform;

        
            SetGameState(GameState.IngameUIMenumode);
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

        print("PlaymodeGameStateOn");
        //resetting layers in PlayerScan

        //disable cursor
        Cursor.lockState = CursorLockMode.Locked;

        //turning on game UIs in case they were off
        UIManager.Instance.SetAllGameUIActive(true);

        //turning on layer UI 
        UIManager.Instance.CloseLayerUI();

        GetInstance<CameraManager>().SwitchCamera(GetInstance<CameraManager>().MainPlayingCam); //switching to main camera view
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

        GetInstance<CameraManager>().SetActiveClosestCamera(player); //switching to closest layer camera 
    }

    private void CutsceneModeOn()
    {
        print("CutsceneModeOn");
        //disable cursor
        Cursor.lockState = CursorLockMode.Locked;

        //stop character controller 
        player.gameObject.GetComponent<CharacterController>().enabled = false;

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

        //disable camera
        //disable all other UIs
        UIManager.Instance.SetAllGameUIActive(false);
        //switch camera
        GetInstance<CameraManager>().SetActiveIngameUIMenuCamera();
    }
}

public enum GameState
{
    Playmode,
    Puzzlemode,
    Cutscenemode,
    IngameUIMenumode
}
