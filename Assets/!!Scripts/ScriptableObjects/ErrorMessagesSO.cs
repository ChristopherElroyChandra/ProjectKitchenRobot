using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ErrorMessages", menuName = "ScriptableObjects/Data_SO/ErrorMessages")]
public class ErrorMessagesSO : ScriptableObject
{
    [TextArea(1, 10)]
    public string NoIngredientToTakeFromCounterErrorMessage;
    [TextArea(1, 10)]
    public string PlayerAlreadyHasAnIngredientErrorMessage;
    [TextArea(1, 10)]
    public string NoIngredientInInventorySlotErrorMessage;
    [TextArea(1, 10)]
    public string StationAlreadyHasAnIngredientErrorMessage;
    [TextArea(1, 10)]
    public string CannotTakeFromChefStationErrorMessage;
    [TextArea(1, 10)]
    public string ChefStationIsEmptyErrorMessage;
    [TextArea(1, 10)]
    public string SubmittedIngredientsNotARecipeErrorMessage;
    [TextArea(1, 10)]
    public string CanotPlaceOnContainerErrorMessage;
    [TextArea(1, 10)]
    public string PlacedIngredientCannotBeCookedErrorMessage;
    [TextArea(1, 10)]
    public string CannotTakeWhileStationIsCookingErrorMessage;
    [TextArea(1, 10)]
    public string PlacedIngredientCannotBeCutErrorMessage;
    [TextArea(1, 10)]
    public string CannotTakeWhileStationIsStewingErrorMessage;
    [TextArea(1, 10)]
    public string NoPathToKitchenStationErrorMessage;
    [TextArea(1, 10)]
    public string OrdersNotCompletedErrorMessage;
    [TextArea(1, 10)]
    public string SubmittedMealNotOrderedErrorMessage;
    [TextArea(1, 10)]
    public string CommandLineNotAssignedErrorMessage;
    [TextArea(1, 10)]
    public string IngredientNotAssignedErrorMessage;
    [TextArea(1, 10)]
    public string InventorySlotNotAssignedErrorMessage;
    [TextArea(1, 10)]
    public string KitchenStationNotAssignedErrorMessage;
    [TextArea(1, 10)]
    public string TickAmountNotAssignedErrorMessage;
    [TextArea(1, 10)]
    public string NoCommandsHaveBeenAssignedErrorMessage;
    [TextArea(1, 10)]
    public string IngredientNotAcceptedByChefStationErrorMessage;
    [TextArea(1, 10)]
    public string InfiniteLoopDetectedErrorMessage;
    [TextArea(1, 10)]
    public string CommandLineIndexOutOfBoundsErrorMessage;
    
    public string GetErrorMessage(ProgramErrorType errorType)
    {
        return errorType switch
        {
            ProgramErrorType.NoIngredientToTakeFromCounter => NoIngredientToTakeFromCounterErrorMessage,
            ProgramErrorType.PlayerAlreadyHasAnIngredient => PlayerAlreadyHasAnIngredientErrorMessage,
            ProgramErrorType.NoIngredientInInventorySlot => NoIngredientInInventorySlotErrorMessage,
            ProgramErrorType.StationAlreadyHasAnIngredient => StationAlreadyHasAnIngredientErrorMessage,
            ProgramErrorType.CannotTakeFromChefStation => CannotTakeFromChefStationErrorMessage,
            ProgramErrorType.ChefStationIsEmpty => ChefStationIsEmptyErrorMessage,
            ProgramErrorType.SubmittedIngredientsNotARecipe => SubmittedIngredientsNotARecipeErrorMessage,
            ProgramErrorType.CanotPlaceOnContainer => CanotPlaceOnContainerErrorMessage,
            ProgramErrorType.PlacedIngredientCannotBeCooked => PlacedIngredientCannotBeCookedErrorMessage,
            ProgramErrorType.CannotTakeWhileStationIsCooking => CannotTakeWhileStationIsCookingErrorMessage,
            ProgramErrorType.PlacedIngredientCannotBeCut => PlacedIngredientCannotBeCutErrorMessage,
            ProgramErrorType.CannotTakeWhileStationIsStewing => CannotTakeWhileStationIsStewingErrorMessage,
            ProgramErrorType.NoPathToKitchenStation => NoPathToKitchenStationErrorMessage,
            ProgramErrorType.OrdersNotCompleted => OrdersNotCompletedErrorMessage,
            ProgramErrorType.SubmittedMealNotOrdered => SubmittedMealNotOrderedErrorMessage,
            ProgramErrorType.CommandLineNotAssigned => CommandLineNotAssignedErrorMessage,
            ProgramErrorType.IngredientNotAssigned => IngredientNotAssignedErrorMessage,
            ProgramErrorType.InventorySlotNotAssigned => InventorySlotNotAssignedErrorMessage,
            ProgramErrorType.KitchenStationNotAssigned => KitchenStationNotAssignedErrorMessage,
            ProgramErrorType.TickAmountNotAssigned => TickAmountNotAssignedErrorMessage,
            ProgramErrorType.NoCommandsHaveBeenAssigned => NoCommandsHaveBeenAssignedErrorMessage,
            ProgramErrorType.IngredientNotAcceptedByChefStation => IngredientNotAcceptedByChefStationErrorMessage,
            ProgramErrorType.InfiniteLoopDetected => InfiniteLoopDetectedErrorMessage,
            ProgramErrorType.CommandLineIndexOutOfBounds => CommandLineIndexOutOfBoundsErrorMessage,
            _ => "Error",
        };
    }
}
