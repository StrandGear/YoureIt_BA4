using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadCutsceneOnLevelLoad : MonoBehaviour
{
    public bool LoadCutsceneOnGameStart = false;
    public int cutsceneID = 0;

    private void Start()
    {
        if (LoadCutsceneOnGameStart)
            Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(cutsceneID);
    }
}
