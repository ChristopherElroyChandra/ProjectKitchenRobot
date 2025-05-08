using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class IngredientVariableUI : MonoBehaviour, IDragPointerData, IDragable, IPointerEnterHandler, IPointerMoveHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{
    private KitchenIngredientSO _ingredient;
    private Image _ingredientImage;
    public object GetDraggedObjectData()
    {
        return (object)_ingredient;
    }

    public IDragable GetDraggedType()
    {
        return this;
    }

    public DragPointerDataSO GetDragPointerData()
    {
        DragPointerDataSO data = ScriptableObject.CreateInstance<DragPointerDataSO>();
        data.PointerNameText = _ingredient.IngredientName;
        data.PointerIconSprite = _ingredient.IngredientIcon;
        data.PointerDescriptionText = _ingredient.IngredientDescription;
        return data;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (DragPointer.Instance.IsDragging)
        {
            return;
        }
        DragPointer.Instance.SetDragTarget(this);

        DragPointerVisualUI.Instance.SetDragPointerData(GetDragPointerData(), DragPointerVisualUI.DragPointerVisualType.IconTextAndDescription);

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

    public void StartDragging()
    {
        DragPointerVisualUI.Instance.SetActiveElements(DragPointerVisualUI.DragPointerVisualType.IconOnly);
        DragPointerVisualUI.Instance.StartDragging();
    }

    public void OnDragging()
    {
        DragPointerVisualUI.Instance.OnDragging();
    }

    public void StopDragging()
    {
        DragPointerVisualUI.Instance.StopDragging();
    }

    internal void SetIngredient(KitchenIngredientSO ingredient)
    {
        _ingredient = ingredient;
        _ingredientImage = GetComponent<Image>();
        _ingredientImage.sprite = ingredient.IngredientIcon;
    }
}
