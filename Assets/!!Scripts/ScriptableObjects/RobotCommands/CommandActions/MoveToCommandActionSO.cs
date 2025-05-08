using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MoveTo", menuName = "ScriptableObjects/CommandSystem/CommandActions/MoveTo")]
public class MoveToCommandActionSO : CommandActionSO
{
    public override void Execute(VoidEventChannelSO actionCompleteEventChannel, RobotCommandSO parentRobotCommand)
    {
        if (!parentRobotCommand.AcceptsKitchenStation)
        {
            Debug.LogWarning("parent RobotCommandSO does not accept a kitchen station! RobotCommandSO configuration mismatch!");
            return;
        }

        if (parentRobotCommand.GetKitchenStation() == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.KitchenStationNotAssigned);

            Debug.Log("KitchenStation not assigned!");

            return;
        }

        PlayerActionReceiver.Instance.MoveTowards(parentRobotCommand.GetKitchenStation(), actionCompleteEventChannel);
    }
}
