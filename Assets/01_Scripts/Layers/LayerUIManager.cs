using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LayerUIManager : MonoBehaviour
{
    [SerializeField] private GameObject selectedLayerFrame;

    [Header("Object sprite represention")]
    [SerializeField] private GameObject itemImageButton; //image itself is assigned dynamically
    public GameObject ItemImageButton { get => itemImageButton; }

    [Header("Hide/unhide toggle")]
    [SerializeField] private GameObject eyeToggle;
    public GameObject EyeToggle { get => eyeToggle; }

    [SerializeField] private GameObject inactiveEyeToggleObject;

    [Header("Lock toggle")]
    [SerializeField] private GameObject lockToggle;
    public GameObject LockToggle { get => lockToggle; }

    [SerializeField] private GameObject inactiveLockToggleObject;

    /*    [SerializeField] private GameObject linkToggle;
        public GameObject LinkToggle { get => linkToggle; }*/

    public void SetEyeToggleInactive()
    {
        for (int i = 0; i < eyeToggle.transform.childCount; i++)
        {
            eyeToggle.transform.GetChild(i).gameObject.SetActive(false);
        }
        inactiveEyeToggleObject.SetActive(true);
    }
    public void SetLockToggleInactive()
    {
        for (int i = 0; i < lockToggle.transform.childCount; i++)
        {
            lockToggle.transform.GetChild(i).gameObject.SetActive(false);
        }
        inactiveLockToggleObject.SetActive(true);
    }

        public void ShowLayerFrame(bool show)
    {
        selectedLayerFrame.SetActive(show);
    }
}
