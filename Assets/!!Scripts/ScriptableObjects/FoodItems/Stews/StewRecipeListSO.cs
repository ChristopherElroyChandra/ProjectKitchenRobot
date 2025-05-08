using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StewRecipeList", menuName = "ScriptableObjects/Data_SO/RecipeList/StewRecipeList")]
public class StewRecipeListSO : ScriptableObject
{
    public List<StewRecipeSO> StewRecipes;
}
