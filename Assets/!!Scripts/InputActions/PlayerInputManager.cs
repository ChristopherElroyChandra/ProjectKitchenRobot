using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    [SerializeField] VoidEventChannelSO _onLeftMouseReleasedEventChannel;
    [SerializeField] VoidEventChannelSO _onEscapeKeyEventChannel;
    [SerializeField] VoidEventChannelSO _onSpaceKeyEventChannel;
    [SerializeField] VoidEventChannelSO _onOneKeyEventChannel;
    [SerializeField] VoidEventChannelSO _onThreeKeyEventChannel;

    private PlayerInputActions _playerInputActions;
    private PlayerInputActions.GameplayActions _gameplayActions;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
            return;
        }

        Instance = this;

        _playerInputActions = new PlayerInputActions();
        _gameplayActions = _playerInputActions.Gameplay;
    }
    
    private void Start()
    {
        _gameplayActions.MouseButton.started += OnLeftMousePressed;
        _gameplayActions.MouseButton.canceled += OnLeftMouseReleased;
        _gameplayActions.Pause.performed += OnEscapeKeyEvent;
        _gameplayActions.StartStop.performed += OnSpaceKeyEvent;
        _gameplayActions.PlusMultiplier.performed += OnOneKeyEvent;
        _gameplayActions.MinMultiplier.performed += OnThreeKeyEvent;
    }

    private void OnEnable()
    {
        EnableGameInput();
    }

    private void OnDisable()
    {
        DisableGameInput();
    }

    public void EnableGameInput()
    {
        _gameplayActions.Enable();
    }

    public void DisableGameInput()
    {
        _gameplayActions.Disable();
    }

    private void OnLeftMouseReleased(InputAction.CallbackContext context)
    {
        _onLeftMouseReleasedEventChannel.RaiseEvent();
        // Debug.Log("Left mouse released");
    }

    private void OnThreeKeyEvent(InputAction.CallbackContext context)
    {
        // _onThreeKeyEventChannel.RaiseEvent();
    }

    private void OnOneKeyEvent(InputAction.CallbackContext context)
    {
        // _onOneKeyEventChannel.RaiseEvent();
    }

    private void OnSpaceKeyEvent(InputAction.CallbackContext context)
    {
        // _onSpaceKeyEventChannel.RaiseEvent();
    }

    private void OnEscapeKeyEvent(InputAction.CallbackContext context)
    {
        // _onEscapeKeyEventChannel.RaiseEvent();
    }
    
    private void OnLeftMousePressed(InputAction.CallbackContext context)
    {
        if (IsPointerOverUIElement(out GameObject hitObject))
        {
            if (hitObject != null && hitObject.GetComponent<Button>() != null)
            {
                Debug.Log("Button pressed");
                AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.ButtonPress);
                return;
            }
            // else
            // {
            //     Debug.Log("Left mouse pressed while over UI");
            //     AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.Click);
            // }
        }
        // else
        // {
        //     Debug.Log("Left mouse pressed");
        //     AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.Click);
        // }
        Debug.Log("Left mouse pressed");
        AudioClipPlayer.Instance.PlayAudioClip(AudioClipPlayer.AudioClips.Click);
    }

    private bool IsPointerOverUIElement(out GameObject hitObject)
    {
        PointerEventData pointerEventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerEventData, raycastResults);

        if (raycastResults.Count > 0)
        {
            hitObject = raycastResults[0].gameObject;
            return true;
        }

        hitObject = null;
        return false;
    }
}
