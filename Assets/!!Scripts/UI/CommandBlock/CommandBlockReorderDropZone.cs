using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandBlockReorderDropZone : DropZone, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CommandBlockSingle _commandBlockSingle;

    [SerializeField] GameObject _insertLine;

    private void OnEnable()
    {
        _onLeftMouseReleasedEventChannel.OnEventRaised += OnLeftMouseReleased;
    }

    private void OnDisable()
    {
        _onLeftMouseReleasedEventChannel.OnEventRaised -= OnLeftMouseReleased;
    }


    private void Awake()
    {
        SetMembers();

        RegisterDropHandlers();
    }

    protected override void RegisterDropHandlers()
    {
        AddDropHandler<RobotCommandUI>(HandleRobotCommandUIDrop);
    }

    void Start()
    {
        SetPriority(); 
    }

    private void HandleRobotCommandUIDrop(IDragable draggedObject)
    {
        Debug.Log("Block reorder drop zone");
        RobotCommandUI robotCommandUI = draggedObject as RobotCommandUI;
        if (robotCommandUI == null)
        {
            Debug.LogWarning("RobotCommandUI dragged object is null");
            return;
        }

        RobotCommandSO robotCommand = robotCommandUI.GetDraggedObjectData() as RobotCommandSO;
        if (robotCommand == null)
        {
            Debug.LogWarning("RobotCommand dragged data is null");
            return;
        }

        CommandManager.Instance.InsertCommandAtIndex(robotCommand, _commandBlockSingle.Command);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOver = false;
        DeactivateLine();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOver = true;

        CheckActivateLine();
    }

    public void CheckActivateLine()
    {
        if (DragPointer.Instance.IsDragging)
        {
            if (CanAccept(DragPointer.Instance.DragTargetType))
            {
                _insertLine.SetActive(true);
            }
            else
            {
                _insertLine.SetActive(false);
            }
        }
    }

    public void DeactivateLine()
    {
        
        _insertLine.SetActive(false);
    }
}
