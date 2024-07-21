using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerObject : MonoBehaviour, ILayerObject
{
    private string objectName = "";
    public string Name { get => objectName; set => objectName = value; }

    public GameObject LayerGameObject => gameObject;

    [SerializeField] private bool isUsed = false;
    public bool IsUsed { get => isUsed; set => isUsed = value; }
    public int ID { get; private set; }

    //public Vector3 StartPosition { get; set; }
    public Vector3 CurrentFixedPosition { get; set; }

    [SerializeField] private Sprite objectSprite;
    public Sprite ObjectSprite { get => objectSprite; set => objectSprite = value; }

     private Transform shader;

    private void OnValidate()
    {
        Name = objectName;

        //CurrentFixedPosition = transform.position;

        ObjectSprite = objectSprite;

        ID = GetInstanceID();

        // for the future because GetInstaneID changes every time
        //int id = Guid.NewGuid().GetHashCode(); 

    }

    private void Awake()
    {
        Name = objectName;

        CurrentFixedPosition = transform.position;

        ObjectSprite = objectSprite;

        ID = GetInstanceID();

        if (shader == null)
        {
            shader = FindShaderObject(transform, "shader");
        }
    }

    private void Start()
    {
        SetShaderActive(false);
    }

    public void ResetPosition()
    {
        throw new NotImplementedException();
    }

    public void SetNewPosition(Vector3 newPosition)
    {
        //float currentZPosition = CurrentFixedPosition.z;

        CurrentFixedPosition = new Vector3 (newPosition.x, CurrentFixedPosition.y, newPosition.z);

        transform.position = CurrentFixedPosition;
    }

    public void SetShaderActive(bool active)
    {
        if (shader != null)
            shader.gameObject.SetActive(active);
    }

    //Utilities
    private Transform FindShaderObject(Transform parent, string postfix)
    {
        foreach (Transform child in parent)
        {
            if (child.name.ToLower().Contains(postfix.ToLower()))
            {
                return child;
            }

            Transform found = FindShaderObject(child, postfix);
            if (found != null)
            {
                return found;
            }
        }
        return null;
    }

}
