using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SidebarUI : MonoBehaviour
{
    [Header("Pause")]
    [SerializeField] Button _pauseButton;
    [SerializeField] VoidEventChannelSO _onEscapeKeyEvent;
    [SerializeField] SettingsPausePanelUI _pausePanel;
    private bool _isPaused { get { return _pausePanel.gameObject.activeSelf; } }

    [Header("Time Multiplier")]
    [SerializeField] Button _timeMultiplierIncreaseButton;
    [SerializeField] Button _timeMultiplierDecreaseButton;
    [SerializeField] VoidEventChannelSO _onOneKeyEvent;
    [SerializeField] VoidEventChannelSO _onThreeKeyEvent;
    [SerializeField] TextMeshProUGUI _timeMultiplierText;

    [Header("Start Stop Program")]
    [SerializeField] Button _startButton;
    [SerializeField] Button _stopButton;
    [SerializeField] VoidEventChannelSO _onSpaceKeyEvent;

    private void Start()
    {
        _pauseButton.onClick.AddListener(OnPauseButtonClicked);
        _timeMultiplierIncreaseButton.onClick.AddListener(OnTimeMultiplierIncreaseButtonClicked);
        _timeMultiplierDecreaseButton.onClick.AddListener(OnTimeMultiplierDecreaseButtonClicked);
        _startButton.onClick.AddListener(OnStartButtonClicked);
        _stopButton.onClick.AddListener(OnStopButtonClicked);
    }

    private void Update()
    {
        if (GameTimeManager.Instance.CanIncreaseTimeMultiplier())
        {
            _timeMultiplierIncreaseButton.interactable = true;
        }
        else
        {
            _timeMultiplierIncreaseButton.interactable = false;
        }

        if (GameTimeManager.Instance.CanDecreaseTimeMultiplier())
        {
            _timeMultiplierDecreaseButton.interactable = true;
        }
        else
        {
            _timeMultiplierDecreaseButton.interactable = false;
        }

        _timeMultiplierText.text = GameTimeManager.Instance.TimeMultiplier.ToString() + "x";

        if (CommandManager.Instance.IsRunning)
        {
            _startButton.gameObject.SetActive(false);
            _stopButton.gameObject.SetActive(true);
        }
        else
        {
            _startButton.gameObject.SetActive(true);
            _stopButton.gameObject.SetActive(false);
        }
    }

    private void OnPauseButtonClicked()
    {
        TogglePause();
    }

    private void OnTimeMultiplierIncreaseButtonClicked()
    {
        if (GameTimeManager.Instance.CanIncreaseTimeMultiplier())
        {
            GameTimeManager.Instance.IncreaseTimeMultiplier();
        }
    }

    private void OnTimeMultiplierDecreaseButtonClicked()
    {
        if (GameTimeManager.Instance.CanDecreaseTimeMultiplier())
        {
            GameTimeManager.Instance.DecreaseTimeMultiplier();
        }
    }

    private void OnStartButtonClicked()
    {
        ToggleStartStopProgram();
    }

    private void OnStopButtonClicked()
    {
        ToggleStartStopProgram();
    }

    private void OnEnable()
    {
        _onEscapeKeyEvent.OnEventRaised += OnEscapeKeyEventRaised;
        _onOneKeyEvent.OnEventRaised += OnOneKeyEventRaised;
        _onThreeKeyEvent.OnEventRaised += OnThreeKeyEventRaised;
        _onSpaceKeyEvent.OnEventRaised += OnSpaceKeyEventRaised;
    }

    private void OnDisable()
    {
        _onEscapeKeyEvent.OnEventRaised -= OnEscapeKeyEventRaised;
        _onOneKeyEvent.OnEventRaised -= OnOneKeyEventRaised;
        _onThreeKeyEvent.OnEventRaised -= OnThreeKeyEventRaised;
        _onSpaceKeyEvent.OnEventRaised -= OnSpaceKeyEventRaised;
    }

    private void OnEscapeKeyEventRaised()
    {
        TogglePause();
    }

    private void OnOneKeyEventRaised()
    {
        if (_isPaused)
        {
            return;
        }
        OnTimeMultiplierDecreaseButtonClicked();
    }

    private void OnThreeKeyEventRaised()
    {
        if (_isPaused)
        {
            return;
        }
        OnTimeMultiplierIncreaseButtonClicked();
    }

    private void OnSpaceKeyEventRaised()
    {
        if (_isPaused)
        {
            return;
        }
        ToggleStartStopProgram();
    }

    private void TogglePause()
    {
        if (_isPaused)
        {
            _pausePanel.ClosePanel();
        }
        else
        {
            _pausePanel.OpenPausePanel();
        }
    }

    private void ToggleStartStopProgram()
    {
        if (CommandManager.Instance.IsRunning)
        {
            CommandManager.Instance.StopCommands();
        }
        else
        {
            CommandManager.Instance.StartCommands();
        }
    }
}
