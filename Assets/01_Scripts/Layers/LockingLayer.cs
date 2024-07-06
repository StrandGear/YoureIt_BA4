using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(LayerObject))]

public class LockingLayer : MonoBehaviour, ILockable
{
    public bool IsLocked { get; set ; }

    public void LockLayer()
    {
        throw new NotImplementedException();
    }
}
