using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MealRecipeList", menuName = "ScriptableObjects/Data_SO/RecipeList/MealRecipeList")]
public class MealRecipeListSO : ScriptableObject
{
    public List<MealRecipeSO> MealRecipes;
}
