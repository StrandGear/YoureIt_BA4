using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool activated = false;

    //private Transform checkpointPosition;

    //public Transform CheckpointPosition { get => checkpointPosition;}

    private void Awake()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<CharacterController>() != null && !activated)
        {
            LayerManager.Instance.SetAllObjectsAsUsed();
            LayerManager.Instance.ClearLayerList();

            //Singleton.GetInstance<PlayerScan>().StopScanning();
            Singleton.GetInstance<GameStates>().SetGameState(GameState.Playmode);

            activated = true;

            //checkpointPosition = gameObject.transform;

            Singleton.GetInstance<CheckpointManager>().AddCheckpoint(this);
        }
    }

    public Transform CheckpointPosition
    {
        get { return this.transform; }
    }
}
