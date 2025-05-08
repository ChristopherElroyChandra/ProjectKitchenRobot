using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderUIManager : MonoBehaviour
{
    [SerializeField] OrderSingleUI _orderSingleUI;

    public void UpdateOrderUI()
    {
        foreach (Transform child in transform)
        {
            if (child.gameObject == _orderSingleUI.gameObject)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (GameLevelSO.MealOrder order in OrderManager.Instance.MealOrders)
        {
            OrderSingleUI orderSingleUI = Instantiate(_orderSingleUI, transform);
            orderSingleUI.SetOrder(order);
            orderSingleUI.gameObject.SetActive(true);
        }
    }
}
