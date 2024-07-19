using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventory : MonoBehaviour
{
    /*    [SerializeField] private int numberOfEyes;
        public int NumberOfEyes { get => numberOfEyes; 
            set {
                value = value < 0 ? 0 : value;
                numberOfEyes = value; 
            } }

        public UnityEvent<PlayerInventory> OnEyeCollected;
        public UnityEvent<PlayerInventory> OnEyeUsed;*/

    public bool hasKey = false;

    public GameObject KeyUI;

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

        KeyUI.SetActive(false);

/*        if (OnEyeCollected == null)
            OnEyeCollected = new UnityEvent<PlayerInventory>();

        if (OnEyeUsed == null)
            OnEyeUsed = new UnityEvent<PlayerInventory>();
*/
    }

    public void AddKey()
    {
        hasKey = true;

        KeyUI.SetActive(true);
    }

    public void UseKey()
    {
        hasKey = false;

        KeyUI.SetActive(false);
    }

/*    public void EyeCollected()
    {
        NumberOfEyes++;
        OnEyeCollected.Invoke(this);
    }

    public void EyeUsed()
    {
        NumberOfEyes--;
        OnEyeCollected.Invoke(this);
    }*/
}
