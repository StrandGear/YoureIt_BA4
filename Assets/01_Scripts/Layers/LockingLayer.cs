using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LayerObject))]

public class LockingLayer : MonoBehaviour, ILockable
{
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
    }

    public void LockLayer()
    {
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
}
