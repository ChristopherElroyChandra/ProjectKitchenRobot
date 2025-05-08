using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectSingleUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _levelNameText;
    [SerializeField] TextMeshProUGUI _levelDescriptionText;
    [SerializeField] Button _levelButton;
    [SerializeField] Image _stampImage;
    [SerializeField] Sprite _completedStamp;
    [SerializeField] Sprite _lockedStamp;
    [SerializeField] Image _lockedMask;

    [SerializeField] GameLevelSO _levelSO;

    private void Start()
    {
        _levelButton.onClick.AddListener(LevelButtonClick);
    }

    private void LevelButtonClick()
    {
        if (_levelSO == null) return;
        LevelManager.Instance.LoadLevel(_levelSO);
    }

    public void SetLevel(GameLevelSO level, GameLevelStatus levelStatus)
    {
        _levelSO = level;

        _levelNameText.text = level.LevelName;
        _levelDescriptionText.text = level.LevelDescription;

        if (levelStatus == GameLevelStatus.Locked)
        {
            _stampImage.sprite = _lockedStamp;
            _stampImage.gameObject.SetActive(true);

            _lockedMask.gameObject.SetActive(true);
            _levelButton.interactable = false;
        }
        else
        {
            _levelButton.interactable = true;
            _lockedMask.gameObject.SetActive(false);

            if (levelStatus == GameLevelStatus.Completed)
            {
                _stampImage.sprite = _completedStamp;
                _stampImage.gameObject.SetActive(true);
            }
            else
            {
                _stampImage.gameObject.SetActive(false);
            }
        }

    }
}
