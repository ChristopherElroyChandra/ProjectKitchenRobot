using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CuttingRecipe", menuName = "ScriptableObjects/Data_SO/CuttingRecipe")]
public class CuttingRecipeSO : ScriptableObject
{
    public KitchenIngredientSO InputIngredient;
    public KitchenIngredientSO OutputIngredient;
    public int CutsNeeded;
}
