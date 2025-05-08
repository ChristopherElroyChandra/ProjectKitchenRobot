using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StewRecipe", menuName = "ScriptableObjects/Data_SO/StewRecipe")]
public class StewRecipeSO : ScriptableObject
{
    public string StewName;
    public List<KitchenIngredientSO> InputIngredients;
    public KitchenIngredientSO OutputIngredient;
}
