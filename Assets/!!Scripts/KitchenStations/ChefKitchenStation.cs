using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using UnityEngine;

public class ChefKitchenStation : UsableKitchenStation
{
    
    private static List<KitchenIngredientSO> _ingredients = new List<KitchenIngredientSO>();
    public static List<KitchenIngredientSO> Ingredients { get { return _ingredients; } }

    [SerializeField] MealRecipeListSO _recipeList;

    protected override void Awake()
    {
        base.Awake();

        RegisterOnResetAction(ResetIngredients);
    }

    private void ResetIngredients()
    {
        _ingredients.Clear();
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

        if (!playerInventorySlot.Ingredient.AcceptedByChefStation)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.IngredientNotAcceptedByChefStation);

            Debug.Log("Ingredient not accepted by chef station");
            return;
        }

        _ingredients.Add(playerInventorySlot.Ingredient);
        playerInventorySlot.Ingredient = null;

        AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.PlaceChefStation);

        ChefStationListVisual.Instance.SetStationList(_ingredients);

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            actionCompleteEventChannel.RaiseEvent();
        });
    }

    public override void TakeFromStation(PlayerInventorySlot playerInventorySlot, VoidEventChannelSO actionCompleteEventChannel)
    {
        // Show error message

        CommandManager.Instance.CommandErrorOccured(ProgramErrorType.CannotTakeFromChefStation);

        Debug.Log("Cannot take from chef station");

        return;
    }

    public override void Use(VoidEventChannelSO actionCompleteEventChannel)
    {
        if (_ingredients.Count == 0)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.ChefStationIsEmpty);

            Debug.Log("Chef station is empty");
            return;
        }

        MealRecipeSO recipe = GetRecipeFromIngredients();

        if (recipe == null)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.SubmittedIngredientsNotARecipe);

            Debug.Log("Recipe not found");
            return;
        }

        _ingredients.Clear();
        ChefStationListVisual.Instance.SetStationList(_ingredients);

        AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.UseChefStation);

        OrderManager.Instance.SubmitOrder(recipe);

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            actionCompleteEventChannel.RaiseEvent();
        });
    }

    private MealRecipeSO GetRecipeFromIngredients()
    {
        foreach (MealRecipeSO recipe in _recipeList.MealRecipes)
        {
            if (IngredientsEqualsRecipe(recipe))
            {
                return recipe;
            }
        }

        return null;
    }

    private bool IngredientsEqualsRecipe(MealRecipeSO recipe)
    {
        if (_ingredients == null || recipe.MealIngredients == null)
        {
            return false;
        }

        if (_ingredients.Count != recipe.MealIngredients.Count)
        {
            return false;
        }

        var ingredientCounts = _ingredients.GroupBy(x => x)
        .ToDictionary(g => g.Key, g => g.Count());

        foreach (var ingredient in recipe.MealIngredients)
        {
            if (!ingredientCounts.ContainsKey(ingredient) || ingredientCounts[ingredient] == 0)
            {
                return false;
            }
            ingredientCounts[ingredient]--;
        }

        return true;
    }

    protected override void SetDragPointerData()
    {
        _dragPointerData = ScriptableObject.CreateInstance<DragPointerDataSO>();
        _dragPointerData.PointerNameText = "Chef station";
        _dragPointerData.PointerIconSprite = _sprite;
        _dragPointerData.PointerDescriptionText = "Place ingredients of recipes. Use this station to send the ingredients to the chef.";
    }
}
