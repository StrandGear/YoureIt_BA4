using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private CharacterController playerController; 
    [SerializeField] private Transform respawnPoint;

    private Transform initialRespawnPoint;

    private static float yPos; //always keeping y pos the same

    private void Start()
    {
        yPos = gameObject.transform.position.y;

        if (playerController == null)
        {
            playerController = GetComponent<CharacterController>();
        }

        initialRespawnPoint = respawnPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Hole"))
        {
            RespawnPlayer();
            print("Player Respawned");
            
        }
    }

    private void RespawnPlayer()
    {
        AssignNewRespawnPosition();

        playerController.enabled = false; 
        playerController.transform.position = new Vector3(respawnPoint.position.x, yPos, respawnPoint.position.z);
        playerController.transform.rotation = respawnPoint.rotation;
        playerController.enabled = true;

        Singleton.GetInstance<PlayerScan>().StopScanning();
    }

    private void AssignNewRespawnPosition()
    {
        if (Singleton.GetInstance<CheckpointManager>().GetLastCheckpoint().CheckpointPosition == null)
            respawnPoint = initialRespawnPoint;
        else
            respawnPoint = Singleton.GetInstance<CheckpointManager>().GetLastCheckpoint().CheckpointPosition;
    }
}