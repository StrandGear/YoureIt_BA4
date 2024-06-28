using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockingLayer : MonoBehaviour, ILockable
{
    [SerializeField] private bool layerLocked = false;
    public bool IsLocked { get => layerLocked; set {
            layerLocked = value;
            UpdateLayerLocking();
        } }

    private void Start()
    {
    }

    private void UpdateLayerLocking()
    {
    }
}
