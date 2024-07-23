using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractableObject))]
public class InteractablePicture : InteractableObject
{
    [Tooltip("This is gonna be displayed as a drawing on UI")]
    public Sprite UIdrawing;    

    private GameObject uiGameObjectToRenderPicture; //normal UI 

    private void Start()
    {
        uiGameObjectToRenderPicture = transform.Find("UICanvas").gameObject.GetComponentInChildren<Image>().gameObject;
        uiGameObjectToRenderPicture.GetComponentInChildren<Image>().sprite = UIdrawing;
        StopInteraction();
    }
    public override void Interact()
    {
        base.Interact();

        transform.Find("UICanvas").gameObject.SetActive(true);
    }

    public override void StopInteraction()
    {
        base.StopInteraction();

        transform.Find("UICanvas").gameObject.SetActive(false);
    }
}
