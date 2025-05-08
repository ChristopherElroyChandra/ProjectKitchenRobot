using System.Collections;
using System.Collections.Generic;
using Unity.Burst.Intrinsics;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/EventChannels/VariableTypeEventChannel", fileName = "VariableTypeEventChannel")]
public class VariableTypeEventChannelSO : GenericEventChannelSO<CommandVariableUI.VariableType>
{
    
}
