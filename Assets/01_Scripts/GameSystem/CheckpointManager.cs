using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    [SerializeField] private Stack<Checkpoint> checkpointStack = new Stack<Checkpoint>();
    public static CheckpointManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Found more than one CheckpointManager in the scene.");
        }
        instance = this;
    }

    public void AddCheckpoint(Checkpoint newCheckpoint)
    {
        checkpointStack.Push(newCheckpoint);
    }

    public Checkpoint GetLastCheckpoint()
    {
        if (checkpointStack.Count > 0)
        {
            return checkpointStack.Peek();
        }
        else
        {
            return null;
        }
    }
}
