using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerNextDialogue : MonoBehaviour
{
    public bool isTriggered { get; set; }
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
                DialogueSoundManager.Instance.PlayNextDialogueSequence();
                isTriggered = true;
            }
        }
    }
}
