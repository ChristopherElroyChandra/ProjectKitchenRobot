using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OrderSingleUI : MonoBehaviour
{
    [SerializeField] Image _orderImage;
    [SerializeField] TextMeshProUGUI _orderQuantityText;

    [SerializeField] OrderIngredientsUI _orderIngredientsUI;
    internal void SetOrder(GameLevelSO.MealOrder order)
    {
        _orderImage.sprite = order.mealRecipe.MealIcon;
        _orderQuantityText.text = order.quantity.ToString() + "x";
        _orderIngredientsUI.SetRecipe(order.mealRecipe);
    }
}
