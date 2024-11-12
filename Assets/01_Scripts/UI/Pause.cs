using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button exitButton;
    [SerializeField] private InputActionReference pauseControl;  
    [SerializeField] private GameObject pauseScreen;  

    private bool isPaused = false;

    private void Awake()
    {
        
        if (pauseControl != null)
        {
            pauseControl.action.Enable();
        }
        else
        {
            Debug.LogError("Pause Control input action is not assigned in the Inspector.");
        }

        
        if (pauseScreen != null)
        {
            pauseScreen.SetActive(false);
        }
        else
        {
            Debug.LogError("Pause Screen GameObject is not assigned in the Inspector.");
        }

        
        if (playButton != null)
        {
            playButton.onClick.AddListener(ResumeGame);
        }
        else
        {
            Debug.LogError("Play Button is not assigned in the Inspector.");
        }

       
        if (exitButton != null)
        {
            exitButton.onClick.AddListener(() =>
            {
                Application.Quit();
                
            });
        }
        else
        {
            Debug.LogError("Exit Button is not assigned in the Inspector.");
        }
    }

    private void Update()
    {
        CheckPauseInput();
    }

    private void CheckPauseInput()
    {
        if (pauseControl != null && pauseControl.action.triggered)
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
    {
        if (isPaused)
        {
            ResumeGame();
        }
        else
        {
            PauseGame();
        }
    }

    private void PauseGame()
    {
        if (pauseScreen != null)
        {
            Cursor.lockState = CursorLockMode.Confined;
            isPaused = true;
            pauseScreen.SetActive(true);  
            Time.timeScale = 0f;  
            Debug.Log("Game Paused");
        }
    }

    public void ResumeGame()
    {
        if (pauseScreen != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            isPaused = false;
            pauseScreen.SetActive(false);  
            Time.timeScale = 1f; 
            Debug.Log("Game Resumed");
        }
    }

    private void OnEnable()
    {
        if (pauseControl != null) pauseControl.action.Enable();
    }

    private void OnDisable()
    {
        if (pauseControl != null) pauseControl.action.Disable();
    }
}
