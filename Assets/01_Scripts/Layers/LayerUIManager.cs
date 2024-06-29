using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject selectedLayerFrame;
    //public GameObject SelectedLayerFrame { get => selectedLayerFrame; }

    [SerializeField] private GameObject itemImageButton; //image itself is assigned dynamically
    public GameObject ItemImageButton { get => itemImageButton; }

    [SerializeField] private GameObject eyeToggle;
    public GameObject EyeToggle { get => eyeToggle; }

    [SerializeField] private GameObject lockToggle;
    public GameObject LockToggle { get => lockToggle; }

    [SerializeField] private GameObject linkToggle;
    public GameObject LinkToggle { get => linkToggle; }

    private void Awake()
    {
        
    }

    public void ShowLayerFrame(bool show)
    {
        selectedLayerFrame.SetActive(show);
    }
}
