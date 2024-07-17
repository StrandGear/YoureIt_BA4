using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour

{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public List<EventReference> playerFootsteps {  get; set; }

    [field: Header("Eye SFX")]
    

    [field: Header("UI SFX")]
    [field: SerializeField] public EventReference UI_selectElement { get; private set; }
    [field: SerializeField] public EventReference UI_puzzleSuccess { get; private set; }
    [field: SerializeField] public EventReference UI_paperUnfolding { get; private set; }
    [field: SerializeField] public EventReference UI_cantSelect { get; private set; }

    [field: Header("General SFX")]
    [field: SerializeField] public EventReference RussianDoll { get; private set; }
    [field: SerializeField] public EventReference PlankFalling { get; private set; }
    [field: SerializeField] public EventReference Keys { get; private set; }
    [field: SerializeField] public EventReference Door { get; private set; }
    [field: SerializeField] public EventReference BackpackZipper { get; private set; }
    [field: SerializeField] public EventReference eyeCollected { get; private set; }
    public static FMODEvents instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one FMOD Events instance in the scene.");
        }
        instance = this;
    }
}

