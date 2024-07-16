using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class LayerManager2 : MonoBehaviour
{
    [SerializeField] private GameObject layerUIPrefab;
    [SerializeField] private Transform uiParent;
    [SerializeField] private Canvas canvas;

    private int startDragIndex;

    public Canvas Canvas { get => canvas; }

    private ToggleButton activeLayer;
    private bool layerLocked = false;

    [SerializeField] private List<LayerData> layers = new List<LayerData>();
    public List<LayerData> Layers { get => layers; }

    private LayerData currentlySelectedLayerData;

    public LayerData CurrentlySelectedLayerData
    {
        get { return currentlySelectedLayerData; }
        set { currentlySelectedLayerData = value; }
    }

    [System.Serializable]
    public class LayerData
    {
        public int id;
        public GameObject gameObject;
        public LayerObject layerObject;
        public RectTransform uiElement;
        public LayerUIManager2 layerUIManager;
        public string name;
    }

    private static LayerManager2 instance = null;
    public static LayerManager2 Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LayerManager2>();
                if (instance == null)
                {
                    GameObject go = new GameObject("LayerManager");
                    instance = go.AddComponent<LayerManager2>();
                }
            }

            return instance;
        }
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddLayer(ILayerObject obj)
    {
        foreach (LayerData elem in layers)
        {
            if (elem.id == obj.ID || obj.IsUsed)
            {
                return;
            }
        }

        LayerData layerData = new LayerData
        {
            id = obj.ID,
            gameObject = obj.LayerGameObject,
            layerObject = (LayerObject)obj,
            name = obj.Name
        };
        layers.Add(layerData);

        UpdateUI();
    }

    private void UpdateUI()
    {
        foreach (Transform child in uiParent)
        {
            Destroy(child.gameObject);
        }

        foreach (LayerData layer in layers)
        {
            GameObject uiElement = Instantiate(layerUIPrefab, uiParent);
            layer.uiElement = uiElement.GetComponent<RectTransform>();
            layer.layerUIManager = uiElement.GetComponent<LayerUIManager2>();

            if (layer.layerObject.ObjectSprite != null)
                layer.layerUIManager.ItemImageButton.GetComponent<Image>().sprite = layer.layerObject?.ObjectSprite;

            // Setting default values
            layer.layerObject.TryGetComponent(out HidingLayer hidingLayer);
            if (hidingLayer != null)
            {
                layer.layerUIManager.EyeToggle.GetComponent<Toggle>().isOn = !hidingLayer.IsHidden;
            }

            layer.layerObject.TryGetComponent(out LockingLayer lockingLayer);
            if (lockingLayer != null)
            {
                layer.layerUIManager.LockToggle.GetComponent<Toggle>().isOn = lockingLayer.IsLocked;
            }

            // Add drag handlers
            AddDragHandlers(uiElement);

            // Assigning Listeners
            layer.layerUIManager.ItemImageButton.GetComponent<ToggleButton>().onClick.AddListener(() => SetActiveLayer(layer));

            if (layer.gameObject.GetComponent<HidingLayer>() != null)
                layer.layerUIManager.EyeToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => OnEyeToggleValueChange(value, layer));
            else
                layer.layerUIManager.SetEyeToggleInactive();

            if (layer.gameObject.GetComponent<LockingLayer>() != null)
                layer.layerUIManager.LockToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => OnLockToggleValueChange(value, layer));
            else
                layer.layerUIManager.SetLockToggleInactive();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Find the layer being dragged and its index
        for (int i = 0; i < layers.Count; i++)
        {
            if (eventData.pointerDrag == layers[i].uiElement.gameObject)
            {
                startDragIndex = i;
                break;
            }
        }
    }

    /*    public void OnEndDrag(PointerEventData eventData)
        {
            List<LayerData> sortedLayers = layers.OrderBy(l => l.uiElement.anchoredPosition.y).ToList();
            layers = sortedLayers;

            int layerBeingDragged;

            for (int i = 0; i < layers.Count; i++)
            {
                layers[i].uiElement.SetSiblingIndex(i);
            }

            UpdateLayerPositions();
        }*/

    public void OnEndDrag(PointerEventData eventData)
    {
        int endDragIndex = startDragIndex; // Default to original index if no valid drop target is found

        // Find the target index based on the position of the dragged UI element
        for (int i = 0; i < layers.Count; i++)
        {
            if (eventData.pointerDrag.transform.position.y > layers[i].uiElement.transform.position.y)
            {
                endDragIndex = i;
                break;
            }
        }

        // Ensure the end index is valid
        if (endDragIndex != startDragIndex)
        {
            UpdateLayerPositions(startDragIndex, endDragIndex);
        }
    }

    public void SetActiveLayer(LayerData newActiveLayer)
    {
        ToggleButton activeLayerToggleButton = newActiveLayer.layerUIManager.ItemImageButton.GetComponent<ToggleButton>();

        if (activeLayer != null && activeLayer != activeLayerToggleButton)
        {
            activeLayer.Deselect();
            ClearActiveLayer();
        }

        activeLayer = activeLayerToggleButton;
        activeLayer.Highlight(true);

        CurrentlySelectedLayerData = layers.Find(layer => layer.layerUIManager.ItemImageButton.GetComponent<ToggleButton>() == activeLayer); //wtf

        newActiveLayer.layerUIManager.ShowLayerFrame(true);
    }
    public void ClearActiveLayer()
    {
        if (activeLayer != null)
        {
            activeLayer.Highlight(false);
            currentlySelectedLayerData.layerUIManager.ShowLayerFrame(false);

            activeLayer = null;
            CurrentlySelectedLayerData = null;
            //HideTransparencySlider();
        }

    }

    public ToggleButton GetActiveLayer()
    {
        return activeLayer;
    }

    void OnEyeToggleValueChange(bool value, LayerData layerData)
    {
        if (layerData.gameObject.GetComponent<HidingLayer>() != null)
            layerData.gameObject.GetComponent<HidingLayer>().IsHidden = !value; //fix later
    }
    public void OnLockToggleValueChange(bool value, LayerData layerData)
    {
        if (layerData.gameObject.GetComponent<LockingLayer>() != null)
        {
            layerData.gameObject.GetComponent<LockingLayer>().IsLocked = value;
        }
    }

    public void RemoveLayer(ILayerObject obj)
    {
        if (layers.Count == 0)
            return;
        for (int i = layers.Count - 1; i >= 0; i--)
        {
            if (layers[i].id == obj.ID)
            {
                Destroy(layers[i].uiElement.gameObject);
                layers.RemoveAt(i);
            }
        }
    }

    public void ClearLayerList()
    {
        if (layers.Count > 0)
            layers.Clear();

        UpdateUI();
    }

    public void SetAllObjectsAsUsed()
    {
        if (layers.Count > 0)
            foreach (LayerData elem in layers)
            {
                elem.layerObject.IsUsed = true;
            }
        ClearLayerList();
    }

    private void UpdateLayerPositions(int selectedLayerIndex, int swappedLayerIndex)
    {
        // Swap positions in the list
        LayerData temp = layers[selectedLayerIndex];
        layers[selectedLayerIndex] = layers[swappedLayerIndex];
        layers[swappedLayerIndex] = temp;

        // Update the UI elements' sibling index
        for (int i = 0; i < layers.Count; i++)
        {
            LayerData layer = layers[i];
            layer.uiElement.SetSiblingIndex(i);
        }

        // Ensure all layer game objects have a common parent
        Transform parentTransform = layers[0].gameObject.transform.parent;

        // Update the positions of the game objects
        for (int i = 0; i < layers.Count; i++)
        {
            LayerData layer = layers[i];
            layer.gameObject.transform.SetSiblingIndex(i);

            // Optionally, update the positions to reflect changes in UI
            Vector3 currentPos = layer.gameObject.transform.position;
            Vector3 newPosition = new Vector3(currentPos.x, currentPos.y, parentTransform.position.z + i * 1.5f); // Adjust the offset as needed
            layer.gameObject.transform.position = newPosition;
        }
    }


    // Other methods like RemoveLayer, ClearLayerList, SetActiveLayer, etc.

    public void AddDragHandlers(GameObject uiElement)
    {
        EventTrigger trigger = uiElement.GetComponent<EventTrigger>() ?? uiElement.AddComponent<EventTrigger>();

        // Add OnBeginDrag
        EventTrigger.Entry beginDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.BeginDrag };
        beginDragEntry.callback.AddListener((eventData) => OnBeginDrag((PointerEventData)eventData));
        trigger.triggers.Add(beginDragEntry);

        // Add OnEndDrag
        EventTrigger.Entry endDragEntry = new EventTrigger.Entry { eventID = EventTriggerType.EndDrag };
        endDragEntry.callback.AddListener((eventData) => OnEndDrag((PointerEventData)eventData));
        trigger.triggers.Add(endDragEntry);
    }

}
