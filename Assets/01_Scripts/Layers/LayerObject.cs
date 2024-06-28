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
    [SerializeField]
    private bool isUsed = false;
    public bool IsUsed { get => isUsed; set => isUsed = value; }

    public int ID => GetInstanceID();

    private void OnValidate()
    {
        Name = objectName;

        // for the future because GetInstaneID changes every time
        //int id = Guid.NewGuid().GetHashCode(); 

    }
}
