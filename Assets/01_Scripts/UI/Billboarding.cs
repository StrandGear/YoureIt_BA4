using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboarding : MonoBehaviour
{
    Vector3 cameraDir;
    [SerializeField] bool inverseRotation = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        cameraDir = Camera.main.transform.forward;
        cameraDir.y = 0;

        if (!inverseRotation)
            transform.rotation = Quaternion.LookRotation(-cameraDir);
        else
            transform.rotation = Quaternion.LookRotation(cameraDir);
    }
}
