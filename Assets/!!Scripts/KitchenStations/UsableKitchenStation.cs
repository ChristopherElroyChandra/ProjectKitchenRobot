using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UsableKitchenStation : InteractibleKitchenStation
{
    public override object GetDraggedObjectData()
    {
        return (object)this;
    }

    public override IDragable GetDraggedType()
    {
        return this;
    }

    public virtual void Use(VoidEventChannelSO actionCompleteEventChannel)
    {
        Debug.LogWarning("Use not implemented for " + name);
    }
}
