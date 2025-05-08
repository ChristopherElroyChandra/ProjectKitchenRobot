using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelectUI : MonoBehaviour
{
    [SerializeField] LevelSelectSingleUI _levelSelectTemplate;
    [SerializeField] GameLevelListSO _levelList;

    [SerializeField] GameObject _levelSelectPanel;

    [SerializeField] Button _backButton;
    [SerializeField] Button _nextPageButton;
    [SerializeField] Button _previousPageButton;

    [SerializeField] List<LevelSelectPageUI> _pages;

    private int _currentPage;

    private void Start()
    {
        _pages = new List<LevelSelectPageUI>();
        _backButton.onClick.AddListener(BackButtonClick);
        _nextPageButton.onClick.AddListener(NextPageButtonClick);
        _previousPageButton.onClick.AddListener(PreviousPageButtonClick);

        _currentPage = 0;

        LevelSelectPageUI page = new LevelSelectPageUI();
        foreach (GameLevelSO level in _levelList.Levels)
        {
            page.Levels.Add(level);
            if (page.Levels.Count == 3)
            {
                _pages.Add(page);
                page = new LevelSelectPageUI();
            }
        }

        gameObject.SetActive(false);
    }

    public void OpenLevelSelectPanel()
    {
        gameObject.SetActive(true);
        SetLevelSelects(_pages[_currentPage]);
    }

    private void SetLevelSelects(LevelSelectPageUI page)
    {
        foreach (Transform child in _levelSelectPanel.transform)
        {
            if (child.gameObject == _levelSelectTemplate.gameObject)
            {
                continue;
            }
            Destroy(child.gameObject);
        }

        foreach (GameLevelSO level in page.Levels)
        {
            LevelSelectSingleUI levelUI = Instantiate(_levelSelectTemplate, _levelSelectPanel.transform);
            levelUI.SetLevel(level, DataManager.Instance.IsLevelCompleted(level));
            levelUI.gameObject.SetActive(true);
        }
    }

    private void BackButtonClick()
    {
        gameObject.SetActive(false);
    }

    private void NextPageButtonClick()
    {
        if (_currentPage < _pages.Count - 1)
        {
            _currentPage++;
            SetLevelSelects(_pages[_currentPage]);
        }
    }

    private void PreviousPageButtonClick()
    {
        if (_currentPage > 0)
        {
            _currentPage--;
            SetLevelSelects(_pages[_currentPage]);
        }
    }
}

[System.Serializable]
public class LevelSelectPageUI
{
    public List<GameLevelSO> Levels;
    
    public LevelSelectPageUI()
    {
        Levels = new List<GameLevelSO>();
    }
}
