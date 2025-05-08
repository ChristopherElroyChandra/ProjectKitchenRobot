using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlotVisualManager : MonoBehaviour
{
    [SerializeField] InventorySlotVisualUI _inventorySlotVisualUITemplate;
    private void Start()
    {
        Invoke("InitializeInventorySlotVisuals", 0.1f);
    }

    private void InitializeInventorySlotVisuals()
    {
        foreach (PlayerInventorySlot inventorySlot in PlayerActionReceiver.Instance.PlayerInteract.PlayerInventorySlots)
        {
            InventorySlotVisualUI ui = Instantiate(_inventorySlotVisualUITemplate, this.transform);
            ui.gameObject.SetActive(true);
            ui.SetPlayerInventorySlot(inventorySlot);
        }
    }
}
