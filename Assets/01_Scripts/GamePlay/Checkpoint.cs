using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated = false;
    public EnemyAttack enemyAttack;

    private Quaternion playerRotation;
    //private Transform checkpointPosition;

    //public Transform CheckpointPosition { get => checkpointPosition;}

    private void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        
        if (enemyAttack == null)
        {
            Debug.Log("EnemyAttack reference is not assigned in Checkpoint script.");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CharacterController>() != null )
        {
            LayerManager.Instance.SetAllObjectsAsUsed();
            LayerManager.Instance.ClearLayerList();

            //Singleton.GetInstance<PlayerScan>().StopScanning();
            Singleton.GetInstance<GameStates>().SetGameState(GameState.Playmode);

            activated = true;

            //checkpointPosition = gameObject.transform;

            playerRotation = other.transform.rotation;

            Singleton.GetInstance<CheckpointManager>().AddCheckpoint(this);
            
            if (enemyAttack != null)
            {
                enemyAttack.PlayerExited(); // Call PlayerExited to stop the attack animation
            }
            else
            {
                Debug.Log("EnemyAttack reference is not assigned in Checkpoint script.");
            }
        }
        
        if (other.CompareTag("Player"))
        {
            if (enemyAttack != null)
            {
                enemyAttack.PlayerExited(); // Call PlayerExited to stop the attack animation
            }
            else
            {
                Debug.Log("EnemyAttack reference is not assigned in Checkpoint script.");
            }
        }
    }

    public Transform CheckpointPosition
    {
        get { return this.transform; }
    }

    public Quaternion PlayerRotation // Add this property
    {
        get { return playerRotation; }
    }
}
