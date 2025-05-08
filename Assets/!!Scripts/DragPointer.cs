using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class DragPointer : MonoBehaviour
{
    public static DragPointer Instance { get; private set; }

    [SerializeField] Canvas _canvas;
    [SerializeField] bool _isDragging;
    public bool IsDragging { get { return _isDragging; } }

    private IDragable _dragTarget;
    public IDragable DragTargetType { get { return _dragTarget; } }

    [SerializeField] GameObject _currentDragTarget;
    [SerializeField] DropZone _currentDropZone;
    public DropZone CurrentDropZone { get { return _currentDropZone; } }

    protected GraphicRaycaster _raycaster;
    protected EventSystem _eventSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
        
        _raycaster = _canvas.GetComponent<GraphicRaycaster>();
        _eventSystem = EventSystem.current;
    }

    private void Update()
    {
        PointerEventData pointerEventData = new PointerEventData(_eventSystem);
        pointerEventData.position = Input.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        _raycaster.Raycast(pointerEventData, results);

        List<DropZone> dropZones = new List<DropZone>();

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.TryGetComponent<DropZone>(out DropZone dropZone))
            {
                if (dropZone.CanAccept(DragTargetType))
                {
                    // Debug.Log("Adding drop zone " + dropZone.gameObject.name);
                    dropZones.Add(dropZone);
                }
            }
        }

        dropZones.Sort((DropZone a, DropZone b) => b.Priority.CompareTo(a.Priority));

        if (dropZones.Count == 0)
        {
            // Debug.LogWarning("No drop zones found for " + DragPointer.Instance.DragTargetType);
            _currentDropZone = null;
            return;
        }

        _currentDropZone = dropZones[0];

        if (_dragTarget != null)
        {
            MonoBehaviour monoBehaviour = _dragTarget as MonoBehaviour;
            if (monoBehaviour != null)
            {
                _currentDragTarget = monoBehaviour.gameObject;
            }
        }
    }

    public void SetIsDragging(bool isDragging)
    {
        _isDragging = isDragging;
    }

    public void SetDragTarget(IDragable dragTarget)
    {
        _dragTarget = dragTarget;
    }

    public void ClearDragTarget()
    {
        _dragTarget = null;
    }
}
