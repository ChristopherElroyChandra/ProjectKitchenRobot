using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaitCommandBlockUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] GameObject _buttonHolderGO;
    [SerializeField] Button _addButton;
    [SerializeField] Button _decreaseButton;

    [SerializeField] RobotCommandUI _robotCommandUI;

    private void Start()
    {
        _addButton.onClick.AddListener(AddTicks);
        _decreaseButton.onClick.AddListener(DecreaseTicks);
    }

    private void DecreaseTicks()
    {
        TickAmountVariable tickAmount = _robotCommandUI.Command.GetTickAmount();

        if (tickAmount == null)
        {
            tickAmount = new TickAmountVariable();
            tickAmount.TickAmount = GameTimeManager.Instance.TicksPerSecond;
        }

        _robotCommandUI.Command.SetTickAmount(new TickAmountVariable { TickAmount = tickAmount.TickAmount - GameTimeManager.Instance.TicksPerSecond });
    }

    private void AddTicks()
    {
        TickAmountVariable tickAmount = _robotCommandUI.Command.GetTickAmount();

        if (tickAmount == null)
        {
            tickAmount = new TickAmountVariable();
            tickAmount.TickAmount = GameTimeManager.Instance.TicksPerSecond;
        }

        _robotCommandUI.Command.SetTickAmount(new TickAmountVariable { TickAmount = tickAmount.TickAmount + GameTimeManager.Instance.TicksPerSecond });
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _buttonHolderGO.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_robotCommandUI.GenerateNewCommand)
        {
            return;
        }
        _buttonHolderGO.SetActive(true);
    }
}
