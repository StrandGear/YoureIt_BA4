using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.HID;
using UnityEngine.InputSystem;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private InputActionReference pauseControl;
    [SerializeField] private GameObject pauseScreen;

    bool firstTimeGameStarted = true;
    bool isPaused = false;

    private void Awake()
    {
        print("Awake called");
        playButton.onClick.AddListener((() =>
        {
            //SceneManager.LoadScene(1);
            if (firstTimeGameStarted)
            {
                firstTimeGameStarted = false;
                Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(0);
            }
            
        }));
        
        exitButton.onClick.AddListener((() =>
        {
            Application.Quit();
        }));
    }

    private void Start()
    {
        print("Start called");
    }

    private void Update()
    {
        print("Update called");
        CheckPauseInput();
    }

    public void CheckPauseInput()
    {
        print("CheckPauseInput called");
        if (pauseControl.action.triggered)
        {
            print("Pause Menu triggered");
            TogglePauseMenu();
        }
    }

    public void TogglePauseMenu()
    {
        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    private void PauseGame()
    {
        isPaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    private void ResumeGame()
    {
        isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        print("OnEnable called");
        pauseControl.action.Enable();
    }

    private void OnDisable()
    {
        print("OnDisable called");
        pauseControl.action.Disable();
    }
}
