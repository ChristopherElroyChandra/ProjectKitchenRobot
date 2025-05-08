using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlotVisualUI : MonoBehaviour, IDragable, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IDragPointerData
{
    private PlayerInventorySlot _playerInventorySlot;
    [SerializeField] Image _inventorySlotImage;

    private void Update()
    {
        if (_playerInventorySlot == null)
        {
            return;
        }

        if (_playerInventorySlot.Ingredient == null)
        {
            _inventorySlotImage.gameObject.SetActive(false);
            return;
        }

        _inventorySlotImage.sprite = _playerInventorySlot.Ingredient.IngredientIcon;
        _inventorySlotImage.gameObject.SetActive(true);

    }
    
    public void SetPlayerInventorySlot(PlayerInventorySlot playerInventorySlot)
    {
        _playerInventorySlot = playerInventorySlot;

        if (PlayerActionReceiver.Instance.GetIndexOfInventorySlot(_playerInventorySlot) >= LevelManager.Instance.GameLevel.InventorySlots)
        {
            gameObject.SetActive(false);
            return;
        }
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

    public object GetDraggedObjectData()
    {
        return (object)_playerInventorySlot;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (LevelManager.Instance.GameLevel.InventorySlots < 3)
        {
            return;
        }
        OnDragging();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (LevelManager.Instance.GameLevel.InventorySlots < 3)
        {
            return;
        }
        StopDragging();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (LevelManager.Instance.GameLevel.InventorySlots < 3)
        {
            return;
        }
        StartDragging();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointerVisualUI.Instance.OnDragging();
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

    public DragPointerDataSO GetDragPointerData()
    {
        DragPointerDataSO dragPointerData = ScriptableObject.CreateInstance<DragPointerDataSO>();
        dragPointerData.PointerIconSprite = null;
        dragPointerData.PointerNameText = "Inventory Slot " + (PlayerActionReceiver.Instance.GetIndexOfInventorySlot(_playerInventorySlot) + 1);
        return dragPointerData;
    }
}
