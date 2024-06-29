using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LayerObject))]
[RequireComponent(typeof(Renderer))]
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

    private List<Material> originalMaterials = new List<Material>();
    private List<Material> instanceMaterials = new List<Material>();

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
        meshRenderers = FillListWithComponentsInChildren<MeshRenderer>();

        colliders = FillListWithComponentsInChildren<Collider>();


        for (int i = 0; i < meshRenderers.Count; i++)
        {
            var materials = meshRenderers[i].sharedMaterials;
            for (int j = 0; j < materials.Length; j++)
            {
                Material originalMaterial = materials[j];
                originalMaterials.Add(originalMaterial);

                // Create a new instance of the material
                Material newMat = new Material(originalMaterial);
                // Set the rendering mode to transparent
                SetMaterialTransparent(newMat);

                instanceMaterials.Add(newMat);
                materials[j] = newMat;  // Apply the new material to the renderer
            }
            meshRenderers[i].sharedMaterials = materials;
        }
    }

/*    private void OnValidate() //to see changes from the inspector
    {
        Transparency = _transparency; //to upd value in inspector
        UpdateTransparency();
    }*/

    private void SetMaterialTransparent(Material material)
    {
        material.SetFloat("_Mode", 3); // Set to Transparent
        material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        material.SetInt("_ZWrite", 0);
        material.DisableKeyword("_ALPHATEST_ON");
        material.EnableKeyword("_ALPHABLEND_ON");
        material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
    }

    private void UpdateTransparency()
    {

        //double check value & assign collision state
        if (_transparency <= 0f)
        {
            _transparency = 0f;
            collisionState = transparentCollision ? CollisionState.IsCollidable : CollisionState.NonCollidable;
        }
        else if (_transparency == 0.5f)
        {
            collisionState = semiTransparentCollision ? CollisionState.IsCollidable : CollisionState.NonCollidable;
        }
        else if (_transparency >= 1f)
        {
            _transparency = 1f;
            collisionState = CollisionState.IsCollidable;
        }

        for (int i = 0; i < instanceMaterials.Count; i++)
        {
            Color color = instanceMaterials[i].color;
            color.a = _transparency;
            instanceMaterials[i].color = color;
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
