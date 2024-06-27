using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LayerManager : MonoBehaviour
{
    //[SerializeField] private LayerObjectsVisibilityRadius radiusOfVisibility;

    [Tooltip("Prefab to show each layer button")]
    [SerializeField]private GameObject layerUIPrefab;
    [SerializeField] private Button layerBtnUP;
    [SerializeField] private Button layerBtnDown;
    [SerializeField] private Transform uiParent;

    [Tooltip("Displayed for objects with Transparent Layer")]
    [SerializeField] private Slider transparencySlider;
    private ToggleButton activeLayer;
    private bool layerLocked = false;

    [SerializeField] private List<LayerData> layers = new List<LayerData>();
    public List<LayerData> Layers { get => layers; }

    private LayerData currentlySelectedLayerData;

    public LayerData CurrentlySelectedLayerData
    {
        get { print(currentlySelectedLayerData); return currentlySelectedLayerData; }
        set { currentlySelectedLayerData = value; }
    }

    [System.Serializable]
    public class LayerData
    {
        public int id;
        public GameObject layerGameObject;
        public RectTransform uiElement;
        public ToggleButton toggleButton;
        public string name;
        //public int depth;
    }

    private static LayerManager instance = null;
    public static LayerManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LayerManager>();
                if (instance == null)
                {
                    GameObject go = new GameObject("LayerManager");
                    instance = go.AddComponent<LayerManager>();
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

        /*        foreach (ILayerObject obj in FindObjectsOfType<MonoBehaviour>().OfType<ILayerObject>())
                {
                    AddLayer(obj);
                }*/

        layerBtnDown.onClick.AddListener(() => MoveLayerDown(CurrentlySelectedLayerData));
        layerBtnUP.onClick.AddListener(() => MoveLayerUp(CurrentlySelectedLayerData));

        HideTransparencySlider();
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
        //GameObject uiElement = Instantiate(layerUIPrefab, uiParent);

        LayerData layerData = new LayerData
        {
            id = obj.ID,
            layerGameObject = obj.LayerGameObject,
            //uiElement = uiElement.GetComponent<RectTransform>(),
            //toggleButton = uiElement.GetComponentInChildren<ToggleButton>(),
            name = obj.Name
        };
        layers.Add(layerData);

        //uiElement.GetComponentInChildren<TMP_Text>().text = layerData.name;

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
            layer.toggleButton = uiElement.GetComponentInChildren<ToggleButton>();
            uiElement.GetComponentInChildren<TMP_Text>().text = layer.name;

            // Set up the toggle button's onClick listener to handle layer selection
            layer.toggleButton.onClick.AddListener(() => SetActiveLayer(layer.toggleButton));
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

/*    public void ClearAndUpdateUI()
    {
        if (layers.Count == 0)
            return;

        for (int i = layers.Count - 1; i > -1; i--)
        {
            Destroy(layers[i].uiElement.gameObject);
            

        }
    }*/

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
                elem.layerGameObject.GetComponent<LayerObject>().IsUsed = true;
            }
        //UpdateUI();
    }

    public void SetActiveLayer(ToggleButton newActiveLayer)
    {
        if (activeLayer != null && activeLayer != newActiveLayer)
        {
            activeLayer.Deselect();
            ClearActiveLayer();
        }

        activeLayer = newActiveLayer;
        activeLayer.Highlight(true);

        CurrentlySelectedLayerData = layers.Find(layer => layer.toggleButton == activeLayer);

        ShowTransparencySlider();

        ShowLockToggle();
    }

    public void ClearActiveLayer()
    {
        if (activeLayer != null)
        {
            activeLayer.Highlight(false);
            activeLayer = null;
            CurrentlySelectedLayerData = null;
            HideTransparencySlider();
        }
        
    }

    public ToggleButton GetActiveLayer()
    {
        return activeLayer;
    }

    public void UpdateLockToggleValue(bool value)
    {
        layerLocked = value;
        print("layer locked " + value);
        currentlySelectedLayerData.layerGameObject.TryGetComponent(out MoveObject moveObject);
        if (moveObject != null)
            moveObject.ToggleFreeze();
    }

    private void ShowTransparencySlider()
    {
        CurrentlySelectedLayerData.layerGameObject.TryGetComponent(out TransparentLayer tempGameObj);

        if (tempGameObj != null)
        {
            transparencySlider.gameObject.SetActive(true);

            transparencySlider.value = tempGameObj.Transparency;

            transparencySlider.onValueChanged.AddListener((value) => tempGameObj.Transparency = value);
        }
        else return;
    }

    private void ShowLockToggle()
    {
        CurrentlySelectedLayerData.uiElement.Find("LockToggle");
    }

    private void HideTransparencySlider()
    {
        transparencySlider.onValueChanged.RemoveAllListeners();
        transparencySlider.gameObject.SetActive(false);
    }

    private void MoveLayerDown(LayerData layerData)
    {
        int index = layers.IndexOf(layerData);
        print("first " + index);
        if (index < layers.Count - 1)
        {
            int swappedLayerIndex = index + 1;
            layers.RemoveAt(index);
            print(index);
            layers.Insert(index + 1, layerData);
            UpdateLayerPositions(index, swappedLayerIndex);
        }
        else 
            return;
    }

    private void MoveLayerUp(LayerData layerData)
    {
        int index = layers.IndexOf(layerData);
        if (index > 0)
        {
            int swappedLayerIndex = index - 1;
            layers.RemoveAt(index);
            layers.Insert(index - 1, layerData);
            UpdateLayerPositions(index, swappedLayerIndex);
        }
        else
            return;
    }

    private void UpdateLayerPositions(int selectedLayerIndex, int swappedLayerIndex)
    {
        Vector3 newPosition = layers[selectedLayerIndex].layerGameObject.transform.position;
        layers[selectedLayerIndex].layerGameObject.transform.position = layers[swappedLayerIndex].layerGameObject.transform.position;
        layers[swappedLayerIndex].layerGameObject.transform.position = newPosition;

        for (int i = 0; i < layers.Count; i++)
        {
            LayerData layer = layers[i];
            layer.uiElement.SetSiblingIndex(i);
        }
    }
}
