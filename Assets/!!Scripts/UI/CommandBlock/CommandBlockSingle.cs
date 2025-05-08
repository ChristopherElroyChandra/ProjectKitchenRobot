using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandBlockSingle : MonoBehaviour
{
    [SerializeField] int _topPadding;
    [SerializeField] LineNumberUI _lineNumberUI;

    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] RectTransform _scrollRectContent;

    [SerializeField] Image _commandBlockBacklight;

    [SerializeField] GameObject _insertLineVisual;
    [SerializeField] CommandBlockReorderDropZone _commandBlockReorderDropZone;

    [SerializeField] RobotCommandEventChannelSO _currentCommandEventChannel;
    [SerializeField] RobotCommandEventChannelSO _commandErrorEventChannel;

    private int _lineNumber;
    private RobotCommandSO _command;
    public RobotCommandSO Command { get { return _command; } }

    private void OnEnable()
    {
        _currentCommandEventChannel.OnEventRaised += OnCurrentCommandRaised;
        _commandErrorEventChannel.OnEventRaised += OnCommandErrorRaised;
    }

    private void OnDisable()
    {
        _currentCommandEventChannel.OnEventRaised -= OnCurrentCommandRaised;
        _commandErrorEventChannel.OnEventRaised -= OnCommandErrorRaised;
    }

    private void OnCurrentCommandRaised(RobotCommandSO arg0)
    {
        if (_command == arg0)
        {
            _commandBlockBacklight.color = Color.green;
            _commandBlockBacklight.gameObject.SetActive(true);
            ScrollToThis();
        }
        else
        {
            _commandBlockBacklight.gameObject.SetActive(false);
        }
    }

    private void OnCommandErrorRaised(RobotCommandSO arg0)
    {
        if (_command == arg0)
        {
            _commandBlockBacklight.color = Color.red;
            _commandBlockBacklight.gameObject.SetActive(true);
            ScrollToThis();
        }
        else
        {
            _commandBlockBacklight.gameObject.SetActive(false);
        }
    }

    private void ScrollToThis()
    {
        Vector3 worldPosition = transform.position;
        Vector3 localPosition = _scrollRectContent.InverseTransformPoint(worldPosition);

        float normalizedPosition = Mathf.Clamp01((localPosition.y - _scrollRect.viewport.rect.height / 2f) / (_scrollRectContent.rect.height - _scrollRect.viewport.rect.height));

        _scrollRect.verticalNormalizedPosition = 1f - normalizedPosition;
    }

    public void SetCommand(RobotCommandSO command, int lineNumber)
    {
        _lineNumber = lineNumber;
        _command = command;

        _lineNumberUI.SetLineNumber(lineNumber);
        GameObject gameObject = Instantiate(_command.Prefab, this.transform);
        RobotCommandUI robotCommandUI = gameObject.GetComponentInChildren<RobotCommandUI>();
        robotCommandUI.SetCommand(command, _commandBlockReorderDropZone);

        RectTransform goRect = gameObject.GetComponent<RectTransform>();
        RectTransform rectTransform = this.GetComponent<RectTransform>();
        RectTransform lineRect = _lineNumberUI.gameObject.GetComponent<RectTransform>();

        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, goRect.sizeDelta.y + _topPadding);

        lineRect.sizeDelta = new Vector2(lineRect.sizeDelta.x, goRect.sizeDelta.y);
        lineRect.anchoredPosition = Vector2.zero;

        RectTransform insertLineRect = _insertLineVisual.GetComponent<RectTransform>();
        Vector2 pos = lineRect.anchoredPosition;
        
        pos.y -= (_topPadding - insertLineRect.rect.height) / 2f;

        insertLineRect.anchoredPosition = pos;
    }
}
