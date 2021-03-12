using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(menuName = "Audio/Audio Settings", fileName = "Audio Settings")]
public class AudioSettings : ScriptableObject
{
    #region Properties
    [SerializeField] private float _musicVolume;
    [SerializeField] private float _soundVolume;

    public float musicVolume
    {
        get => _musicVolume;
        set
        {
            _musicVolume = value;
            onMusicVolumeChanged?.Invoke(value);
        }
    }
    public float soundVolume
    {
        get => _soundVolume;
        set
        {
            _soundVolume = value;
            onSoundVolumeChanged?.Invoke(value);
        }
    }
    #endregion

    #region Events
    public event Action<float> onMusicVolumeChanged;
    public event Action<float> onSoundVolumeChanged;
    #endregion
}
