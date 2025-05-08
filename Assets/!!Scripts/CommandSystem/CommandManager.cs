using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandManager : MonoBehaviour
{
    public static CommandManager Instance { get; private set; }

    [SerializeField] List<RobotCommandSO> _robotCommands;
    public List<RobotCommandSO> RobotCommands { get { return _robotCommands; } }

    [SerializeField] VoidEventChannelSO _robotCommandListChangedEventChannel;

    [SerializeField] VoidEventChannelSO _commandCompleteEventChannel;
    [SerializeField] VoidEventChannelSO _actionCompleteEventChannel;

    [SerializeField] VoidEventChannelSO _startProgramEventChannel;
    [SerializeField] VoidEventChannelSO _stopProgramEventChannel;

    [SerializeField] RobotCommandEventChannelSO _currentCommandEventChannel;
    [SerializeField] RobotCommandEventChannelSO _commandErrorEventChannel;

    [SerializeField] ProgramErrorUI _programErrorUI;

    

    private int _currentCommandIndex;
    public int CurrentCommandIndex { get { return _currentCommandIndex; } }

    [SerializeField] bool _isRunning;
    public bool IsRunning { get { return _isRunning; } }

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
        _isRunning = false;
    }

    private void OnEnable()
    {
        _commandCompleteEventChannel.OnEventRaised += OnCommandComplete;
        _actionCompleteEventChannel.OnEventRaised += OnActionComplete;
    }

    private void OnDisable()
    {
        _commandCompleteEventChannel.OnEventRaised -= OnCommandComplete;
        _actionCompleteEventChannel.OnEventRaised -= OnActionComplete;
    }

    private void OnActionComplete()
    {
        if (!_isRunning)
        {
            return;
        }
        _robotCommands[_currentCommandIndex].NextAction(_commandCompleteEventChannel, _actionCompleteEventChannel);
    }

    private void OnCommandComplete()
    {
        if (!_isRunning)
        {
            return;
        }
        Debug.Log("Command complete: " + _robotCommands[_currentCommandIndex].name);
        _currentCommandIndex++;
        RunCommand();
    }

    public void JumpToCommand(CommandLineVariable commandLine)
    {
        if (!_isRunning)
        {
            return;
        }
        Debug.Log("Jumping to command: " + _robotCommands[commandLine.CommandLine].name);
        _currentCommandIndex = commandLine.CommandLine;
        RunCommand();
    }

    public void StartCommands()
    {
        _programErrorUI.Dismiss();
        KitchenManager.Instance.ResetKitchenState();
        _isRunning = true;
        _currentCommandIndex = 0;
        _startProgramEventChannel.RaiseEvent();
        if (RobotCommands.Count == 0)
        {
            StopCommands();
            _programErrorUI.ShowError(ProgramErrorType.NoCommandsHaveBeenAssigned);
            _commandErrorEventChannel.RaiseEvent(null);
            return;
        }
        RunCommand();
    }

    public void StopCommands()
    {
        _isRunning = false;

        OrderManager.Instance.GetOrders();

        _currentCommandEventChannel.RaiseEvent(null);
        _stopProgramEventChannel.RaiseEvent();
    }

    public void CommandErrorOccured(ProgramErrorType errorType)
    {
        StopCommands();
        _programErrorUI.ShowError(errorType);
        if (_currentCommandIndex >= _robotCommands.Count)
        {
            _commandErrorEventChannel.RaiseEvent(_robotCommands[^1]);
        }
        else
        {
            _commandErrorEventChannel.RaiseEvent(_robotCommands[_currentCommandIndex]);
        }
    }

    private void RunCommand()
    {
        if (_currentCommandIndex >= RobotCommands.Count)
        {
            Debug.Log("All commands completed!");

            StopCommands();

            OrderManager.Instance.CheckCompletion();

            return;
        }

        RobotCommandSO commandToRun = _robotCommands[_currentCommandIndex];

        _currentCommandEventChannel.RaiseEvent(commandToRun);
        commandToRun.StartCommand(_commandCompleteEventChannel, _actionCompleteEventChannel);
    }

    public void AddOrReorderCommandToEnd(RobotCommandSO robotCommand)
    {
        int index = _robotCommands.IndexOf(robotCommand);
        if (index == -1)
        {
            Debug.Log("RobotCommand not found in RobotCommands list, adding to end");
            _robotCommands.Add(robotCommand);
        }
        else
        {
            Debug.Log("RobotCommand found in RobotCommands list, reordering");
            _robotCommands.RemoveAt(index);
            _robotCommands.Add(robotCommand);
        }

        _robotCommandListChangedEventChannel.RaiseEvent();
    }

    public void InsertCommandAtIndex(RobotCommandSO robotCommand, RobotCommandSO targetRobotCommand)
    {

        int targetIndex = _robotCommands.IndexOf(targetRobotCommand);

        if (targetIndex == -1)
        {
            // Target command not found in the list
            return;
        }

        if (!_robotCommands.Contains(robotCommand))
        {
            // Add the command to the list if it's not already in it
            _robotCommands.Add(robotCommand);
        }

        // Remove the command from its current position
        _robotCommands.Remove(robotCommand);

        // Insert the command at the target index
        _robotCommands.Insert(targetIndex, robotCommand);

        _robotCommandListChangedEventChannel.RaiseEvent();
    }

    public void RemoveCommand(RobotCommandSO robotCommand)
    {
        if (!_robotCommands.Contains(robotCommand))
        {
            return;
        }

        _robotCommands.Remove(robotCommand);
        _robotCommandListChangedEventChannel.RaiseEvent();
    }
}
