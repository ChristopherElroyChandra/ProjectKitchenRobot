using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class LineNumberUI : MonoBehaviour, IDragable, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler, IDragPointerData
{
    [SerializeField] TextMeshProUGUI _lineNumberText;

    private int _lineNumber = 0;

    public void SetLineNumber(int lineNumber)
    {
        _lineNumber = lineNumber;
        SetUI();
    }

    private void Update()
    {
        SetUI();
    }

    private void SetUI()
    {
        _lineNumberText.text = _lineNumber.ToString();
    }

    public object GetDraggedObjectData()
    {
        CommandLineVariable commandLineVariable = new CommandLineVariable();
        commandLineVariable.CommandLine = _lineNumber - 1;
        return (object)commandLineVariable;
    }

    public IDragable GetDraggedType()
    {
        return this;
    }

    public void OnDragging()
    {
        DragPointerVisualUI.Instance.OnDragging();
    }

    public void StartDragging()
    {
        DragPointerVisualUI.Instance.SetActiveElements(DragPointerVisualUI.DragPointerVisualType.TextOnly);
        DragPointerVisualUI.Instance.StartDragging();
    }

    public void StopDragging()
    {
        DragPointerVisualUI.Instance.StopDragging();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        StopDragging();
    }

    public void OnDrag(PointerEventData eventData)
    {
        OnDragging();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        StartDragging();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointer.Instance.SetDragTarget(this);
        DragPointerVisualUI.Instance.SetDragPointerData(GetDragPointerData() , DragPointerVisualUI.DragPointerVisualType.TextOnly);
        
        DragPointerVisualUI.Instance.SetInitial();

        DragPointerVisualUI.Instance.EnablePointerVisual(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointer.Instance.ClearDragTarget();
        DragPointerVisualUI.Instance.EnablePointerVisual(false);
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointerVisualUI.Instance.OnDragging();
    }

    public DragPointerDataSO GetDragPointerData()
    {
        DragPointerDataSO dragPointerData = ScriptableObject.CreateInstance<DragPointerDataSO>();
        dragPointerData.PointerIconSprite = null;
        dragPointerData.PointerNameText = "Line " + _lineNumber;
        return dragPointerData;
    }
}
