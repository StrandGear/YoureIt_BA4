using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : Singleton
{
    [SerializeField] private Stack<Checkpoint> checkpointStack = new Stack<Checkpoint>();

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
