using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioClipPlayer : MonoBehaviour
{
    public static AudioClipPlayer Instance { get; private set; }

    [SerializeField] AudioSource _buttonPressAudioSource;
    [SerializeField] AudioSource _clickAudioSource;
    [SerializeField] AudioSource _placeChefStationAudioSource;
    [SerializeField] AudioSource _putBlockVariableAudioSource;
    [SerializeField] AudioSource _takeAudioSource;
    [SerializeField] AudioSource _placeCookingStationAudioSource;
    [SerializeField] AudioSource _useCuttingStationAudioSource;
    [SerializeField] AudioSource _useChefStationAudioSource;

    public void PlayAudioClip(AudioClips audioClip)
    {
        AudioSource source = GetAudioSource(audioClip);
        if (source == null)
        {
            return;
        }
        source.Play();
    }

    private AudioSource GetAudioSource(AudioClips audioClip)
    {
        return audioClip switch
        {
            AudioClips.ButtonPress => _buttonPressAudioSource,
            AudioClips.Click => _clickAudioSource,
            AudioClips.PlaceChefStation => _placeChefStationAudioSource,
            AudioClips.PutBlockVariable => _putBlockVariableAudioSource,
            AudioClips.Take => _takeAudioSource,
            AudioClips.PlaceCookingStation => _placeCookingStationAudioSource,
            AudioClips.UseCuttingStation => _useCuttingStationAudioSource,
            AudioClips.UseChefStation => _useChefStationAudioSource,
            _ => null,
        };
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    public enum AudioClips
    {
        ButtonPress,
        Click,
        PlaceChefStation,
        PutBlockVariable,
        Take,
        PlaceCookingStation,
        UseCuttingStation,
        UseChefStation
    }
}
