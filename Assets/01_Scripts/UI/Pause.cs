using UnityEngine;
using UnityEngine.InputSystem;

public class Pause : MonoBehaviour
{
    [SerializeField] public InputActionReference pauseControl;  
    [SerializeField] public GameObject pauseScreen;  

    private bool isPaused = false;

    private void Awake()
    {
        pauseControl.action.Enable();
    }

    private void Update()
    {
        CheckPauseInput();
    }

    private void CheckPauseInput()
    {
        if (pauseControl.action.triggered)
        {
            TogglePauseMenu();
        }
    }

    private void TogglePauseMenu()
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
        Cursor.lockState = CursorLockMode.Confined;
        isPaused = true;
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        isPaused = false;
        pauseScreen.SetActive(false);
        Time.timeScale = 1f;
    }

    private void OnEnable()
    {
        pauseControl.action.Enable();
    }

    private void OnDisable()
    {
        pauseControl.action.Disable();
    }
}