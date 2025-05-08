using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JumpCommandAction", menuName = "ScriptableObjects/CommandSystem/CommandActions/JumpCommandActionSO")]
public class JumpCommandActionSO : CommandActionSO
{
    public override void Execute(VoidEventChannelSO actionCompleteEventChannel, RobotCommandSO parentRobotCommand)
    {
        if (!parentRobotCommand.AcceptsCommandLine)
        {
            Debug.LogWarning("parent RobotCommandSO does not accept a command line! RobotCommandSO configuration mismatch!");
        }

        if (parentRobotCommand.GetCommandLine() == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.CommandLineNotAssigned);

            Debug.Log("CommandLine not assigned!");

            return;
        }

        CommandLineVariable commandLine = parentRobotCommand.GetCommandLine();

        if (commandLine.CommandLine == CommandManager.Instance.CurrentCommandIndex)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.InfiniteLoopDetected);

            Debug.Log("Infinite loop detected!");
            return;
        }

        if (commandLine.CommandLine < 0 || commandLine.CommandLine >= CommandManager.Instance.RobotCommands.Count)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.CommandLineIndexOutOfBounds);

            Debug.Log("CommandLine index out of bounds!");
            return;
        }

        
        

        LeanTween.value(0, 1, GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            CommandManager.Instance.JumpToCommand(parentRobotCommand.GetCommandLine());
            // actionCompleteEventChannel.RaiseEvent();
        });
    }
}
