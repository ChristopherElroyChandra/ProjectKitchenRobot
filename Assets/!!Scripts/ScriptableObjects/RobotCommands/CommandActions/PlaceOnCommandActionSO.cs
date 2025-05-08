using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlaceOn", menuName = "ScriptableObjects/CommandSystem/CommandActions/PlaceOn")]
public class PlaceOnCommandActionSO : CommandActionSO
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

        if (parentRobotCommand.AcceptsInventoryIndex)
        {
            PlayerInventorySlot inventorySlot = parentRobotCommand.GetInventoryIndex();

            if (inventorySlot == null)
            {
                // Popup error message

                CommandManager.Instance.CommandErrorOccured(ProgramErrorType.InventorySlotNotAssigned);

                Debug.Log("InventoryIndex not assigned!");

                return;
            }

            PlayerActionReceiver.Instance.PlaceOnStation(parentRobotCommand.GetKitchenStation(), actionCompleteEventChannel, inventorySlot);
        }
        else
        {
            PlayerActionReceiver.Instance.PlaceOnStation(parentRobotCommand.GetKitchenStation(), actionCompleteEventChannel);
        }
    }
}
