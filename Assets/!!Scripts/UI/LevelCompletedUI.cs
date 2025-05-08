using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelCompletedUI : MonoBehaviour
{
    public static LevelCompletedUI Instance { get; private set; }

    [SerializeField] TextMeshProUGUI _levelNameText;
    [SerializeField] TextMeshProUGUI _levelDescriptionText;

    [SerializeField] RectTransform levelCompletedUI;
    [SerializeField] RectTransform blocker;

    [SerializeField] float _animTime;

    [SerializeField] Button _improveButton;
    [SerializeField] Button _mainMenuButton;

    private float _initialYPos;
    private float _hiddenYPos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        _improveButton.onClick.AddListener(ImproveButtonClicked);
        _mainMenuButton.onClick.AddListener(MainMenuButtonClicked);

        blocker.gameObject.SetActive(false);
        _initialYPos = levelCompletedUI.localPosition.y;
        _hiddenYPos = _initialYPos + levelCompletedUI.rect.height;

        LeanTween.moveLocalY(levelCompletedUI.gameObject, _hiddenYPos, 0f);
        levelCompletedUI.gameObject.SetActive(false);
    }

    private void ImproveButtonClicked()
    {
        HideMenu();
    }

    private void MainMenuButtonClicked()
    {
        SceneHandler.Instance.LoadScene(GameSceneNames.MainMenuScene.ToString());
    }

    public void LevelCompleted()
    {
        _levelNameText.text = LevelManager.Instance.GameLevel.LevelName + " Completed";
        _levelDescriptionText.text = LevelManager.Instance.GameLevel.LevelDescription;

        DataManager.Instance.SetLevelCompletion(LevelManager.Instance.GameLevel);

        blocker.gameObject.SetActive(true);
        ShowMenu();
    }

    private void ShowMenu()
    {
        if (LeanTween.isTweening(levelCompletedUI.gameObject))
        {
            LeanTween.cancel(levelCompletedUI.gameObject);
        }

        levelCompletedUI.gameObject.SetActive(true);
        LeanTween.moveLocalY(levelCompletedUI.gameObject, _initialYPos, _animTime);
    }

    private void HideMenu()
    {
        if (LeanTween.isTweening(levelCompletedUI.gameObject))
        {
            LeanTween.cancel(levelCompletedUI.gameObject);
        }

        LeanTween.moveLocalY(levelCompletedUI.gameObject, _hiddenYPos, _animTime)
        .setOnComplete(() =>
        {
            levelCompletedUI.gameObject.SetActive(false);
            blocker.gameObject.SetActive(false);
        });
    }
}
