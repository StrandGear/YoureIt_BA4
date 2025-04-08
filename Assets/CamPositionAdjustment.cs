using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPositionAdjustment : MonoBehaviour
{
    private void OnEnable()
    {
        transform.position = new Vector3(-39.416935f, 0.603665471f, 32.3886452f);
        transform.rotation = Quaternion.Euler(-4.07f, -146.11f, 0f);
    }
}
