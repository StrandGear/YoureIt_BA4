using FMODUnity;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LayerObject))]

public class LockingLayer : MonoBehaviour, ILockable
{
    [field: SerializeField] public EventReference SoundToPlayWhenStateChanged { get; private set; }

    UnityEvent onObjectStateChange = new UnityEvent();

    [SerializeField] private bool isLocked;
    public bool IsLocked { 
        get => isLocked; 
        set
        {
            isLocked = value;
            LockLayer();
        } }

    public List<Animator> ConnectedAnimations = new List<Animator>();

    private void Awake()
    {
        IsLocked = IsLocked;

        onObjectStateChange.AddListener(PlaySound);
    }

    public void LockLayer()
    {
        onObjectStateChange.Invoke();

        if (IsLocked)
        {
            gameObject.GetComponent<Animator>().speed = 0; // Pause the animation

            foreach (Animator elem in ConnectedAnimations)
            {
                elem.GetComponent<Animator>().speed = 0;
            }
        }
        else
        {
            gameObject.GetComponent<Animator>().speed = 1; // Resume the animation

            foreach (Animator elem in ConnectedAnimations)
            {
                elem.GetComponent<Animator>().speed = 1;
            }
        }
    }

    void PlaySound()
    {
       AudioManager.instance.PlayOneShot(SoundToPlayWhenStateChanged, gameObject.transform.position);
    }
}
