using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseKitchenStation : GridObject
{
    [SerializeField] protected KitchenIngredientSO _ingredient;
    public KitchenIngredientSO Ingredient { get { return _ingredient; } }

    [SerializeField] KitchenStationVisual _visual;

    private KitchenIngredientSO _initialIngredient;

    protected override void Awake()
    {
        base.Awake();

        RegisterOnSetInitialAction(SetInitialIngredient);
        RegisterOnResetAction(ResetIngredient);
    }

    private void SetInitialIngredient()
    {
        _initialIngredient = _ingredient;
    }

    private void ResetIngredient()
    {
        _ingredient = _initialIngredient;
    }

    protected override void SetSpriteLayerOrder()
    {
        _visual.SetSpriteOrder((KitchenManager.Instance.KitchenDesign.KitchenHeight - GridPosition.y) * KitchenManager.Instance.KitchenDesign.KitchenWidth + GridPosition.x);
    }

    public virtual void TakeFromStation(PlayerInventorySlot playerInventorySlot, VoidEventChannelSO actionCompleteEventChannel)
    {
        if (_ingredient == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.NoIngredientToTakeFromCounter);

            Debug.Log("No ingredient to take from counter");
            return;
        }

        if (playerInventorySlot.Ingredient != null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.PlayerAlreadyHasAnIngredient);

            Debug.Log("Player already has an ingredient");
            return;
        }

        playerInventorySlot.Ingredient = _ingredient;
        _ingredient = null;

        AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.Take);

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            actionCompleteEventChannel.RaiseEvent();
        });
    }

    public virtual void PlaceOnStation(PlayerInventorySlot playerInventorySlot, VoidEventChannelSO actionCompleteEventChannel)
    {
        if (playerInventorySlot.Ingredient == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.NoIngredientInInventorySlot);

            Debug.Log("Player does not have an ingredient");
            return;
        }

        if (_ingredient != null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.StationAlreadyHasAnIngredient);

            Debug.Log("Already has an ingredient");
            return;
        }

        _ingredient = playerInventorySlot.Ingredient;
        playerInventorySlot.Ingredient = null;

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            actionCompleteEventChannel.RaiseEvent();
        });
    }
}