using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AudioSliderUI : MonoBehaviour
{

    [SerializeField] AudioSettingType type;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI titleText;
    [SerializeField] TextMeshProUGUI volumeText;

    private void Awake()
    {
        slider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void Start()
    {
        titleText.text = GetVolumeTitle();

        slider.value = AudioController.Instance.GetVolume(type);
        OnSliderValueChanged(AudioController.Instance.GetVolume(type));
    }

    private void OnSliderValueChanged(float arg0)
    {
        volumeText.text = Mathf.Round(arg0 * 100).ToString();
        AudioController.Instance.SetVolume(type, arg0);
    }

    public enum AudioSettingType
    {
        MasterVolume,
        MusicVolume,
        SFXVolume
    }

    private string GetVolumeTitle()
    {
        return type switch
        {
            AudioSettingType.SFXVolume => "Sound Effects Volume",
            AudioSettingType.MusicVolume => "Music Volume",
            _ => "Master Volume",
        };
    }

}