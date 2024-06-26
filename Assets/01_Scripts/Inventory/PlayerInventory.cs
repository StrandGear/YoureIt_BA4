using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfEyes { get; private set; }

    public UnityEvent<PlayerInventory> OnEyeCollected;

    private void Awake()
    {
        if (OnEyeCollected == null)
            OnEyeCollected = new UnityEvent<PlayerInventory>();
    }

    public void EyeCollected()
    {
        NumberOfEyes++;
        OnEyeCollected.Invoke(this);
    }
}
