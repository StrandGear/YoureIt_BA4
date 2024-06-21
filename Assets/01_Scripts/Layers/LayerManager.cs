using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class LayerManager : MonoBehaviour
{
    [SerializeField]private GameObject layerUIPrefab;
    [SerializeField] private Button layerBtnUP;
    [SerializeField] private Button layerBtnDown;
    [SerializeField] private Transform uiParent;

    [SerializeField] private float zOffset = 0.1f; // Distance between layers

    [SerializeField] private List<LayerData> layers = new List<LayerData>();

    private LayerData currentlySelectedLayer;

    public LayerData CurrentlySelectedLayer
    {
        get { print(currentlySelectedLayer); return currentlySelectedLayer; }
        set { currentlySelectedLayer = value; }
    }

    [System.Serializable]
    public class LayerData
    {
        public GameObject layerGameObject;
        public RectTransform uiElement;
        //public int depth;
    }

    private void Awake()
    {
        foreach (ILayerObject obj in FindObjectsOfType<MonoBehaviour>().OfType<ILayerObject>())
        {
            AddLayer(obj);
        }

        layerBtnDown.onClick.AddListener(() => MoveLayerDown(CurrentlySelectedLayer));
        layerBtnUP.onClick.AddListener(() => MoveLayerUp(CurrentlySelectedLayer));
    }

    public void AddLayer(ILayerObject obj)
    {
        GameObject uiElement = Instantiate(layerUIPrefab, uiParent);
        LayerData layerData = new LayerData
        {
            layerGameObject = obj.LayerGameObject,
            uiElement = uiElement.GetComponent<RectTransform>(),
            //depth = 
        };
        layers.Add(layerData);

        string name = obj.Name;

        print("Added layer obj " + " " + layerData.layerGameObject.name);

        uiElement.GetComponentInChildren<TMP_Text>().text = name;

        uiElement.GetComponentInChildren<Button>().onClick.AddListener(() => LayerSelect(layerData));

/*        EventTrigger trigger = uiElement.GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.Deselect;
        entry.callback.AddListener((data) => { OnDeselectDelegate((PointerEventData)data); });
        trigger.triggers.Add(entry);*/
    }

    private void LayerSelect(LayerData layerData)
    {
        if (CurrentlySelectedLayer != layerData)
            CurrentlySelectedLayer = layerData;
        print(layers.IndexOf(layerData));
        //currentlySelectedLayer = layerData;

        //print("Layer select " + CurrentlySelectedLayer.layerGameObject.name);
    }

    public void OnDeselectDelegate(PointerEventData data)
    {
        CurrentlySelectedLayer = null;
        print("Layer deselect");
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
        /*        Vector3 newPosition = selectedLayer.gameObject.transform.position;
                selectedLayer.gameObject.transform.position = swappedLayer.gameObject.transform.position;
                swappedLayer.gameObject.transform.position = newPosition;*/
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
