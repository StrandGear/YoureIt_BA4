using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableEye : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //other.TryGetComponent(out PlayerInventory playerInventory);

        PlayerInventory playerInventory = other.GetComponentInChildren<PlayerInventory>(); 

        if (playerInventory != null)
        {
            playerInventory.EyeCollected();
            Destroy(gameObject);
        }
    }
}
