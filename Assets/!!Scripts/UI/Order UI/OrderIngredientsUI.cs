using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrderIngredientsUI : MonoBehaviour
{
    [SerializeField] IngredientVariableUI _ingredientImage;

    internal void SetRecipe(MealRecipeSO mealRecipe)
    {
        foreach (KitchenIngredientSO ingredient in mealRecipe.MealIngredients)
        {
            IngredientVariableUI ingredientVar = Instantiate(_ingredientImage, transform);
            ingredientVar.SetIngredient(ingredient);
            ingredientVar.gameObject.SetActive(true);
        }
    }
}
