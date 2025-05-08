using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDragable
{
    public object GetDraggedObjectData();
    public IDragable GetDraggedType();
    void StartDragging();
    void OnDragging();
    void StopDragging();
}
