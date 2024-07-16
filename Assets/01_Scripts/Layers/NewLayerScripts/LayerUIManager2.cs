using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LayerUIManager2 : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public ToggleButton ItemImageButton;
    public Toggle EyeToggle;
    public Toggle LockToggle;

    [SerializeField] private GameObject inactiveLockToggleObject;
    [SerializeField] private GameObject inactiveEyeToggleObject;
    [SerializeField] private GameObject selectedLayerFrame;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private LayerManager2 layerManager;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        layerManager = LayerManager2.Instance;
    }

    public void SetEyeToggleInactive()
    {
        for (int i = 0; i < EyeToggle.transform.childCount; i++)
        {
            EyeToggle.transform.GetChild(i).gameObject.SetActive(false);
        }
        inactiveEyeToggleObject.SetActive(true);
    }
    public void SetLockToggleInactive()
    {
        for (int i = 0; i < LockToggle.transform.childCount; i++)
        {
            LockToggle.transform.GetChild(i).gameObject.SetActive(false);
        }
        inactiveLockToggleObject.SetActive(true);
    }

    public void ShowLayerFrame(bool show)
    {
        selectedLayerFrame.SetActive(show);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / layerManager.Canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        layerManager.OnEndDrag(eventData);
    }
}
