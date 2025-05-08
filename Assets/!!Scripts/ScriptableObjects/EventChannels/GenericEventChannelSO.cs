using UnityEngine;
using UnityEngine.Events;

public class GenericEventChannelSO<T> : DescriptionSO
{
    [Tooltip("The action to perform")]
    public UnityAction<T> OnEventRaised;

    public void RaiseEvent(T parameter)
    {

        if (OnEventRaised == null)
            return;

        OnEventRaised.Invoke(parameter);

    }
}


// Example usage:

// Example for a generic event channel with a int parameter

// using UnityEngine;

// [CreateAssetMenu(fileName = "IntEventChannel", menuName = "ScriptableObjects/EventChannels/IntEventChannel")]
// public class IntEventChannelSO : GenericEventChannelSO<int>
// {
// }