using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class InteractibleKitchenStation : BaseKitchenStation, IDragable, IDragPointerData
{
    [SerializeField] protected Sprite _sprite;
    [SerializeField] protected DragPointerDataSO _dragPointerData;

    protected override void Awake()
    {
        base.Awake();

        SetDragPointerData();
    }

    protected abstract void SetDragPointerData();

    public DragPointerDataSO GetDragPointerData()
    {
        return _dragPointerData;
    }


    public virtual object GetDraggedObjectData()
    {
        return (object)this;
    }

    public virtual IDragable GetDraggedType()
    {
        return this;
    }

    public void OnDragging()
    {
        DragPointerVisualUI.Instance.OnDragging();
    }

    public void StartDragging()
    {
        DragPointerVisualUI.Instance.SetActiveElements(DragPointerVisualUI.DragPointerVisualType.IconOnly);
        DragPointerVisualUI.Instance.StartDragging();
    }

    public void StopDragging()
    {
        DragPointerVisualUI.Instance.StopDragging();
    }

    void OnMouseEnter()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            return;
        }

        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointer.Instance.SetDragTarget(this);
        DragPointerVisualUI.Instance.SetDragPointerData(_dragPointerData, DragPointerVisualUI.DragPointerVisualType.IconTextAndDescription);
        
        DragPointerVisualUI.Instance.SetInitial();

        DragPointerVisualUI.Instance.EnablePointerVisual(true);
    }

    void OnMouseOver()
    {
        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointerVisualUI.Instance.OnDragging();
    }

    void OnMouseExit()
    {
        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointer.Instance.ClearDragTarget();
        DragPointerVisualUI.Instance.EnablePointerVisual(false);
    }

    void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject()) return;
        StartDragging();
    }

    void OnMouseDrag()
    {
        OnDragging();
    }

    void OnMouseUp()
    {
        StopDragging();
    }
}