using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RobotCommandUI : DropZone, IDragable, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler, IPointerUpHandler
{

    [SerializeField] bool _generateNewCommand;

    [SerializeField] VariableTypeEventChannelSO _enableHighlightEventChannel;
    [SerializeField] VariableTypeEventChannelSO _disableHighlightEventChannel;

    public bool GenerateNewCommand { get { return _generateNewCommand; } }

    private Vector2 _offset;

    private RectTransform _rectTransform;
    private CanvasGroup _canvasGroup;
    private Canvas _parentCanvas;

    private CommandBlockReorderDropZone _commandBlockReorderDropZone;

    private RobotCommandSO _command;
    public RobotCommandSO Command { get => _command;}

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
        _parentCanvas = _rectTransform.GetComponentInParent<Canvas>();
        
        SetMembers();
    }

    void Start()
    {
        SetPriority();
    }

    private void Update()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(_rectTransform);
    }

    void OnEnable()
    {
        _onLeftMouseReleasedEventChannel.OnEventRaised += OnLeftMouseReleased;
    }

    void OnDisable()
    {
        _onLeftMouseReleasedEventChannel.OnEventRaised -= OnLeftMouseReleased;
    }

    protected override void OnLeftMouseReleased()
    {
        if (_generateNewCommand)
        {
            return;
        }

        base.OnLeftMouseReleased();
    }

    public void SetCommand(RobotCommandSO command, CommandBlockReorderDropZone commandBlockReorderDropZone)
    {
        _command = command;
        _commandBlockReorderDropZone = commandBlockReorderDropZone;

        RegisterDropHandlers();
        Canvas.ForceUpdateCanvases();
    }

    protected override void RegisterDropHandlers()
    {
        if (_generateNewCommand)
        {
            return;
        }

        if (_command == null)
        {
            Debug.LogWarning("Command is null");
            return;
        }

        if (_command.AcceptsKitchenStation)
        {
            if (_command.KitchenStationUsable)
            {
                // Debug.Log("Adding usable kitchen station drop zone");
                AddDropHandler<UsableKitchenStation>(HandleUsableKitchenStationDrop);
            }
            else
            {
                // Debug.Log("Adding interactible kitchen station drop zone");
                AddDropHandler<InteractibleKitchenStation>(HandleInteractibleKitchenStationDrop);
            }
        }

        if (_command.AcceptsInventoryIndex)
        {
            AddDropHandler<InventorySlotVisualUI>(HandlePlayerInventorySlotDrop);
        }

        if (_command.AcceptsCommandLine)
        {
            AddDropHandler<LineNumberUI>(HandleLineNumberDrop);
        }

        if (_command.AcceptsIngredient)
        {
            AddDropHandler<IngredientVariableUI>(HandleIngredientVariableDrop);
        }
    }

    public void SetGenerateNewCommand(bool generateNewCommand)
    {
        _generateNewCommand = generateNewCommand;
    }

    public object GetDraggedObjectData()
    {
        if (_generateNewCommand)
        {
            return (object)Instantiate(_command);
        }
        else
        {
            return (object)_command;
        }
    }

    public IDragable GetDraggedType()
    {
        return this;
    }

    public void StartDragging()
    {
        if (RectTransformUtility.RectangleContainsScreenPoint(_rectTransform, Input.mousePosition, null))
        {
            DragPointer.Instance.SetIsDragging(true);
            _canvasGroup.blocksRaycasts = false;

            _canvasGroup.alpha = 0.95f;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas.transform as RectTransform, Input.mousePosition, null, out Vector2 localPosition);
            _offset = _rectTransform.anchoredPosition - localPosition;

            SetMaskable(false);
        }
    }

    public void OnDragging()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(_parentCanvas.transform as RectTransform, Input.mousePosition, _parentCanvas.worldCamera, out Vector2 localPosition);
        _rectTransform.anchoredPosition = localPosition + _offset;
    }

    public void StopDragging()
    {
        DragPointer.Instance.SetIsDragging(false);

        _rectTransform.anchoredPosition = Vector2.zero;

        _canvasGroup.blocksRaycasts = true;
        _canvasGroup.alpha = 1f;
        SetMaskable(true);
    }

    private void SetMaskable(bool value)
    {
        MaskableGraphic[] maskableGraphics = GetComponentsInChildren<MaskableGraphic>();
        foreach (var graphic in maskableGraphics)
        {
            graphic.maskable = value;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (CommandManager.Instance.IsRunning) return;
        // Debug.Log("OnPointerUp");
        StopDragging();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (CommandManager.Instance.IsRunning) return;
        // Debug.Log("OnDrag");
        OnDragging();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (CommandManager.Instance.IsRunning) return;
        // Debug.Log("OnPointerDown");
        StartDragging();
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!_generateNewCommand) DisableHighlight();

        if (_commandBlockReorderDropZone != null)
        {
            _commandBlockReorderDropZone.DeactivateLine();
        }

        _isPointerOver = false;

        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointer.Instance.ClearDragTarget();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!_generateNewCommand) EnableHighlight();

        if (_commandBlockReorderDropZone != null)
        {
            _commandBlockReorderDropZone.CheckActivateLine();
        }
        _isPointerOver = true;

        if (DragPointer.Instance.IsDragging)
        {
            return;
        }

        DragPointer.Instance.SetDragTarget(this);
    }

    private void HandleInteractibleKitchenStationDrop(IDragable draggedObject)
    {
        Debug.Log("Handling interactible kitchen station drop");
        InteractibleKitchenStation interactibleKitchenStation = draggedObject as InteractibleKitchenStation;
        if (interactibleKitchenStation == null)
        {
            Debug.LogWarning("InteractibleKitchenStation dragged object is null");
            return;
        }

        InteractibleKitchenStation kitchenStation = interactibleKitchenStation.GetDraggedObjectData() as InteractibleKitchenStation;
        if (kitchenStation == null)
        {
            Debug.LogWarning("InteractibleKitchenStation dragged data is null");
            return;
        }


        
        _command.SetKitchenStation(kitchenStation);
    }

    private void HandleUsableKitchenStationDrop(IDragable draggedObject)
    {
        Debug.Log("Handling usable kitchen station drop");

        UsableKitchenStation usableKitchenStation = draggedObject as UsableKitchenStation;
        if (usableKitchenStation == null)
        {
            Debug.LogWarning("UsableKitchenStation dragged object is null");
            return;
        }

        UsableKitchenStation kitchenStation = usableKitchenStation.GetDraggedObjectData() as UsableKitchenStation;
        if (kitchenStation == null)
        {
            Debug.LogWarning("UsableKitchenStation dragged data is null");
            return;
        }

        _command.SetKitchenStation(kitchenStation);
    }

    private void HandleLineNumberDrop(IDragable dragable)
    {
        Debug.Log("Handling line number drop");

        LineNumberUI lineNumberUI = dragable as LineNumberUI;
        if (lineNumberUI == null)
        {
            Debug.LogWarning("LineNumberUI dragged object is null");
            return;
        }

        CommandLineVariable commandLineVariable = lineNumberUI.GetDraggedObjectData() as CommandLineVariable;
        if (commandLineVariable == null)
        {
            Debug.LogWarning("CommandLineVariable dragged data is null");
            return;
        }

        _command.SetCommandLine(commandLineVariable);
    }

    private void HandlePlayerInventorySlotDrop(IDragable draggedObject)
    {
        Debug.Log("Handling player inventory slot drop");

        InventorySlotVisualUI inventorySlotVisualUI = draggedObject as InventorySlotVisualUI;
        if (inventorySlotVisualUI == null)
        {
            Debug.LogWarning("InventorySlotVisualUI dragged object is null");
            return;
        }

        PlayerInventorySlot playerInventorySlot = inventorySlotVisualUI.GetDraggedObjectData() as PlayerInventorySlot;
        if (playerInventorySlot == null)
        {
            Debug.LogWarning("PlayerInventorySlot dragged data is null");
            return;
        }

        _command.SetInventoryIndex(playerInventorySlot);
    }

    private void HandleIngredientVariableDrop(IDragable draggedObject)
    {
        Debug.Log("Handling ingredient variable drop");

        IngredientVariableUI ingredientVariableUI = draggedObject as IngredientVariableUI;
        if (ingredientVariableUI == null)
        {
            Debug.LogWarning("IngredientVariableUI dragged object is null");
            return;
        }

        KitchenIngredientSO ingredient = ingredientVariableUI.GetDraggedObjectData() as KitchenIngredientSO;
        if (ingredient == null)
        {
            Debug.LogWarning("KitchenIngredientSO dragged data is null");
            return;
        }

        _command.SetIngredient(ingredient);
    }

    private void EnableHighlight()
    {
        if (_command.AcceptsKitchenStation)
        {
            _enableHighlightEventChannel.RaiseEvent(CommandVariableUI.VariableType.KitchenStation);
        }
        if (_command.AcceptsInventoryIndex)
        {
            _enableHighlightEventChannel.RaiseEvent(CommandVariableUI.VariableType.InventoryIndex);
        }
        if (_command.AcceptsCommandLine)
        {
            _enableHighlightEventChannel.RaiseEvent(CommandVariableUI.VariableType.CommandLine);
        }
        if (_command.AcceptsIngredient)
        {
            _enableHighlightEventChannel.RaiseEvent(CommandVariableUI.VariableType.Ingredient);
        }
    }

    private void DisableHighlight()
    {
        if (_command.AcceptsKitchenStation)
        {
            _disableHighlightEventChannel.RaiseEvent(CommandVariableUI.VariableType.KitchenStation);
        }
        if (_command.AcceptsInventoryIndex)
        {
            _disableHighlightEventChannel.RaiseEvent(CommandVariableUI.VariableType.InventoryIndex);
        }
        if (_command.AcceptsCommandLine)
        {
            _disableHighlightEventChannel.RaiseEvent(CommandVariableUI.VariableType.CommandLine);
        }
        if (_command.AcceptsIngredient)
        {
            _disableHighlightEventChannel.RaiseEvent(CommandVariableUI.VariableType.Ingredient);
        }
    }
}
