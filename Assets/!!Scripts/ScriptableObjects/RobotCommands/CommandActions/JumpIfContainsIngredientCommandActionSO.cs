using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpIfContainsIngredientCommandAction", menuName = "ScriptableObjects/CommandSystem/CommandActions/JumpIfContainsIngredientCommandActionSO")]
public class JumpIfContainsIngredientCommandActionSO : CommandActionSO
{
    public override void Execute(VoidEventChannelSO actionCompleteEventChannel, RobotCommandSO parentRobotCommand)
    {
        if (!parentRobotCommand.AcceptsCommandLine)
        {
            Debug.LogWarning("Attempted to execute JumpIfContainsIngredientCommandActionSO on RobotCommandSO that does not accept command line");
            return;
        }

        if (!parentRobotCommand.AcceptsIngredient)
        {
            Debug.LogWarning("Attempted to execute JumpIfContainsIngredientCommandActionSO on RobotCommandSO that does not accept ingredient");
            return;
        }
        
        if (parentRobotCommand.GetCommandLine() == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.CommandLineNotAssigned);

            Debug.Log("CommandLine not assigned!");
            return;
        }

        if (parentRobotCommand.GetIngredient() == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.IngredientNotAssigned);

            Debug.Log("Ingredient not assigned!");
            return;
        }

        if (OrderManager.Instance == null)
        {
            Debug.LogWarning("OrderManager not initialized!");
            return;
        }

        KitchenIngredientSO ingredient = parentRobotCommand.GetIngredient();

        bool contains = OrderManager.Instance.MealOrders[0].mealRecipe.MealIngredients.Contains(ingredient);

        if (contains == parentRobotCommand.CheckContains)
        {
            LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
            .setOnComplete(() =>
            {
                CommandManager.Instance.JumpToCommand(parentRobotCommand.GetCommandLine());
                // actionCompleteEventChannel.RaiseEvent();
            });
        }
        else
        {
            LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
            .setOnComplete(() =>
            {
                actionCompleteEventChannel.RaiseEvent();
            });
        }
    }
}
