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

    bool firstTimeGameStarted = true;


    private void Awake()
    {
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
}
