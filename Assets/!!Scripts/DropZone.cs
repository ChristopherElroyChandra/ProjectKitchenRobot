using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DropZone : MonoBehaviour
{
    [SerializeField] int _priority;
    public int Priority { get { return _priority; } }
    [SerializeField] protected VoidEventChannelSO _onLeftMouseReleasedEventChannel;
    
    protected bool _isPointerOver;


    private Dictionary<System.Type, System.Action<IDragable>> _dropHandlers = new Dictionary<System.Type, System.Action<IDragable>>();

    protected void SetMembers()
    {
    }

    protected virtual void RegisterDropHandlers()
    {
        Debug.LogWarning("RegisterDropHandlers not implemented for " + GetType());
    }

    protected virtual void OnLeftMouseReleased()
    {
        if (CommandManager.Instance.IsRunning) return;
        if (DragPointer.Instance.IsDragging && IsPointerOverUIElement())
        {
            // Debug.Log("Dragging " + DragPointer.Instance.DragTargetType + " to " + DragPointer.Instance.CurrentDropZone + " at " + gameObject.name);
            AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.PutBlockVariable);
            HandleDropEvent(DragPointer.Instance.DragTargetType);
        }
    }

    public void AddDropHandler<T>(System.Action<IDragable> handler) where T : IDragable
    {
        _dropHandlers.Add(typeof(T), handler);
    }

    public void HandleDropEvent(IDragable draggable)
    {
        System.Type foundType = null;
        foreach (var key in _dropHandlers.Keys)
        {
            if (key.IsAssignableFrom(draggable.GetType()))
            {
                foundType = key;
                break;
            }
        }

        if (foundType != null)
        {
            _dropHandlers[foundType](draggable);
        }
        else
        {
            Debug.LogWarning("No handler for " + draggable.GetType());
        }
    }

    public bool CanAccept(IDragable draggable)
    {
        // return _dropHandlers.ContainsKey(draggable.GetType());

        if (draggable == null)
        {
            return false;
        }

        foreach (var key in _dropHandlers.Keys)
        {
            if (key.IsAssignableFrom(draggable.GetType()))
            {
                return true;
            }
        }
        return false;
    }

    public void SetPriority()
    {
        int parentCount = 0;
        Transform parent = transform.parent;
        while (parent != null)
        {
            parentCount++;
            parent = parent.parent;
        }

        _priority = parentCount;
    }

    protected bool IsPointerOverUIElement()
    {
        return DragPointer.Instance.CurrentDropZone == this;
    }
}
