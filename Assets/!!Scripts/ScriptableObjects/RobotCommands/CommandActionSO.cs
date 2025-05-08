using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CommandActionSO : ScriptableObject
{
    public virtual void Execute(VoidEventChannelSO actionCompleteEventChannel, RobotCommandSO parentRobotCommand)
    {
        Debug.LogWarning("CommandActionSO.Execute() has not been implemented!");
    }
}
