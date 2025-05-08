using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerActionReceiver : MonoBehaviour
{
    public static PlayerActionReceiver Instance { get; private set; }

    [SerializeField] PlayerInteract _playerInteract;
    public PlayerInteract PlayerInteract { get { return _playerInteract; } }
    [SerializeField] PlayerMovement _playerMovement;

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
    }

    public void MoveTowards(BaseKitchenStation targetStation, VoidEventChannelSO actionCompleteEventChannel)
    {
        _playerMovement.MoveTowards(targetStation.Tile, actionCompleteEventChannel);
    }

    public void TakeFromStation(BaseKitchenStation station, VoidEventChannelSO actionCompleteEventChannel)
    {
        TakeFromStation(station, actionCompleteEventChannel, _playerInteract.GetFirstInventorySlot());
    }

    public void TakeFromStation(BaseKitchenStation station, VoidEventChannelSO actionCompleteEventChannel, PlayerInventorySlot playerInventorySlot)
    {
        station.TakeFromStation(playerInventorySlot, actionCompleteEventChannel);
    }

    public void PlaceOnStation(BaseKitchenStation station, VoidEventChannelSO actionCompleteEventChannel)
    {
        PlaceOnStation(station, actionCompleteEventChannel, _playerInteract.GetFirstInventorySlot());
    }

    public void PlaceOnStation(BaseKitchenStation station, VoidEventChannelSO actionCompleteEventChannel, PlayerInventorySlot playerInventorySlot)
    {
        station.PlaceOnStation(playerInventorySlot, actionCompleteEventChannel);
    }

    public void UseStation(UsableKitchenStation station, VoidEventChannelSO actionCompleteEventChannel)
    {
        station.Use(actionCompleteEventChannel);
    }

    public int GetIndexOfInventorySlot(PlayerInventorySlot inventorySlot)
    {
        return _playerInteract.IndexOfInventorySlot(inventorySlot);
    }


}
