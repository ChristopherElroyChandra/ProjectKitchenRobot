using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainButtonPanelUI : MonoBehaviour
{
    [SerializeField] Button StartButton;
    [SerializeField] Button OptionsButton;
    [SerializeField] Button CreditsButton;
    [SerializeField] Button ExitButton;


    [SerializeField] LevelSelectUI _levelSelectUI;
    [SerializeField] SettingsPausePanelUI _settingsPausePanelUI;
    [SerializeField] CreditsPanelUI _creditsPanelUI;

    void Start()
    {
        StartButton.onClick.AddListener(OnStartButtonClick);
        OptionsButton.onClick.AddListener(OnOptionsButtonClick);
        CreditsButton.onClick.AddListener(OnCreditsButtonClick);
        ExitButton.onClick.AddListener(OnExitButtonClick);
    }

    private void OnStartButtonClick()
    {
        // Go to the game scene
        _levelSelectUI.OpenLevelSelectPanel();
    }

    private void OnOptionsButtonClick()
    {
        _settingsPausePanelUI.OpenSettingsPanel();
    }

    private void OnCreditsButtonClick()
    {
        _creditsPanelUI.OpenCreditsPanel();
    }

    private void OnExitButtonClick()
    {
        SceneHandler.Instance.QuitApp();
    }
}
