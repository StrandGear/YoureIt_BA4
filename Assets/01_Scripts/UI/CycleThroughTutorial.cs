using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleThroughTutorial : MonoBehaviour
{
    [SerializeField] private GameObject[] gameObjects;
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        UpdateObjects();
    }

    // Update is called once per frame
    void UpdateObjects()
    {
        for (int i = 0; i < gameObjects.Length; i++)
        {
            if (i == currentIndex)
            {
                gameObjects[i].SetActive(true);
            }
            else
            {
                gameObjects[i].SetActive(false);
            }
        }
    }
    
    public void NextObject()
    {
        currentIndex = (currentIndex + 1) % gameObjects.Length;
        UpdateObjects();
        
    }

    public void PreviousObject()
    {
        currentIndex = (currentIndex - 1 + gameObjects.Length) % gameObjects.Length;
        UpdateObjects();
        
    }
}
