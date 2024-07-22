using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem.HID;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;


    private void Awake()
    {
        playButton.onClick.AddListener((() =>
        {
            SceneManager.LoadScene(1);
        }));
        
        exitButton.onClick.AddListener((() =>
        {
            Application.Quit();
        }));
    }
}
