using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NewCommandsBoxUI : MonoBehaviour
{
    [SerializeField] ScrollRect _scrollRect;
    [SerializeField] RectTransform _contentTransform;
    [SerializeField] RectTransform _viewPortTransform;
    [SerializeField] RectTransform _bottomSpacerTransform;

    private void Start()
    {
        foreach (RobotCommandSO command in LevelManager.Instance.GameLevel.RobotCommands)
        {
            GameObject commandUIGO = Instantiate(command.Prefab, _contentTransform);
            commandUIGO.SetActive(true);

            RobotCommandUI commandUI = commandUIGO.GetComponentInChildren<RobotCommandUI>();

            commandUI.SetCommand(command, null);
            commandUI.SetGenerateNewCommand(true);
        }

        CheckBottomSpacer();

        _scrollRect.verticalNormalizedPosition = 1f;
    }

    private void CheckBottomSpacer()
    {
        float contentWithoutBottomSpacerHeight = _contentTransform.rect.height - _bottomSpacerTransform.rect.height;
        
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
}
