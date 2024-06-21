using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerOrder : MonoBehaviour, ILayerOrder
{
    public Transform obj1;
    public Transform obj2;
    public bool swap = false;
    public float desiredDepth = 3;

    [Tooltip("Distance between layers")]
    [SerializeField] private float zOffset = 0.1f;
    int ILayerOrder.LayerOrder { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

    private List<Transform> objects = new List<Transform>();

    private void Awake()
    {
        objects.Add(obj1);
        objects.Add(obj2);
    }
    
    public void UpdateObjectDepth (Transform obj, float desiredDepth)
    {
        float targetZ = desiredDepth * zOffset;
        bool positionIsFree = true;

        foreach (Transform other in objects)
        {print(positionIsFree);
            if (other != obj && (Mathf.Abs(targetZ - other.position.z) < zOffset))
            {
                positionIsFree = false;
                break;
            }
        }

        if (positionIsFree)
        {
            Vector3 newPosition = obj.position;
            newPosition.z = targetZ;
            obj.position = newPosition;
        }
        else
        {
            // Handle what happens if the position is not free (e.g., find the next available spot)
        }
    }

    public void SwapPositions(Transform obj1, Transform obj2)
    {
        Vector3 obj1Pos = obj1.position;
        Vector3 obj2Pos = obj2.position;

        obj1.position = obj2Pos;
        obj2.position = obj1Pos;
    }

    private void Update()
    {
        /*        if (swap)
                    UpdateObjectDepth(obj1, desiredDepth);
                else
                    UpdateObjectDepth(obj1, -desiredDepth);*/
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            SwapPositions(obj1, obj2);
        }

    }
}

