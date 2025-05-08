using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CookingRecipe", menuName = "ScriptableObjects/Data_SO/CookingRecipe")]
public class CookingRecipeSO : ScriptableObject
{
    public KitchenIngredientSO InputIngredient;
    public KitchenIngredientSO OutputIngredient;
    public int CookingTime;
}
