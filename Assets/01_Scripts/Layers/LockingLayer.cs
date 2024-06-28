using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LayerObject))]
public class LockingLayer : MonoBehaviour, ILockable
{
    public bool IsLocked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public void LockLayer()
    {
        throw new NotImplementedException();
    }
}
