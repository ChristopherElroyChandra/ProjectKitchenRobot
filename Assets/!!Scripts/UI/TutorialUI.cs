using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] GameObject _tutorialPanel;
    [SerializeField] Image _tutorialImage;

    [SerializeField] Button _openTutorialButton;

    [SerializeField] Button _closeTutorialButton;

    [SerializeField] Button _nextTutorialButton;
    [SerializeField] TextMeshProUGUI _tutorialText;
    [SerializeField] Button _prevTutorialButton;

    private int _currentTutorialIndex;

    private void Awake()
    {
        _currentTutorialIndex = 0;
        _nextTutorialButton.onClick.AddListener(NextTutorial);
        _prevTutorialButton.onClick.AddListener(PrevTutorial);
        _closeTutorialButton.onClick.AddListener(HideTutorial);
        _openTutorialButton.onClick.AddListener(OnOpenTutorialButtonClick);
    }

    private void OnOpenTutorialButtonClick()
    {
        if (CanOpenTutorial())
        {
            ShowTutorial();
        }
    }

    private void Start()
    {
        CheckIfDisplayTutorial();
    }

    private void CheckIfDisplayTutorial()
    {
        if (DataManager.Instance.IsLevelCompleted(LevelManager.Instance.GameLevel) == GameLevelStatus.Completed)
        {
            _tutorialPanel.SetActive(false);
            return;
        }

        if (!CanOpenTutorial())
        {
            _tutorialPanel.SetActive(false);
            _openTutorialButton.interactable = false;
            _openTutorialButton.gameObject.SetActive(false);
            return;
        }

        ShowTutorial();
    }

    public bool CanOpenTutorial()
    {
        return LevelManager.Instance.GameLevel.HasTutorial;
    }

    private void ShowTutorial()
    {
        _currentTutorialIndex = 0;

        SetTutorialPageData();

        _tutorialPanel.SetActive(true);
    }

    private void HideTutorial()
    {
        _tutorialPanel.SetActive(false);
    }

    private void SetTutorialPageData()
    {
        _tutorialImage.sprite = LevelManager.Instance.GameLevel.TutorialSprites[_currentTutorialIndex];

        _tutorialText.text = (_currentTutorialIndex + 1).ToString() + "/" + LevelManager.Instance.GameLevel.TutorialSprites.Count.ToString();

        if (_currentTutorialIndex == 0)
        {
            _prevTutorialButton.interactable = false;
        }
        else
        {
            _prevTutorialButton.interactable = true;
        }

        if (_currentTutorialIndex == LevelManager.Instance.GameLevel.TutorialSprites.Count - 1)
        {
            _nextTutorialButton.interactable = false;
        }
        else
        {
            _nextTutorialButton.interactable = true;
        }

    }

    private void PrevTutorial()
    {
        if (_currentTutorialIndex > 0)
        {
            _currentTutorialIndex--;
        }
        
        SetTutorialPageData();
    }

    private void NextTutorial()
    {
        if (_currentTutorialIndex < LevelManager.Instance.GameLevel.TutorialSprites.Count - 1)
        {
            _currentTutorialIndex++;
        }
        
        SetTutorialPageData();
    }
}
