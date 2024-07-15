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

        //stop scanning in case of death
        //player.gameObject.GetComponentInChildren<PlayerScan>().StopScanning();

        //turning off layer UI 
        GetInstance<UIManager>().LayerUI.SetActive(false);
        GetInstance<UIManager>().GameUI.SetActive(true);

        //switching to main camera view
        
        GetInstance<CameraManager>().SwitchCamera(GetInstance<CameraManager>().MainPlayingCam);

        //disable cursor
    }

    private void PuzzleGameStateOn()
    {
        //adding layers in PlayerScan

        //turning on layer UI 
        GetInstance<UIManager>().GameUI.SetActive(false);
        GetInstance<UIManager>().LayerUI.SetActive(true);

        //switching to closest layer camera 
        print("seeting up closest camer");
        GetInstance<CameraManager>().SetActiveClosestCamera(player);

        //enable cursor
    }
}

public enum GameState
{
    Playmode,
    Puzzlemode
}
