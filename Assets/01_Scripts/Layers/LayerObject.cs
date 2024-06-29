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
    public int ID { get; private set; }

    public Vector3 StartPosition { get; set; }
    public Vector3 CurrentFixedPosition { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    [SerializeField] private Sprite objectSprite;
    public Sprite ObjectSprite { get => objectSprite; set => objectSprite = value; }

    private void OnValidate()
    {
        Name = objectName;

        StartPosition = transform.position;

        ObjectSprite = objectSprite;

        ID = GetInstanceID();

        // for the future because GetInstaneID changes every time
        //int id = Guid.NewGuid().GetHashCode(); 

    }

    private void Awake()
    {
        Name = objectName;

        StartPosition = transform.position;

        ObjectSprite = objectSprite;

        ID = GetInstanceID();
    }

    public void ResetPosition()
    {
        throw new NotImplementedException();
    }
}
