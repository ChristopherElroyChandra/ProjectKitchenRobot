using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance { get; private set; }

    [SerializeField] OrderUIManager _orderUIManager;

    [SerializeField] List<GameLevelSO.MealOrder> _mealOrders;
    public List<GameLevelSO.MealOrder> MealOrders { get { return _mealOrders; } }
    
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        GetOrders();
    }

    public void GetOrders()
    {
        _mealOrders = new List<GameLevelSO.MealOrder>();
        foreach (GameLevelSO.MealOrder order in LevelManager.Instance.GameLevel.MealOrders)
        {
            GameLevelSO.MealOrder newOrder = new(order);
            _mealOrders.Add(newOrder);
        }
        _orderUIManager.UpdateOrderUI();
    }

    public void CheckCompletion()
    {
        if (_mealOrders.Count == 0)
        {
            // TODO: Show completion message

            Debug.Log("Orders completed");
            CommandManager.Instance.StopCommands();

            LevelCompletedUI.Instance.LevelCompleted();
        }
        else
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.OrdersNotCompleted);

            Debug.Log("Orders not completed");
        }
    }

    public void SubmitOrder(MealRecipeSO mealRecipe)
    {
        GameLevelSO.MealOrder order = _mealOrders.Find(x => x.mealRecipe == mealRecipe);
        if (order == null)
        {
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.SubmittedMealNotOrdered);

            Debug.Log("Submitted incorect order");
            
            return;
        }

        if (order.quantity <= 0)
        {
            // Should never happen in theory, but just in case
            // Show error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.SubmittedMealNotOrdered);

            Debug.Log("Submitted incorrect order");
            
            return;
        }

        Debug.Log("Submitting order " + order.mealRecipe.MealName);

        order.quantity--;

        if (order.quantity <= 0)
        {
            _mealOrders.Remove(order);
        }

        _orderUIManager.UpdateOrderUI();

        if (_mealOrders.Count == 0)
        {
            CheckCompletion();
        }
    }
}
