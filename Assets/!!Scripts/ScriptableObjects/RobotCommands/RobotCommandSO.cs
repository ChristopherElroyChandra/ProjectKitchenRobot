using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "New Robot Command", menuName = "ScriptableObjects/CommandSystem/RobotCommand")]
public class RobotCommandSO : ScriptableObject
{
    public GameObject Prefab;
    [Header("Variables")]
    [Header("Kitchen Station")]
    [SerializeField] bool _acceptsKitchenStation;
    public bool AcceptsKitchenStation { get { return _acceptsKitchenStation; } }
    [SerializeField] bool _kitchenStationUsable;
    public bool KitchenStationUsable { get { return _kitchenStationUsable; } }
    private InteractibleKitchenStation _kitchenStation;

    [Header("Inventory Index")]
    [SerializeField] bool _acceptsInventoryIndex;
    public bool AcceptsInventoryIndex { get { return _acceptsInventoryIndex; } }
    private PlayerInventorySlot _inventoryIndex;

    [Header("Command Line")]
    [SerializeField] bool _acceptsCommandLine;
    public bool AcceptsCommandLine { get { return _acceptsCommandLine; } }
    private CommandLineVariable _commandLine;

    [Header("Tick Amount")]
    [SerializeField] bool _acceptsTickAmount;
    public bool AcceptsTickAmount { get { return _acceptsTickAmount; } }
    private TickAmountVariable _tickAmount;

    [Header("Ingredient")]
    [SerializeField] bool _acceptsIngredient;
    public bool AcceptsIngredient { get { return _acceptsIngredient; } }
    [SerializeField] bool _checkContains;
    public bool CheckContains { get { return _checkContains; } }
    private KitchenIngredientSO _ingredientVariable;

    public List<CommandActionSO> Actions;
    private int _currentActionIndex;

    public void SetKitchenStation(InteractibleKitchenStation kitchenStation)
    {
        if (!_acceptsKitchenStation)
        {
            Debug.LogWarning("Attempted to set kitchen station on RobotCommandSO that does not accept kitchen station");
            return;
        }
        if (KitchenStationUsable && kitchenStation is not UsableKitchenStation)
        {
            Debug.LogWarning("KitchenStation is not a UsableKitchenStation");
            Debug.LogWarning("RobotCommandSO misconfigured or data input validation failed");
            return;
        }
        _kitchenStation = kitchenStation;
    }

    public void SetInventoryIndex(PlayerInventorySlot inventoryIndex)
    {
        if (!_acceptsInventoryIndex)
        {
            Debug.LogWarning("Attempted to set inventory index on RobotCommandSO that does not accept inventory index");
            return;
        }
        _inventoryIndex = inventoryIndex;
    }

    public void SetCommandLine(CommandLineVariable commandLine)
    {
        if (!_acceptsCommandLine)
        {
            Debug.LogWarning("Attempted to set command line on RobotCommandSO that does not accept command line");
            return;
        }
        _commandLine = commandLine;
    }

    public void SetTickAmount(TickAmountVariable tickAmount)
    {
        if (!_acceptsTickAmount)
        {
            Debug.LogWarning("Attempted to set tick amount on RobotCommandSO that does not accept tick amount");
            return;
        }

        tickAmount.TickAmount = Mathf.Clamp(tickAmount.TickAmount, GameTimeManager.Instance.TicksPerSecond, int.MaxValue);
        
        _tickAmount = tickAmount;
    }

    public void SetIngredient(KitchenIngredientSO ingredientVariable)
    {
        if (!_acceptsIngredient)
        {
            Debug.LogWarning("Attempted to set ingredient on RobotCommandSO that does not accept ingredient");
            return;
        }
        _ingredientVariable = ingredientVariable;
    }

    public InteractibleKitchenStation GetKitchenStation()
    {
        return _kitchenStation;
    }

    public PlayerInventorySlot GetInventoryIndex()
    {
        return _inventoryIndex;
    }

    public CommandLineVariable GetCommandLine()
    {
        return _commandLine;
    }

    public TickAmountVariable GetTickAmount()
    {
        return _tickAmount;
    }

    public KitchenIngredientSO GetIngredient()
    {
        return _ingredientVariable;
    }

    public void StartCommand(VoidEventChannelSO commandCompleteEventChannel, VoidEventChannelSO actionCompleteEventChannel)
    {
        // if (!CheckCommandInputValidity())
        // {
        //     Debug.LogWarning("Command input invalid!");
        //     return;
        // }

        Debug.Log("Starting command " + name);

        _currentActionIndex = 0;

        ExecuteAction(commandCompleteEventChannel, actionCompleteEventChannel);
    }

    public void NextAction(VoidEventChannelSO commandCompleteEventChannel, VoidEventChannelSO actionCompleteEventChannel)
    {
        _currentActionIndex++;

        ExecuteAction(commandCompleteEventChannel, actionCompleteEventChannel);
    }

    private void ExecuteAction(VoidEventChannelSO commandCompleteEventChannel, VoidEventChannelSO actionCompleteEventChannel)
    {
        if (_currentActionIndex >= Actions.Count)
        {
            commandCompleteEventChannel.RaiseEvent();
            return;
        }

        CommandActionSO action = Actions[_currentActionIndex];
        action.Execute(actionCompleteEventChannel, this);
    }
}

public class CommandLineVariable
{
    public int CommandLine;
}

public class TickAmountVariable
{
    public int TickAmount;

    public TickAmountVariable()
    {
        TickAmount = 1;
    }
}