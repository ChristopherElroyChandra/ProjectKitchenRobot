using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class CookingKitchenStation : InteractibleKitchenStation
{
    [SerializeField] CookingRecipeListSO _cookingRecipeList;
    [SerializeField] CookingStationProgressVisual _cookingStationProgressVisual;

    [SerializeField] AudioSource _fryingSound;

    private bool _isCooking;
    public bool IsCooking { get { return _isCooking; } }

    private CookingRecipeSO _recipe;

    private int _ticksCooked;
    [SerializeField] int _ticksNeeded;
    // Add cooking time, progress, etc variables

    protected override void Awake()
    {
        base.Awake();

        RegisterOnResetAction(ResetCooking);
    }

    private void ResetCooking()
    {
        _ticksCooked = 0;
    }

    protected override void SetDragPointerData()
    {
        _dragPointerData = ScriptableObject.CreateInstance<DragPointerDataSO>();
        _dragPointerData.PointerNameText = "Cooking Station";
        _dragPointerData.PointerIconSprite = _sprite;
        _dragPointerData.PointerDescriptionText = "Place ingredients on the station to start cooking. Takes " + (_ticksNeeded / GameTimeManager.Instance.TicksPerSecond) + " seconds to complete.";
    }

    public override void PlaceOnStation(PlayerInventorySlot playerInventorySlot, VoidEventChannelSO actionCompleteEventChannel)
    {
        if (playerInventorySlot.Ingredient == null)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.NoIngredientInInventorySlot);

            Debug.Log("Player inventory slot is empty");
            return;
        }

        if (_ingredient != null)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.StationAlreadyHasAnIngredient);

            Debug.Log("Already has an ingredient");
            return;
        }

        _recipe = GetRecipeFromIngredients(playerInventorySlot.Ingredient);

        if (_recipe == null)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.PlacedIngredientCannotBeCooked);

            Debug.Log("Recipe not found");
            return;
        }

        _ingredient = playerInventorySlot.Ingredient;
        playerInventorySlot.Ingredient = null;
        _ticksCooked = 0;

        AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.PlaceCookingStation);

        _isCooking = true;

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            _fryingSound.Play();
            actionCompleteEventChannel.RaiseEvent();
            StartCooking();
        });
    }

    private void StartCooking()
    {
        _cookingStationProgressVisual.SetProgress((float)_ticksCooked / (float)_ticksNeeded);

        if (_ticksCooked >= _ticksNeeded)
        {
            _isCooking = false;
            _ingredient = _recipe.OutputIngredient;

            _fryingSound.Stop();

            return;
        }

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            _ticksCooked++;
            StartCooking();
        });
    }

    private CookingRecipeSO GetRecipeFromIngredients(KitchenIngredientSO ingredient)
    {
        foreach (CookingRecipeSO recipe in _cookingRecipeList.CookingRecipes)
        {
            if (recipe.InputIngredient == ingredient)
            {
                return recipe;
            }
        }
        return null;
    }

    public override void TakeFromStation(PlayerInventorySlot playerInventorySlot, VoidEventChannelSO actionCompleteEventChannel)
    {
        if (_isCooking)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.CannotTakeWhileStationIsCooking);

            Debug.Log("Cannot take ingredient while cooking");
            return;
        }
        
        if (_ingredient == null)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.NoIngredientToTakeFromCounter);

            Debug.Log("No ingredient to take from counter");
            return;
        }

        if (playerInventorySlot.Ingredient != null)
        {
            // Show error message

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


}
