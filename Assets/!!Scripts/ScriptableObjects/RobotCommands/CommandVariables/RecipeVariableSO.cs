using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecipeVariableSO : ScriptableObject
{
    public MealRecipeSO Recipe;

    public virtual MealRecipeSO GetRecipe()
    {
        return Recipe;
    }

    public bool RecipeContains(KitchenIngredientSO ingredient)
    {
        return Recipe.MealIngredients.Contains(ingredient);
    }

    public bool RecipeNotContains(KitchenIngredientSO ingredient)
    {
        return !RecipeContains(ingredient);
    }
}
