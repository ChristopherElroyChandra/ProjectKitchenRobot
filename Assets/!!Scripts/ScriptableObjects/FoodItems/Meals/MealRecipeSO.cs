using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MealRecipe", menuName = "ScriptableObjects/Data_SO/MealRecipe")]
public class MealRecipeSO : ScriptableObject
{
    public string MealName;
    public Sprite MealIcon;
    public List<KitchenIngredientSO> MealIngredients;
}
