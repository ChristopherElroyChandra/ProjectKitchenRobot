using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    private const int MAX_INVENTORY_SIZE = 3;
    [SerializeField] PlayerInventorySlot[] _playerInventorySlots = new PlayerInventorySlot[MAX_INVENTORY_SIZE];
    public PlayerInventorySlot[] PlayerInventorySlots { get { return _playerInventorySlots; } }

    private void OnValidate()
    {
        if (_playerInventorySlots.Length != MAX_INVENTORY_SIZE)
        {
            Debug.LogWarning("PlayerInventorySlots array length is not " + MAX_INVENTORY_SIZE + ", resizing...");
            Array.Resize(ref _playerInventorySlots, MAX_INVENTORY_SIZE);
        }
    }

    public PlayerInventorySlot GetFirstInventorySlot()
    {
        return _playerInventorySlots[0];
    }

    public int IndexOfInventorySlot(PlayerInventorySlot inventorySlot)
    {
        for (int i = 0; i < _playerInventorySlots.Length; i++)
        {
            if (_playerInventorySlots[i] == inventorySlot)
            {
                return i;
            }
        }
 
       return -1;
    }

    public void ResetInventorySlots()
    {
        for (int i = 0; i < _playerInventorySlots.Length; i++)
        {
            _playerInventorySlots[i].Ingredient = null;
        }
    }
}

[System.Serializable]
public class PlayerInventorySlot
{
    public KitchenIngredientSO Ingredient;
}