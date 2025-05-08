using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstOrderRecipeSO : RecipeVariableSO
{
    public override MealRecipeSO GetRecipe()
    {
        if (OrderManager.Instance.MealOrders.Count == 0)
        {
            return null;
        }
        else
        {
            return OrderManager.Instance.MealOrders[0].mealRecipe;
        }
    }
}
