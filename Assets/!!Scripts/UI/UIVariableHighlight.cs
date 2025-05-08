using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIVariableHighlight : MonoBehaviour
{
    [SerializeField] CommandVariableUI.VariableType _variableType;
    [SerializeField] VariableOutlineColorsSO _outlineColors;
    [SerializeField] Image _highlightImage;

    Color _highlightColor;

    [SerializeField] float _highlightDuration;
    [SerializeField] float _targetAlpha;

    [SerializeField] VariableTypeEventChannelSO _enableHighlightEventChannel;
    [SerializeField] VariableTypeEventChannelSO _disableHighlightEventChannel;

    private bool _canHighlight;

    private void Start()
    {
        SetOutlineColor();
    }

    private void Update()
    {
        if (_highlightImage != null)
        {
            _highlightImage.color = _highlightColor;
        }
    }

    private void OnEnable()
    {
        _enableHighlightEventChannel.OnEventRaised += OnEnableHighlight;
        _disableHighlightEventChannel.OnEventRaised += OnDisableHighlight;
    }

    private void OnDisable()
    {
        _enableHighlightEventChannel.OnEventRaised -= OnEnableHighlight;
        _disableHighlightEventChannel.OnEventRaised -= OnDisableHighlight;
    }

    private void OnEnableHighlight(CommandVariableUI.VariableType arg0)
    {
        if (arg0 == _variableType)
        {
            _canHighlight = true;
            EnableHighlight(_targetAlpha);
        }
    }

    private void OnDisableHighlight(CommandVariableUI.VariableType arg0)
    {
        if (arg0 == _variableType)
        {
            _canHighlight = false;
            DisableHighlight();
        }
    }

    private void EnableHighlight(float targetAlpha)
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.value(_highlightColor.a, targetAlpha, _highlightDuration)
        .setOnUpdate((float value) =>
        {
            SetAlpha(value);
        })
        .setOnComplete(() =>
        {
            if (targetAlpha > 0f)
            {
                EnableHighlight(0f);
            }
            else
            {
                if (!_canHighlight)
                {
                    DisableHighlight();
                    return;
                }
                EnableHighlight(_targetAlpha);
            }
        });
    }

    private void DisableHighlight()
    {
        if (LeanTween.isTweening(this.gameObject))
        {
            LeanTween.cancel(this.gameObject);
        }

        SetAlpha(0f);
    }

    private void SetAlpha(float v)
    {
        Color col = _highlightColor;
        col.a = v;
        _highlightColor = col;
    }

    private void SetOutlineColor()
    {
        var col = _variableType switch
        {
            CommandVariableUI.VariableType.InventoryIndex => _outlineColors.InventoryIndexOutlineColor,
            CommandVariableUI.VariableType.CommandLine => _outlineColors.CommandLineOutlineColor,
            CommandVariableUI.VariableType.TickAmount => _outlineColors.TickAmountOutlineColor,
            CommandVariableUI.VariableType.Ingredient => _outlineColors.IngredientOutlineColor,
            _ => _outlineColors.KitchenStationOutlineColor,
        };

        col.a = 0f;

        _highlightColor = col;
    }
}
