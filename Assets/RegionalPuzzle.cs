using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegionalPuzzle : MonoBehaviour
{
    [SerializeField] private List<LayerObject> layerObjects = new List<LayerObject>();
    public List<LayerObject> LayerObjects { get { return layerObjects; } }

    public bool interactive = true;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<MeshRenderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().isTrigger = true;

        if (layerObjects.Count == 0)
            Debug.LogWarning("No layer objects in the regional puzzle assigned.");
        else
        {
            foreach (LayerObject elem in layerObjects)
            {
                elem.IsRegionalPuzzle = true;
            }
        }
    }


}
