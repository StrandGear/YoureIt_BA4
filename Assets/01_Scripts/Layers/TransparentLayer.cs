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
    
    [SerializeField][HideInInspector] private MeshRenderer _renderer;

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

    private Collider _collider;


    private void Awake()
    {
        if (_renderer == null)
            TryGetComponent<MeshRenderer>(out _renderer);
        if (_collider == null)
            TryGetComponent<Collider>(out _collider);
    }
    private void OnValidate() //to see changes from the inspector
    {
        if (_renderer == null)
            TryGetComponent<MeshRenderer>(out _renderer);
        if (_collider == null)
            TryGetComponent<Collider>(out _collider);

        Transparency = _transparency; //to upd value in inspector
        UpdateTransparency();
    }

    private void UpdateTransparency()
    {
        if (_renderer != null)
        {
            var color = _renderer.sharedMaterial.color;
            color.a = _transparency;
            _renderer.sharedMaterial.color = color;
        }

        //double check value & assign collision state
        if (_transparency <= 0f)
        {
            _transparency = 0f;
            _renderer.enabled = false;
            collisionState = transparentCollision ? CollisionState.IsCollidable : CollisionState.NonCollidable;
        }
        else if (_transparency == 0.5f)
        {
            _renderer.enabled = true;
            collisionState = semiTransparentCollision ? CollisionState.IsCollidable : CollisionState.NonCollidable;
        }
        else if (_transparency >= 1f)
        {
            _renderer.enabled = true;
            _transparency = 1f;
            collisionState = CollisionState.IsCollidable;
        }

        UpdateCollisionStatus(collisionState);
    }

    private void UpdateCollisionStatus(CollisionState state)
    {
        if (state == CollisionState.IsCollidable)
        {
            _collider.isTrigger = false;
            return;
        }
        else if (state == CollisionState.NonCollidable)
        {
            _collider.isTrigger = true;
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
}
