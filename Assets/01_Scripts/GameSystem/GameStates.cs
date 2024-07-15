using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Defining changes when trigger difeerent game mechanics
public class GameStates : Singleton
{
    GameState gameState;

    public GameState GetCurrentGameState()
    {
        return gameState;
    }

    [SerializeField] private Transform player;

    private void Awake()
    {
        SetGameState(GameState.Playmode);
    }

    public void SetGameState(GameState state)
    {
        gameState = state;

        if (gameState == GameState.Playmode)
            PlaymodeGameStateOn();
        else if (gameState == GameState.Puzzlemode)
            PuzzleGameStateOn();
    }

    private void PlaymodeGameStateOn()
    {
        //resetting layers in PlayerScan
        
        Cursor.lockState = CursorLockMode.Locked; //disable cursor

        //turning off layer UI 
        GetInstance<UIManager>().LayerUI.SetActive(false);
        GetInstance<UIManager>().GameUI.SetActive(true);

        GetInstance<CameraManager>().SwitchCamera(GetInstance<CameraManager>().MainPlayingCam); //switching to main camera view
    }

    private void PuzzleGameStateOn()
    {
        //adding layers in PlayerScan

        //enable cursor
        Cursor.lockState = CursorLockMode.Confined;

        //turning on layer UI 
        GetInstance<UIManager>().GameUI.SetActive(false);
        GetInstance<UIManager>().LayerUI.SetActive(true);
        
        GetInstance<CameraManager>().SetActiveClosestCamera(player); //switching to closest layer camera 
    }
}

public enum GameState
{
    Playmode,
    Puzzlemode
}
