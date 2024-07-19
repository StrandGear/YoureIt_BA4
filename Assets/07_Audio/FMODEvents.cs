using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMODUnity;

public class FMODEvents : MonoBehaviour

{
    [field: Header("Ambience")]
    [field: SerializeField] public EventReference ambience { get; set; }

    [field: Header("Music")]
    [field: SerializeField] public EventReference music { get; private set; }

    [field: Header("Player SFX")]
    [field: SerializeField] public EventReference playerFootsteps {  get; set; }

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

    [field: Header("Dialogues Level 1")]
    [field: SerializeField] public EventReference Dialogue_DidntDoRight { get; private set; }
    [field: SerializeField] public EventReference Dialogue_DidntMeanIt { get; private set; }
    [field: SerializeField] public EventReference Dialogue_DontBeLikeThis { get; private set; }
    [field: SerializeField] public EventReference Dialogue_IDontLikePlayingAlone { get; private set; }
    [field: SerializeField] public EventReference Dialogue_IllFindYouAnyway{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_ItWasAnAccident{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_MadAtMe{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_MyOnlyFriend{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_WeCanPlayTogether{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_WhereAreYou{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_WhereAreYouHiding{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_WhereIsSheHiding{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_WhyAreYouHiding{ get; private set; }
    
    [field: Header("Dialogues Level 2")]
    [field: SerializeField] public EventReference Dialogue_ComingForYouIris { get; private set; }
    [field: SerializeField] public EventReference Dialogue_DontWantyou{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_HideAway { get; private set; }
    [field: SerializeField] public EventReference Dialogue_IDontLikePlayingAlone2 { get; private set; }
    [field: SerializeField] public EventReference Dialogue_IllDoItAgain{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_ImScared2{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_ImScared3{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_LetsPlay{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_NobodyLikesYou{ get; private set; }
    
    [field: Header("Dialogues Level 3")]
    [field: SerializeField] public EventReference Dialogue_IFoundYou{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_ImScared{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_IsThatMe{ get; private set; }
    [field: SerializeField] public EventReference Dialogue_IsThatYou{ get; private set; }
    
    [field: Header("UI Dialogues")]
    [field: SerializeField] public EventReference UIDialogue_Breathing{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_CheckDrawings{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_Crying{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_Gasp{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_ICanDoThat{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_INeedToFindAWay{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_INeedToLookForAClue{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_ItIsLocked{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_Laughing{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_NeedKey{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_NeedToFindLocker{ get; private set; }
    [field: SerializeField] public EventReference UIDialogue_StupidItsBlocked{ get; private set; }
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

