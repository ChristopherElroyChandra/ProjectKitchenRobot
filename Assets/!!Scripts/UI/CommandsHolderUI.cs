using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandsHolderUI : DropZone, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] CommandBlockSingle _commandBlockSingle;
    [SerializeField] VoidEventChannelSO _commandListChangedEventChannel;
    [SerializeField] RectTransform _contentTransform;
    [SerializeField] RectTransform _viewPortTransform;
    [SerializeField] RectTransform _bottomSpacerTransform;

    private void OnEnable()
    {
        _commandListChangedEventChannel.OnEventRaised += OnCommandListChanged;
        _onLeftMouseReleasedEventChannel.OnEventRaised += OnLeftMouseReleased;
    }

    private void OnDisable()
    {
        _commandListChangedEventChannel.OnEventRaised -= OnCommandListChanged;
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

    private void Start()
    {
        SetPriority();
        InitializeCommandBlocks();
    }

    private void OnCommandListChanged()
    {
        InitializeCommandBlocks();
    }

    private void InitializeCommandBlocks()
    {
        foreach (Transform child in _contentTransform)
        {
            if (child.gameObject == _commandBlockSingle.gameObject || child.gameObject == _bottomSpacerTransform.gameObject)
            {
                continue;
            }
            else
            {
                Destroy(child.gameObject);
            }
        }

        int lineNumber = 1;
        foreach (RobotCommandSO command in CommandManager.Instance.RobotCommands)
        {
            CommandBlockSingle commandBlockSingle = Instantiate(_commandBlockSingle, _contentTransform);
            commandBlockSingle.SetCommand(command, lineNumber);
            lineNumber++;

            commandBlockSingle.gameObject.SetActive(true);
        }

        CheckBottomSpacer();
    }

    private void CheckBottomSpacer()
    {
        float contentWithoutBottomSpacerHeight = _contentTransform.rect.height - _bottomSpacerTransform.rect.height;

        // Debug.Log("Content without bottom spacer height: " + contentWithoutBottomSpacerHeight + " Viewport height: " + _viewPortTransform.rect.height);


        if (contentWithoutBottomSpacerHeight > (_viewPortTransform.rect.height * 0.8f))
        {
            _bottomSpacerTransform.gameObject.SetActive(true);
        }
        else
        {
            _bottomSpacerTransform.gameObject.SetActive(false);
        }

        _bottomSpacerTransform.SetSiblingIndex(_contentTransform.childCount - 1);
    }

    private void HandleRobotCommandUIDrop(IDragable draggedObject)
    {
        Debug.Log("Block holder drop zone");
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

        CommandManager.Instance.AddOrReorderCommandToEnd(robotCommand);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isPointerOver = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isPointerOver = true;
    }
}
