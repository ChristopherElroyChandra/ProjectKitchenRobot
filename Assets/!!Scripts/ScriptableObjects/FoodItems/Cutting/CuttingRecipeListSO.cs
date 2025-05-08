using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipeList", menuName = "ScriptableObjects/Data_SO/RecipeList/CuttingRecipeList")]
public class CuttingRecipeListSO : ScriptableObject
{
    public List<CuttingRecipeSO> CuttingRecipes;
}
