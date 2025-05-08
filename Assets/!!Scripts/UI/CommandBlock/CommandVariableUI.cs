using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandVariableUI : MonoBehaviour
{
    [SerializeField] VariableOutlineColorsSO _outlineColors;
    [SerializeField] RobotCommandUI _robotCommandUI;
    [SerializeField] VariableType _variableType;
    [SerializeField] Image _variableImage;
    [SerializeField] TextMeshProUGUI _variableText;


    [SerializeField] Outline _outline;

    private void Start()
    {
        SetOutlineColor();
    }

    private void Update()
    {
        switch (_variableType)
        {
            case VariableType.KitchenStation:
                SetUIKitchenStation();
                break;
            case VariableType.InventoryIndex:
                SetUIInventoryIndex();
                break;
            case VariableType.CommandLine:
                SetUICommandLine();
                break;
            case VariableType.TickAmount:
                SetUITickAmount();
                break;
            case VariableType.Ingredient:
                SetUIIngredient();
                break;
        }
    }

    private void SetUIKitchenStation()
    {
        if (_robotCommandUI.Command.GetKitchenStation() == null)
        {
            _variableImage.gameObject.SetActive(false);
            _variableText.gameObject.SetActive(false);
        }
        else
        {
            _variableImage.gameObject.SetActive(true);
            _variableText.gameObject.SetActive(true);

            DragPointerDataSO dragPointerData = _robotCommandUI.Command.GetKitchenStation().GetDragPointerData();

            _variableImage.sprite = dragPointerData.PointerIconSprite;
            _variableText.text = dragPointerData.PointerNameText;
        }
    }

    public void SetUIInventoryIndex()
    {
        if (_robotCommandUI.Command.GetInventoryIndex() == null)
        {
            _variableImage.gameObject.SetActive(false);
            _variableText.gameObject.SetActive(false);
        }
        else
        {
            int index = PlayerActionReceiver.Instance.GetIndexOfInventorySlot(_robotCommandUI.Command.GetInventoryIndex());
            if (index < 0)
            {
                _variableImage.gameObject.SetActive(false);
                _variableText.gameObject.SetActive(false);
                return;
            }

            _variableImage.gameObject.SetActive(false);
            _variableText.gameObject.SetActive(true);

            PlayerInventorySlot inventorySlot = _robotCommandUI.Command.GetInventoryIndex();

            _variableText.text = "Slot " + (PlayerActionReceiver.Instance.GetIndexOfInventorySlot(inventorySlot) + 1);
        }
    }

    public void SetUICommandLine()
    {
        if (_robotCommandUI.Command.GetCommandLine() == null)
        {
            _variableImage.gameObject.SetActive(false);
            _variableText.gameObject.SetActive(false);
        }
        else
        {
            _variableImage.gameObject.SetActive(false);
            _variableText.gameObject.SetActive(true);

            _variableText.text = "Line " + (_robotCommandUI.Command.GetCommandLine().CommandLine + 1).ToString();
        }
    }

    public void SetUITickAmount()
    {
        if (_robotCommandUI.Command.GetTickAmount() == null)
        {
            _variableImage.gameObject.SetActive(false);
            _variableText.gameObject.SetActive(false);
        }
        else
        {
            _variableImage.gameObject.SetActive(false);
            _variableText.gameObject.SetActive(true);

            string tick = ((float)_robotCommandUI.Command.GetTickAmount().TickAmount / GameTimeManager.Instance.TicksPerSecond).ToString("0.00");

            _variableText.text = tick;
        }
    }

    public void SetUIIngredient()
    {
        if (_robotCommandUI.Command.GetIngredient() == null)
        {
            _variableImage.gameObject.SetActive(false);
            _variableText.gameObject.SetActive(false);
        }
        else
        {
            _variableImage.gameObject.SetActive(true);
            _variableText.gameObject.SetActive(true);

            KitchenIngredientSO ingredientSO = _robotCommandUI.Command.GetIngredient();

            _variableImage.sprite = ingredientSO.IngredientIcon;
            _variableText.text = ingredientSO.IngredientName;
        }
    }

    private void SetOutlineColor()
    {
        switch (_variableType)
        {
            case VariableType.KitchenStation:
                _outline.effectColor = _outlineColors.KitchenStationOutlineColor;
                break;
            case VariableType.InventoryIndex:
                _outline.effectColor = _outlineColors.InventoryIndexOutlineColor;
                break;
            case VariableType.CommandLine:
                _outline.effectColor = _outlineColors.CommandLineOutlineColor;
                break;
            case VariableType.TickAmount:
                _outline.effectColor = _outlineColors.TickAmountOutlineColor;
                break;
            case VariableType.Ingredient:
                _outline.effectColor = _outlineColors.IngredientOutlineColor;
                break;
        }
    }

    public enum VariableType
    {
        KitchenStation,
        InventoryIndex,
        CommandLine,
        TickAmount,
        Ingredient
    }

    [System.Serializable]
    public struct OutlineColors
    {
        public Color KitchenStationOutlineColor;
        public Color InventoryIndexOutlineColor;
        public Color CommandLineOutlineColor;
        public Color TickAmountOutlineColor;
        public Color IngredientOutlineColor;
    }
}
