using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    float animEnd = 20f;

    float timePassed = 0f;

    public int levelToLoad = 0;

    private void Update()
    {
        timePassed += Time.deltaTime;

        if (timePassed >= animEnd)
            LoadLevel();
    }

    private void LoadLevel()
    {
        Cursor.lockState = CursorLockMode.Confined;
        SceneManager.LoadScene(levelToLoad);
    }
}
