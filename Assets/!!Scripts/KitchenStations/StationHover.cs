using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class StationHover : MonoBehaviour
{
    [SerializeField] CommandVariableUI.VariableType _variableType;
    [SerializeField] VariableOutlineColorsSO _outlineColors;
    [SerializeField] VariableTypeEventChannelSO _enableHighlightEventChannel;
    [SerializeField] VariableTypeEventChannelSO _disableHighlightEventChannel;

    [SerializeField] float _highlightDuration;
    [SerializeField] float _targetAlpha;

    [SerializeField] SpriteRenderer _highlightSpriteRenderer;

    private bool _canHighlight;

    private void Start()
    {
        SetOutlineColor();
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

        _highlightSpriteRenderer.color = col;
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

    private void OnDisableHighlight(CommandVariableUI.VariableType arg0)
    {
        if (arg0 == CommandVariableUI.VariableType.KitchenStation)
        {
            _canHighlight = false;
            DisableHighlight();
        }
    }

    private void OnEnableHighlight(CommandVariableUI.VariableType arg0)
    {
        if (arg0 == CommandVariableUI.VariableType.KitchenStation)
        {
            _canHighlight = true;
            EnableHighlight(_targetAlpha);
        }
    }

    private void EnableHighlight(float val)
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.value(_highlightSpriteRenderer.color.a, val, 0.5f)
        .setOnUpdate((float value) =>
        {
            SetAlpha(value);
        })
        .setOnComplete(() =>
        {
            if (val > 0f)
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

    private void SetAlpha(float value)
    {
        Color col = _highlightSpriteRenderer.color;
        col.a = value;
        _highlightSpriteRenderer.color = col;
    }
}
