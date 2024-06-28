using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerObject : MonoBehaviour, ILayerObject
{
    [SerializeField]
    private string objectName = "";
    public string Name { get => objectName; set => objectName = value; }

    public GameObject LayerGameObject => gameObject;

    private bool isUsed = false;
    public bool IsUsed { get => isUsed; set => isUsed = value; }

    public int ID => GetInstanceID();

    public Vector3 StartPosition { get; set; }
    public Vector3 CurrentFixedPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    private void OnValidate()
    {
        Name = objectName;

        StartPosition = transform.position;

        // for the future because GetInstaneID changes every time
        //int id = Guid.NewGuid().GetHashCode(); 

    }

    public void ResetPosition()
    {
        throw new NotImplementedException();
    }
}
