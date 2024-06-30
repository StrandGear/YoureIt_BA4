using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private CharacterController playerController; 
    [SerializeField] private Transform respawnPoint;

    private void Start()
    {
        
        if (playerController == null)
        {
            playerController = GetComponent<CharacterController>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            RespawnPlayer();
            print("Player Respawned");
            
        }
    }

    private void RespawnPlayer()
    {
        
        playerController.enabled = false; 
        playerController.transform.position = respawnPoint.position;
        playerController.transform.rotation = respawnPoint.rotation;
        playerController.enabled = true; 

        
    }
}