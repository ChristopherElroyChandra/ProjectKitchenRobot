using System;
using System.Data.Common;
using UnityEngine;
using UnityEngine.Audio;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance { get; private set; }

    private const string kMasterKey = "masterVol";
    private const string kSfxKey = "sfxVol";
    private const string kBgmKey = "bgmVol";

    [SerializeField] private AudioMixer mixer;

    void Awake()
    {
        if(Instance == null)
        {
            Instance = this;

            InitializeMixer();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeMixer()
    {
        mixer.SetFloat(kMasterKey, GetDb(PlayerPrefs.GetFloat(kMasterKey, 1f)));
        mixer.SetFloat(kSfxKey, GetDb(PlayerPrefs.GetFloat(kSfxKey, .5f)));
        mixer.SetFloat(kBgmKey, GetDb(PlayerPrefs.GetFloat(kBgmKey, .5f)));
    }

    public void SetVolume(AudioSliderUI.AudioSettingType type, float value)
    {
        var key = GetKey(type);

        PlayerPrefs.SetFloat(key, value);
        PlayerPrefs.Save();
        mixer.SetFloat(key, GetDb(value));
    }

    public float GetVolume(AudioSliderUI.AudioSettingType type)
    {
        var key = GetKey(type);

        mixer.GetFloat(key, out float value);
        return GetFloatVolume(value);
    }

    private string GetKey(AudioSliderUI.AudioSettingType type)
    {
        return type switch
        {
            AudioSliderUI.AudioSettingType.SFXVolume => kSfxKey,
            AudioSliderUI.AudioSettingType.MusicVolume => kBgmKey,
            _ => kMasterKey,
        };
    }

    private float GetDb(float value)
    {
        return 20.0f * Mathf.Log10(value);
    }

    private float GetFloatVolume(float value)
    {
        return Mathf.Pow(10.0f, value / 20.0f);
    }
}
