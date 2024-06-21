using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    private Rigidbody rigidbodyComponent;

    private void Start()
    {
        TryGetComponent(out rigidbodyComponent);
    }

    public void StopMovement()
    {

    }
    public void StartMovement()
    {

    }
}
