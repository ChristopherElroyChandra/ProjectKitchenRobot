using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class StewingKitchenStation : InteractibleKitchenStation
{
    [SerializeField] int _maxIngredients;
    [SerializeField] List<KitchenIngredientSO> _ingredients;
    public List<KitchenIngredientSO> Ingredients { get { return _ingredients; } }
    [SerializeField] KitchenIngredientSO _susStewIngredient;
    [SerializeField] StewRecipeListSO _stewRecipeList;
    [SerializeField] StewingStationProgressVisual _stewingStationProgressVisual;

    [SerializeField] AudioSource _boilingSound;

    private bool _isStewing;
    public bool IsStewing { get { return _isStewing; } }

    private StewRecipeSO _recipe;

    private int _ticksStewed;
    [SerializeField] int _ticksNeeded;

    protected override void Awake()
    {
        base.Awake();

        RegisterOnResetAction(ResetStewing);
    }

    private void ResetStewing()
    {
        _ticksStewed = 0;
        Ingredients.Clear();
    }

    protected override void SetDragPointerData()
    {
        _dragPointerData = ScriptableObject.CreateInstance<DragPointerDataSO>();
        _dragPointerData.PointerNameText = "Stewing Station";
        _dragPointerData.PointerIconSprite = _sprite;
        _dragPointerData.PointerDescriptionText = "Place ingredients on the station to start stewing. Takes " + (_ticksNeeded / GameTimeManager.Instance.TicksPerSecond) + " seconds to complete.";
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

        if (_ingredients.Count >= _maxIngredients)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.StationAlreadyHasAnIngredient);

            Debug.Log("Already has an ingredient");
            return;
        }

        _ingredients.Add(playerInventorySlot.Ingredient);
        playerInventorySlot.Ingredient = null;

        AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.PlaceCookingStation);

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            _boilingSound.Play();
            _ticksStewed = 0;
            StartStewing();
            actionCompleteEventChannel.RaiseEvent();
        });
    }

    private void StartStewing()
    {
        _stewingStationProgressVisual.SetProgress((float)_ticksStewed / (float)_ticksNeeded);

        if (_ticksStewed >= _ticksNeeded)
        {
            _isStewing = false;

            _recipe = GetRecipeFromIngredients();

            if (_recipe == null)
            {
                _ingredient = _susStewIngredient;
            }
            else
            {
                _ingredient = _recipe.OutputIngredient;
            }

            _ingredients.Clear();

            _boilingSound.Stop();

            return;
        }

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            _ticksStewed++;
            StartStewing();
        });
    }

    private StewRecipeSO GetRecipeFromIngredients()
    {
        foreach (StewRecipeSO recipe in _stewRecipeList.StewRecipes)
        {
            if (IngredientsEqualsRecipe(recipe))
            {
                return recipe;
            }
        }
        return null;
    }

    private bool IngredientsEqualsRecipe(StewRecipeSO recipe)
    {
        if (_ingredients == null || recipe.InputIngredients == null)
        {
            return false;
        }

        if (_ingredients.Count != recipe.InputIngredients.Count)
        {
            return false;
        }

        var ingredientCounts = _ingredients.GroupBy(x => x)
        .ToDictionary(g => g.Key, g => g.Count());

        foreach (var ingredient in recipe.InputIngredients)
        {
            if (!ingredientCounts.ContainsKey(ingredient) || ingredientCounts[ingredient] == 0)
            {
                return false;
            }
            ingredientCounts[ingredient]--;
        }

        return true;
    }

    public override void TakeFromStation(PlayerInventorySlot playerInventorySlot, VoidEventChannelSO actionCompleteEventChannel)
    {
        if (_isStewing)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.CannotTakeWhileStationIsStewing);

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
