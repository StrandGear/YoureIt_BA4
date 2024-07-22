using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ResetDialogueTriggers : MonoBehaviour
{
    public List<TriggerNextDialogue> DialoguesToReset;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;

        GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            foreach (TriggerNextDialogue elem in DialoguesToReset)
            {
                elem.isTriggered = false;
            }
        }
    }
}
