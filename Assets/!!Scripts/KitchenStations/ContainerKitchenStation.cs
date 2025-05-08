using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ContainerKitchenStation : InteractibleKitchenStation
{
    [SerializeField] SpriteRenderer _ingredientIcon;
    private void Start()
    {
        _ingredientIcon.sprite = _ingredient.IngredientIcon;
    }

    public override void TakeFromStation(PlayerInventorySlot playerInventorySlot, VoidEventChannelSO actionCompleteEventChannel)
    {
        if (playerInventorySlot.Ingredient != null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.PlayerAlreadyHasAnIngredient);

            Debug.Log("Player already has an ingredient");
            return;
        }

        playerInventorySlot.Ingredient = Ingredient;

        AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.Take);

        actionCompleteEventChannel.RaiseEvent();
    }

    public override void PlaceOnStation(PlayerInventorySlot playerInventorySlot, VoidEventChannelSO actionCompleteEventChannel)
    {
        // Show error message

        CommandManager.Instance.CommandErrorOccured(ProgramErrorType.CanotPlaceOnContainer);

        Debug.Log("Cannot place ingredient in container");

        return;
    }

    protected override void SetDragPointerData()
    {
        _dragPointerData = ScriptableObject.CreateInstance<DragPointerDataSO>();
        _dragPointerData.PointerNameText = _ingredient.IngredientName + " container";
        _dragPointerData.PointerIconSprite = _ingredient.IngredientIcon;
        _dragPointerData.PointerDescriptionText = "Take from this container to get " + _ingredient.IngredientName + ".";
    }
}
