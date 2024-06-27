using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    public int NumberOfEyes { get; private set; }

    public UnityEvent<PlayerInventory> OnEyeCollected;
    public UnityEvent<PlayerInventory> OnEyeUsed;

    private static PlayerInventory instance = null;
    public static PlayerInventory Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerInventory>();
                if (instance == null)
                {
                    GameObject go = new GameObject("LayerManager");
                    instance = go.AddComponent<PlayerInventory>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (OnEyeCollected == null)
            OnEyeCollected = new UnityEvent<PlayerInventory>();

        if (OnEyeUsed == null)
            OnEyeUsed = new UnityEvent<PlayerInventory>();
    }

    public void EyeCollected()
    {
        NumberOfEyes++;
        OnEyeCollected.Invoke(this);
    }

    public void EyeUsed()
    {
        NumberOfEyes--;
        OnEyeCollected.Invoke(this);
    }
}
