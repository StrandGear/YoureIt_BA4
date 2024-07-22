using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defining changes when trigger difeerent game mechanics
public class GameStates : Singleton
{
    GameState gameState;

    bool gameStarted = true;

    public GameState GetCurrentGameState()
    {
        return gameState;
    }

    [SerializeField] private Transform player;

    private void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<CharacterController>().gameObject.transform;
       
        SetGameState(GameState.Playmode);
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
    }

    private void PlaymodeGameStateOn()
    {
        //resetting layers in PlayerScan

        //disable cursor
        Cursor.lockState = CursorLockMode.Locked; 

        //turning on layer UI 
        UIManager.Instance.CloseLayerUI();

        GetInstance<CameraManager>().SwitchCamera(GetInstance<CameraManager>().MainPlayingCam); //switching to main camera view
    }

    private void PuzzleGameStateOn()
    {
        //adding layers in PlayerScan

        //enable cursor
        Cursor.lockState = CursorLockMode.Confined;

        //turning on layer UI 
        UIManager.Instance.OpenLayerUI();

        GetInstance<CameraManager>().SetActiveClosestCamera(player); //switching to closest layer camera 
    }
}

public enum GameState
{
    Playmode,
    Puzzlemode
}
