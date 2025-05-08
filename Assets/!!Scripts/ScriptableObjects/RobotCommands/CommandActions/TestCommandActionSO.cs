using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Test Command Action", menuName = "ScriptableObjects/CommandSystem/CommandActions/Test")]
public class TestCommandActionSO : CommandActionSO
{
    public override void Execute(VoidEventChannelSO actionCompleteEventChannel, RobotCommandSO parentRobotCommand)
    {
        Debug.Log("Test Command Action Executed!");

        LeanTween.value(0,0, 1f).setOnComplete(()=>
        {
            actionCompleteEventChannel.RaiseEvent();
        });
    }
}
