using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgramErrorUI : MonoBehaviour
{
    [SerializeField] float _initialYPos;
    [SerializeField] float _hiddenYPos;

    [SerializeField] ErrorMessagesSO _errorMessages;

    [SerializeField] float _animTime;

    [SerializeField] TextMeshProUGUI _errorDescriptionText;
    [SerializeField] Button _dismissButton;

    [SerializeField] RobotCommandEventChannelSO _commandErrorEventChannel;

    private void Awake()
    {
        _initialYPos = transform.localPosition.y;
        _hiddenYPos = _initialYPos + GetComponent<RectTransform>().rect.height + 100f;
    }

    private void Start()
    {
        _dismissButton.onClick.AddListener(Dismiss);

        LeanTween.moveLocalY(this.gameObject, _hiddenYPos, 0f);
    }

    public void ShowError(ProgramErrorType errorType)
    {
        _errorDescriptionText.text = _errorMessages.GetErrorMessage(errorType);
        ShowWindow();
    }

    public void Dismiss()
    {
        HideWindow();
        _commandErrorEventChannel.RaiseEvent(null);
        KitchenManager.Instance.ResetKitchenState();
    }

    private void HideWindow()
    {
        if (LeanTween.isTweening(this.gameObject))
        {
            LeanTween.cancel(this.gameObject);
        }

        LeanTween.moveLocalY(this.gameObject, _hiddenYPos, _animTime);
    }

    private void ShowWindow()
    {
        if (LeanTween.isTweening(this.gameObject))
        {
            LeanTween.cancel(this.gameObject);
        }

        LeanTween.moveLocalY(this.gameObject, _initialYPos, _animTime);
    }
}

public enum ProgramErrorType
{
    NoIngredientToTakeFromCounter,
    PlayerAlreadyHasAnIngredient,
    NoIngredientInInventorySlot,
    StationAlreadyHasAnIngredient,
    CannotTakeFromChefStation,
    ChefStationIsEmpty,
    SubmittedIngredientsNotARecipe,
    CanotPlaceOnContainer,
    PlacedIngredientCannotBeCooked,
    CannotTakeWhileStationIsCooking,
    PlacedIngredientCannotBeCut,
    CannotTakeWhileStationIsStewing,
    NoPathToKitchenStation,
    OrdersNotCompleted,
    SubmittedMealNotOrdered,
    CommandLineNotAssigned,
    IngredientNotAssigned,
    InventorySlotNotAssigned,
    KitchenStationNotAssigned,
    TickAmountNotAssigned,
    NoCommandsHaveBeenAssigned,
    IngredientNotAcceptedByChefStation,
    InfiniteLoopDetected,
    CommandLineIndexOutOfBounds
}