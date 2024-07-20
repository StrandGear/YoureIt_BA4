using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRespawn : MonoBehaviour
{
    [SerializeField] private CharacterController playerController; 
    [SerializeField] private Transform respawnPoint;

    public bool keepXposStatic = false;
    public bool keepYposStatic = true;
    public bool keepZposStatic = false; // for second level
    public event Action onPlayerRespawn;

    private Vector3 initialRespawnPoint;

    private static float xPos; //always keeping y pos the same
    private static float yPos; //always keeping y pos the same
    private static float zPos; //always keeping z pos the same

    private void Start()
    {
        xPos = gameObject.transform.position.x;
        yPos = gameObject.transform.position.y;
        zPos = gameObject.transform.position.z;

        initialRespawnPoint = gameObject.transform.position;

        if (playerController == null)
        {
            playerController = GetComponent<CharacterController>();
        }

        respawnPoint.position = initialRespawnPoint;

        AssignNewRespawnPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") || other.CompareTag("Hole"))
        {
            print("enemy hit");
            AssignNewRespawnPosition();
            RespawnPlayer();            
        }
    }

    private void RespawnPlayer()
    {
        playerController.enabled = false; 
        playerController.transform.position = respawnPoint.position;
        playerController.transform.rotation = respawnPoint.rotation;
        playerController.enabled = true;
        onPlayerRespawn?.Invoke();
        Singleton.GetInstance<GameStates>().SetGameState(GameState.Playmode);
    }

    private void AssignNewRespawnPosition()
    {
        var checkpointManager = CheckpointManager.instance;
        respawnPoint.position = initialRespawnPoint;
        print(respawnPoint);
        
        if (checkpointManager != null)
        {
            var lastCheckpoint = checkpointManager.GetLastCheckpoint();
            if (lastCheckpoint != null)
            {
                print(lastCheckpoint);
                float tempXpos = lastCheckpoint.CheckpointPosition.position.x;
                float tempYpos = lastCheckpoint.CheckpointPosition.position.y;
                float tempZpos = lastCheckpoint.CheckpointPosition.position.z;

                if (keepXposStatic)
                    tempYpos = xPos;

                if (keepYposStatic)
                    tempYpos = yPos;

                if (keepZposStatic)
                    tempZpos = zPos;

                respawnPoint.position = new Vector3(tempXpos, tempYpos, tempZpos);
                respawnPoint.rotation = lastCheckpoint.PlayerRotation;
            }
            else
            {
                //Debug.LogWarning("No checkpoint found. Using initial respawn point.");
                respawnPoint.position = initialRespawnPoint;
            }
        }
        else
        {
            //Debug.LogError("CheckpointManager instance is not available.");
            respawnPoint.position = initialRespawnPoint;
        }
    }
}
