using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CharacterController>() != null)
        {
            LayerManager.Instance.SetAllObjectsAsUsed();
            LayerManager.Instance.ClearLayerList();
        }
    }
}
