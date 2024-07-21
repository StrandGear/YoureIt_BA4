using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(BoxCollider))]
public class InputCutsceneTrigger : MonoBehaviour
{
    public int cutsceneIndex = -1;
    [SerializeField] private InputActionReference triggerKey;

    private bool triggerIsActivated = false;
    private bool playerEnteredCollider = false;

    private void OnEnable()
    {
        triggerKey.action.Enable();
    }
    private void OnDisable()
    {
        triggerKey.action.Disable();
    }

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void Update()
    {
        if (triggerKey.action.WasPressedThisFrame() && playerEnteredCollider && !triggerIsActivated)
        {
            print("Start cutscene");
            triggerIsActivated = true;
            if (cutsceneIndex >= 0)
            {
                Singleton.GetInstance<CutsceneManager>().PlayCutsceneByIndex(cutsceneIndex);
            }
            else
            {
                Singleton.GetInstance<CutsceneManager>().PlayNextCutscene();
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null && !triggerIsActivated)
        {
            
            playerEnteredCollider = true;
        }
    }
}

