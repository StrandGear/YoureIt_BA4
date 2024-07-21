using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using FMOD.Studio;

public class LayerManager : MonoBehaviour
{
    //[SerializeField] private LayerObjectsVisibilityRadius radiusOfVisibility;

    [Tooltip("Prefab to show each layer button")]
    [SerializeField]private GameObject layerUIPrefab;

    [SerializeField] private Button layerBtnUP;
    [SerializeField] private Button layerBtnDown;
    [SerializeField] private Transform uiParent;

/*    [Tooltip("Displayed for objects with Transparent Layer")]
    [SerializeField] private Slider transparencySlider;*/
    private ToggleButton activeLayer;
    private bool isActiveLayerHidden = false;

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
        public GameObject gameObject;
        public LayerObject layerObject;
        public RectTransform uiElement;
        public LayerUIManager layerUIManager;
        public string name;
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

    //private EventInstance UI_selectObject_sound;

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

        layerBtnDown.onClick.AddListener(() => MoveLayerDown(CurrentlySelectedLayerData));
        layerBtnUP.onClick.AddListener(() => MoveLayerUp(CurrentlySelectedLayerData));

        //HideTransparencySlider();
    }

    private void Start()
    {
        if (AudioManager.instance == null)
            return;
        // Initialize playerFootsteps
        //UI_selectObject_sound = AudioManager.instance.CreateEventInstance(FMODEvents.instance.UI_selectElement);

        // Set initial 3D attributes (position and velocity)
        //UIsounds.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(transform.position));
        
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
            layer.layerUIManager = uiElement.GetComponent<LayerUIManager>();

            if(layer.layerObject.ObjectSprite != null)
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

            //Assigning Listeners
            layer.layerUIManager.ItemImageButton.GetComponent<ToggleButton>().onClick.AddListener(() => SetActiveLayer(layer));

            if (layer.gameObject.GetComponent<HidingLayer>() == null)
                layer.layerUIManager.SetEyeToggleInactive();
            layer.layerUIManager.EyeToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => OnEyeToggleValueChange(value, layer));

            if (layer.gameObject.GetComponent<LockingLayer>() == null)
                layer.layerUIManager.SetLockToggleInactive();
            layer.layerUIManager.LockToggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => OnLockToggleValueChange(value, layer));                
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

    /*    public void SetActiveLayer(ToggleButton newActiveLayer)
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
        }*/

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
        
        AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_selectElement);
        //newActiveLayer.layerObject.SetShaderActive(true);
       /* if (!isActiveLayerHidden)
            
        else
            newActiveLayer.layerObject.SetShaderActive(false);*/

        newActiveLayer.layerUIManager.ShowLayerFrame(true);
        //UI_selectObject_sound.start();
    }

    public void ClearActiveLayer()
    {
        if (activeLayer != null)
        {
            activeLayer.Highlight(false);
            currentlySelectedLayerData.layerUIManager.ShowLayerFrame(false);
            currentlySelectedLayerData.layerObject.SetShaderActive(false);

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
        {
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_selectElement);
            layerData.gameObject.GetComponent<HidingLayer>().IsHidden = !value; //fix later
        }    
        else
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_cantSelect);

        if (layerData == CurrentlySelectedLayerData)
        {
            layerData.layerObject.SetShaderActive(!value);
        }

    }
    public void OnLockToggleValueChange(bool value, LayerData layerData)
    {  
        if (layerData.gameObject.GetComponent<LockingLayer>() != null)
        {
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_selectElement);
            layerData.gameObject.GetComponent<LockingLayer>().IsLocked = value;
        }
        else
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_cantSelect);
    }
/*    public void OnLinkToggleValueChange(bool value, LayerData layerData)
    {
        if (layerData.gameObject.GetComponent<HidingLayer>() == null)
            return;
    }*/

    public void UpdateLockToggleValue(bool value)
    {
        //layerLocked = value;
        print("layer locked " + value);
/*        currentlySelectedLayerData.layerGameObject.TryGetComponent(out LockingLayer moveObject);
        if (moveObject != null)
            moveObject.ToggleFreeze();*/
    }

/*    private void ShowTransparencySlider()
    {
        CurrentlySelectedLayerData.gameObject.TryGetComponent(out TransparentLayer tempGameObj);

        if (tempGameObj != null)
        {
            transparencySlider.gameObject.SetActive(true);

            transparencySlider.value = tempGameObj.Transparency;

            transparencySlider.onValueChanged.AddListener((value) => tempGameObj.Transparency = value);
        }
        else return;
    }*/

    private void ShowLockToggle()
    {
        CurrentlySelectedLayerData.uiElement.Find("LockToggle");
    }

/*    private void HideTransparencySlider()
    {
        transparencySlider.onValueChanged.RemoveAllListeners();
        transparencySlider.gameObject.SetActive(false);
    }*/

    private void MoveLayerDown(LayerData layerData)
    {
        int index = layers.IndexOf(layerData);
        //print("first " + index);
        if (index < layers.Count - 1)
        {
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_selectElement);
            int swappedLayerIndex = index + 1;
            layers.RemoveAt(index);
            print(index);
            layers.Insert(index + 1, layerData);
            UpdateLayerPositions(index, swappedLayerIndex);
        }
        else
        {
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_cantSelect);
            return;
        }
            
    }

    private void MoveLayerUp(LayerData layerData)
    {
        int index = layers.IndexOf(layerData);
        if (index > 0)
        {
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_selectElement);
            int swappedLayerIndex = index - 1;
            layers.RemoveAt(index);
            layers.Insert(index - 1, layerData);
            UpdateLayerPositions(index, swappedLayerIndex);
        }
        else
        {
            AudioManager.instance.PlayOneShotAtPlayerPosition(FMODEvents.instance.UI_cantSelect);
            return;
        }
    }

    private void UpdateLayerPositions(int selectedLayerIndex, int swappedLayerIndex)
    {
        //assigning new start position  
        //Vector3 StartPosition1 = layers[selectedLayerIndex].layerObject.StartPosition;
        //Vector3 StartPosition2 = layers[swappedLayerIndex].layerObject.StartPosition;

        //Vector3 newPosition1 = layers[selectedLayerIndex].layerObject.CurrentFixedPosition;
        //Vector3 newPosition2 = layers[swappedLayerIndex].layerObject.CurrentFixedPosition;
        Vector3 newPosition1 = layers[selectedLayerIndex].gameObject.transform.position;
        Vector3 newPosition2 = layers[swappedLayerIndex].gameObject.transform.position;

        //assigning new object position
        /*        layers[selectedLayerIndex].gameObject.transform.position = layers[swappedLayerIndex].layerObject.StartPosition;
                layers[swappedLayerIndex].gameObject.transform.position = StartPosition1;*/
        //layers[selectedLayerIndex].layerObject.SetNewPosition(newPosition2);
        //layers[swappedLayerIndex].layerObject.SetNewPosition (newPosition1); 

        layers[selectedLayerIndex].gameObject.transform.position = new Vector3(newPosition2.x, newPosition1.y, newPosition2.z);
        layers[swappedLayerIndex].gameObject.transform.position = new Vector3(newPosition1.x, newPosition2.y, newPosition1.z);

        //layers[selectedLayerIndex].layerObject.StartPosition = StartPosition2;
        //layers[swappedLayerIndex].layerObject.StartPosition = StartPosition1;

        for (int i = 0; i < layers.Count; i++)
        {
            LayerData layer = layers[i];
            layer.uiElement.SetSiblingIndex(i);
        }
    }
}
