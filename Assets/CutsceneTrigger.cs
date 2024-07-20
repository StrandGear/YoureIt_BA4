using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class CutsceneTrigger : MonoBehaviour
{
    //public static event System.Action OnPlayerEnterTrigger;

    private void Start()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CharacterController>() != null)
        {
            //OnPlayerEnterTrigger?.Invoke();
            OnPlayerEnterTrigger();
        }
    }

   public void OnPlayerEnterTrigger() { }
}
