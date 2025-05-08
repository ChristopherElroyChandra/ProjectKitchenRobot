using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UseStation", menuName = "ScriptableObjects/CommandSystem/CommandActions/UseStation")]
public class UseStationCommandActionSO : CommandActionSO
{
    public override void Execute(VoidEventChannelSO actionCompleteEventChannel, RobotCommandSO parentRobotCommand)
    {
        if (!parentRobotCommand.AcceptsKitchenStation)
        {
            Debug.LogWarning("parent RobotCommandSO does not accept a kitchen station! RobotCommandSO configuration mismatch!");
            return;
        }

        if (!parentRobotCommand.KitchenStationUsable)
        {
            Debug.LogWarning("KitchenStationUsable is false! RobotCommandSO configuration mismatch!");
            return;
        }

        if (parentRobotCommand.GetKitchenStation() == null)
        {
            // Popup error message

            CommandManager.Instance.CommandErrorOccured(ProgramErrorType.KitchenStationNotAssigned);

            Debug.Log("KitchenStation not assigned!");

            return;
        }

        UsableKitchenStation usableStation = parentRobotCommand.GetKitchenStation() as UsableKitchenStation;

        if (usableStation == null)
        {
            Debug.LogWarning("KitchenStation is not a UsableKitchenStation");
            Debug.LogWarning("RobotCommandSO misconfigured or data input validation failed");
            return;
        }

        usableStation.Use(actionCompleteEventChannel);
    }
}
