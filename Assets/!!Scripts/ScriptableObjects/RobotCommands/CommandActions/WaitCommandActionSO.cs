using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WaitCommandAction", menuName = "ScriptableObjects/CommandSystem/CommandActions/WaitCommandActionSO")]
public class WaitCommandActionSO : CommandActionSO
{
    public override void Execute(VoidEventChannelSO actionCompleteEventChannel, RobotCommandSO parentRobotCommand)
    {
        if (!parentRobotCommand.AcceptsTickAmount)
        {
            Debug.LogWarning("parent RobotCommandSO does not accept a tick amount! RobotCommandSO configuration mismatch!");
            return;
        }

        if (parentRobotCommand.GetTickAmount() == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.TickAmountNotAssigned);

            Debug.Log("TickAmount not assigned!");

            return;
        }

        LeanTween.value(0, 1, parentRobotCommand.GetTickAmount().TickAmount * GameTimeManager.Instance.TickInterval)
        .setOnComplete(() =>
        {
            actionCompleteEventChannel.RaiseEvent();
        });
    }
}
