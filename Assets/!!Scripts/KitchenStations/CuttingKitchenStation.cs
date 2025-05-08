using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;

public class CuttingKitchenStation : UsableKitchenStation
{
    [SerializeField] CuttingRecipeListSO _cuttingRecipeList;
    [SerializeField] CuttingStationProgressVisual _progressVisual;
    [SerializeField] Sprite _cutIcon;
    [SerializeField] CuttingRecipeSO _recipe;
    [SerializeField] int _ticksPerCut;

    private int _cutsLeft;

    protected override void Awake()
    {
        base.Awake();

        // Reset cut time
        // RegisterOnResetAction(ResetCutTime);
    }

    public override void Use(VoidEventChannelSO actionCompleteEventChannel)
    {
        _recipe = GetRecipeFromIngredients(_ingredient);

        if (_recipe == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.PlacedIngredientCannotBeCut);

            Debug.Log("Recipe not found");
            return;
        }

        _cutsLeft = _recipe.CutsNeeded;        
        StartCutting(actionCompleteEventChannel);
    }

    private void StartCutting(VoidEventChannelSO actionCompleteEventChannel)
    {
        _progressVisual.SetProgress((_recipe.CutsNeeded - _cutsLeft) / (float)_recipe.CutsNeeded);
        if (_cutsLeft <= 0)
        {
            CutsComplete(actionCompleteEventChannel);
            return;
        }

        LeanTween.value(0, 0, _ticksPerCut * GameTimeManager.Instance.TickInterval)
        .setOnComplete(() => 
        {
            AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.UseCuttingStation);
            _cutsLeft--;
            StartCutting(actionCompleteEventChannel);
        });
    }

    private void CutsComplete(VoidEventChannelSO actionCompleteEventChannel)
    {
        _ingredient = _recipe.OutputIngredient;

        actionCompleteEventChannel.RaiseEvent();
    }

    private CuttingRecipeSO GetRecipeFromIngredients(KitchenIngredientSO ingredient)
    {
        foreach (CuttingRecipeSO recipe in _cuttingRecipeList.CuttingRecipes)
        {
            if (recipe.InputIngredient == ingredient)
            {
                return recipe;
            }
        }
        return null;
    }

    protected override void SetDragPointerData()
    {
        _dragPointerData = ScriptableObject.CreateInstance<DragPointerDataSO>();
        _dragPointerData.PointerIconSprite = _cutIcon;
        _dragPointerData.PointerNameText = "Cutting Station";
        _dragPointerData.PointerDescriptionText = "Use this station to cut ingredients.";
    }
}
