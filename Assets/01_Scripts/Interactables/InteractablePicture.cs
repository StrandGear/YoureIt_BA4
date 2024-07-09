using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(InteractableObject))]
public class InteractablePicture : InteractableObject
{
    public Sprite drawing;

    private GameObject uiGameObjectToRenderPicture;

    private void Start()
    {
        uiGameObjectToRenderPicture = transform.Find("UICanvas").gameObject.GetComponentInChildren<Image>().gameObject;
        uiGameObjectToRenderPicture.GetComponentInChildren<Image>().sprite = drawing;
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
