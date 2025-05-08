using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "CookingRecipeList", menuName = "ScriptableObjects/Data_SO/RecipeList/CookingRecipeList")]
public class CookingRecipeListSO : ScriptableObject
{
    public List<CookingRecipeSO> CookingRecipes;
}
