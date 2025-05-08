using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsPanelUI : MonoBehaviour
{
    [SerializeField] Button _closeButton;

    private void Start()
    {
        _closeButton.onClick.AddListener(ClosePanel);

        gameObject.SetActive(false);
    }

    private void ClosePanel()
    {
        CloseCreditsPanel();   
    }

    public void OpenCreditsPanel()
    {
        gameObject.SetActive(true);
    }

    public void CloseCreditsPanel()
    {
        gameObject.SetActive(false);
    }
}
