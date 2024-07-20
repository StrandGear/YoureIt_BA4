using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Playables;

public class CutsceneManager : Singleton
{
    [SerializeField] public List<CutsceneData> cutscenes;

    private void Awake()
    {
        foreach (CutsceneData elem in cutscenes)
        {
            elem.unityEvent.AddListener(() => PlayCutscene(elem.playableDirector));
        }
    }

    void PlayCutscene(PlayableDirector playableDirector)
    {
        playableDirector.Play();
        print("Should play cutscene");
    }
}

[System.Serializable]
public class CutsceneData
{
    public PlayableDirector playableDirector;

    public UnityEvent unityEvent;
}
