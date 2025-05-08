
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CommandTrashUI : DropZone, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Sprite _trashClosedIcon;
    [SerializeField] Sprite _trashOpenIcon;
    [SerializeField] Image _trashImage;

    void Awake()
    {
        SetMembers();

        RegisterDropHandlers();
    }

    void OnEnable()
    {
        _onLeftMouseReleasedEventChannel.OnEventRaised += OnLeftMouseReleased;
    }

    void OnDisable()
    {
        _onLeftMouseReleasedEventChannel.OnEventRaised -= OnLeftMouseReleased;
    }

    void Start()
    {
        SetPriority();
    }

    protected override void RegisterDropHandlers()
    {
        AddDropHandler<RobotCommandUI>(HandleRobotCommandUIDrop);
    }

    private void HandleRobotCommandUIDrop(IDragable draggedObject)
    {
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

        Debug.Log("Removing command: " + robotCommand.name);

        CommandManager.Instance.RemoveCommand(robotCommand);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _trashImage.sprite = _trashClosedIcon;
        _isPointerOver = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _trashImage.sprite = _trashOpenIcon;
        _isPointerOver = true;
    }
}
