using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerNextDialogue : MonoBehaviour
{
    private bool isTriggered = false;
    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;

        GetComponent<MeshRenderer>().enabled = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!isTriggered)
        {
            if (other.GetComponent<CharacterController>() != null)
            {
                Singleton.GetInstance<DialogueSoundManager>().PlayNextDialogueSequence();
                isTriggered = true;
            }
        }
    }
}
