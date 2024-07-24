using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSceneAfterTimeline : MonoBehaviour
{
    CutsceneManager cutsceneManager = null;
    private void Start()
    {
        cutsceneManager = GetComponent<CutsceneManager>();
    }
    private void Update()
    {
        if (cutsceneManager.CutSceneIsDone)
            SceneManager.LoadScene(3);
    }
}
