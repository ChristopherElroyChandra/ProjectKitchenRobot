using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DragPointerVisualUI : MonoBehaviour
{
    public static DragPointerVisualUI Instance { get; private set; }

    [SerializeField] Image _dragPointerIcon;
    [SerializeField] TextMeshProUGUI _dragPointerText;

    [SerializeField] TextMeshProUGUI _dragPointerDescriptionObject;
    [SerializeField] TextMeshProUGUI _dragPointerDescription;

    private Vector2 _initialPosition;
    private Vector2 _offset;

    private RectTransform _rectTransform;
    private Canvas _parentCanvas;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
            return;
        }

        _rectTransform = GetComponent<RectTransform>();
        _parentCanvas = _rectTransform.GetComponentInParent<Canvas>();

        gameObject.SetActive(false);
    }
    
    private void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }

    public void SetDragPointerData(DragPointerDataSO dragPointerData, DragPointerVisualType visualType)
    {
        if (dragPointerData != null)
        {
            SetDragPointerData(dragPointerData);
        }
        else
        {
            Debug.LogError("DragPointerDataSO is null");
        }
        SetActiveElements(visualType);
    }

    public void SetActiveElements(DragPointerVisualType visualType)
    {
        switch (visualType)
        {
            default:
            case DragPointerVisualType.IconOnly:
                _dragPointerIcon.gameObject.SetActive(true);
                _dragPointerText.gameObject.SetActive(false);
                _dragPointerDescription.gameObject.SetActive(false);
                break;
            case DragPointerVisualType.TextOnly:
                _dragPointerIcon.gameObject.SetActive(false);
                _dragPointerText.gameObject.SetActive(true);
                _dragPointerDescription.gameObject.SetActive(false);
                break;
            case DragPointerVisualType.IconTextAndDescription:
                _dragPointerIcon.gameObject.SetActive(true);
                _dragPointerText.gameObject.SetActive(true);
                _dragPointerDescription.gameObject.SetActive(true);
                break;
        }
    }

    public void SetDragPointerData(DragPointerDataSO dragPointerData)
    {
        _dragPointerIcon.sprite = dragPointerData.PointerIconSprite;
        _dragPointerText.text = dragPointerData.PointerNameText;
        _dragPointerDescription.text = dragPointerData.PointerDescriptionText;
    }

    public void EnablePointerVisual(bool enabled)
    {
        gameObject.SetActive(enabled);
    }

    public void StartDragging()
    {
        DragPointer.Instance.SetIsDragging(true);
        SetInitial();
        gameObject.SetActive(true);
    }

    public void SetInitial()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas.transform as RectTransform, Input.mousePosition, null, out Vector2 localPos);
        _rectTransform.anchoredPosition = localPos;
        _offset = _rectTransform.anchoredPosition - localPos;
    }

    public void OnDragging()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas.transform as RectTransform, Input.mousePosition, null, out Vector2 localPosition);
        _rectTransform.anchoredPosition = localPosition + _offset;
    }

    public void StopDragging()
    {
        DragPointer.Instance.SetIsDragging(false);
        gameObject.SetActive(false);
    }

    public enum DragPointerVisualType
    {
        IconOnly,
        TextOnly,
        IconAndText,
        IconTextAndDescription
    }
}
