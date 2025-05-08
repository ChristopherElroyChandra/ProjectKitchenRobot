using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SettingsPausePanelUI : MonoBehaviour
{
    [SerializeField] Image _fadeBackground;
    [SerializeField] RectTransform _menuPanel;
    [SerializeField] float _animTime;

    [SerializeField] TextMeshProUGUI _headerText;
    [SerializeField] GameObject _settingsPanel;
    [SerializeField] GameObject _pausePanel;

    [SerializeField] Button _closeButton;

    [Header("Menu")]
    [SerializeField] Button _continueButton;
    [SerializeField] Button _settingsButton;
    [SerializeField] Button _mainMenuButton;
    
    private float _hiddenXPos;
    private float _shownXPos;

    private void Start()
    {
        _closeButton.onClick.AddListener(ClosePanel);
        _continueButton.onClick.AddListener(ClosePanel);
        _settingsButton.onClick.AddListener(OpenSettingsPanel);
        _mainMenuButton.onClick.AddListener(MainMenuButtonClick);

        _shownXPos = _menuPanel.localPosition.x;
        _hiddenXPos = _shownXPos + _menuPanel.rect.width;

        _fadeBackground.color = new Color(0, 0, 0, 0);
        // LeanTween.moveLocalX(_menuPanel.gameObject, _hiddenXPos, 0f);
        _menuPanel.localPosition = new Vector3(_hiddenXPos, 0, 0);

        gameObject.SetActive(false);
    }

    private void MainMenuButtonClick()
    {
        SceneHandler.Instance.LoadScene(GameSceneNames.MainMenuScene.ToString());
    }

    public void OpenPausePanel()
    {
        _settingsPanel.SetActive(false);
        _pausePanel.SetActive(true);
        _headerText.text = "Menu";

        OpenAnimation();
    }

    public void OpenSettingsPanel()
    {
        _settingsPanel.SetActive(true);
        _pausePanel.SetActive(false);
        _headerText.text = "Settings";

        OpenAnimation();
    }

    public void ClosePanel()
    {
        CloseAnimation();
    }

    private void OpenAnimation()
    {
        gameObject.SetActive(true);

        if (LeanTween.isTweening(_menuPanel))
        {
            LeanTween.cancel(_menuPanel);
        }
        if (LeanTween.isTweening(_fadeBackground.gameObject))
        {
            LeanTween.cancel(_fadeBackground.gameObject);
        }

        LeanTween.moveLocalX(_menuPanel.gameObject, _shownXPos, _animTime);
        LeanTween.value(_fadeBackground.gameObject, new Color(0, 0, 0, 0), new Color(0, 0, 0, 0.7f), _animTime).setOnUpdate((Color val)=>
        {
            _fadeBackground.color = val;
        }).setEase(LeanTweenType.easeInOutCubic);
    }

    private void CloseAnimation()
    {
        if (LeanTween.isTweening(_menuPanel))
        {
            LeanTween.cancel(_menuPanel);
        }
        if (LeanTween.isTweening(_fadeBackground.gameObject))
        {
            LeanTween.cancel(_fadeBackground.gameObject);
        }

        LeanTween.moveLocalX(_menuPanel.gameObject, _hiddenXPos, _animTime);
        LeanTween.value(_fadeBackground.gameObject, new Color(0, 0, 0, 0.7f), new Color(0, 0, 0, 0), _animTime).setOnUpdate((Color val)=>
        {
            _fadeBackground.color = val;
        }).setOnComplete(()=>
        {
            gameObject.SetActive(false);
        }).setEase(LeanTweenType.easeInOutCubic);

    }
}
