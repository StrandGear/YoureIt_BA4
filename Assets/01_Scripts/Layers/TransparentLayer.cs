using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransparentLayer : MonoBehaviour, ITransparency
{
    [Header("Transparency values")]
    [SerializeField, Range(0f, 1f)]
    private float _transparency;
    private float[] snappingPoints = new float[] { 0f, 0.5f, 1f };

    public float Transparency
    {
        get => _transparency;
        set
        {
            _transparency = SnapValue(value, snappingPoints);
            UpdateTransparency();
        }
    }
    
    [SerializeField][HideInInspector] private List<MeshRenderer> meshRenderers = new List<MeshRenderer>();

    [Header("Collision values")]
    [Tooltip("Has Collision if transparent")]
    [SerializeField] bool transparentCollision = false;
    [Tooltip("Has Collision if semi-transparent")]
    [SerializeField] bool semiTransparentCollision = false;
    private enum CollisionState
    {
        IsCollidable,
        NonCollidable
    }

    private CollisionState collisionState = CollisionState.IsCollidable;

    private List<Collider> colliders = new List<Collider>();


    private void Awake()
    {
        /*        if (_renderer == null)
                    TryGetComponent<MeshRenderer>(out _renderer);
                if (_collider == null)
                    TryGetComponent<Collider>(out _collider);*/

        meshRenderers = FillListWithComponentsInChildren<MeshRenderer>();
        colliders = FillListWithComponentsInChildren<Collider>();

    }
    private void OnValidate() //to see changes from the inspector
    {
        /*        if (_renderer == null)
                    TryGetComponent<MeshRenderer>(out _renderer);
                if (_collider == null)
                    TryGetComponent<Collider>(out _collider);*/

        meshRenderers = FillListWithComponentsInChildren<MeshRenderer>();
        colliders = FillListWithComponentsInChildren<Collider>();

        Transparency = _transparency; //to upd value in inspector
        UpdateTransparency();
    }

    private void UpdateTransparency()
    {
        bool rendererEnabled = true;

        //double check value & assign collision state
        if (_transparency <= 0f)
        {
            _transparency = 0f;
            rendererEnabled = false;
            collisionState = transparentCollision ? CollisionState.IsCollidable : CollisionState.NonCollidable;
        }
        else if (_transparency == 0.5f)
        {
            rendererEnabled = true;
            collisionState = semiTransparentCollision ? CollisionState.IsCollidable : CollisionState.NonCollidable;
        }
        else if (_transparency >= 1f)
        {
            rendererEnabled = true;
            _transparency = 1f;
            collisionState = CollisionState.IsCollidable;
        }

        foreach (MeshRenderer elem in meshRenderers)
        {
            
            var color = elem.sharedMaterial.color;
            color.a = _transparency;
            elem.sharedMaterial.color = color;
            elem.enabled = rendererEnabled;
        }

        UpdateCollisionStatus(collisionState);
    }

    private void UpdateCollisionStatus(CollisionState state)
    {
        if (state == CollisionState.IsCollidable)
        {
            foreach (Collider elem in colliders)
                elem.isTrigger = false;
            return;
        }
        else if (state == CollisionState.NonCollidable)
        {
            foreach (Collider elem in colliders)
                elem.isTrigger = true;
            return;
        }    
    }

    private float SnapValue(float value, float[] snapPoints)
    {
        float closest = snapPoints[0];
        float minDifference = Mathf.Abs(value - closest);

        foreach (float snapPoint in snapPoints)
        {
            float difference = Mathf.Abs(value - snapPoint);

            if (difference < minDifference)
            {
                closest = snapPoint;
                minDifference = difference;
            }
        }
        return closest;
    }

    private List<T> FillListWithComponentsInChildren<T>()
    {
        List<T> tempList = new();

        tempList.Add(gameObject.GetComponent<T>());

        if (transform.childCount > 0)
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                tempList.Add(transform.GetChild(i).transform.GetComponent<T>());
            }
        }

        return tempList;
    }
}
