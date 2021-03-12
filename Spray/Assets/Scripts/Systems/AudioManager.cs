using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Properties
    [SerializeField] private AudioSettings _audioSettings;
    [SerializeField] private AudioSource _mainMusic;
    public AudioSettings audioSettings => _audioSettings;
    #endregion

    #region Messages
    void Start()
    {
        _audioSettings.musicVolume = _mainMusic.volume; //TODO: Remove later
        _audioSettings.soundVolume = _audioSettings.soundVolume;
    }
    void OnEnable()
    {
        _audioSettings.onMusicVolumeChanged += UpdateMusicVolume;
    }
    void OnDisable()
    {
        _audioSettings.onMusicVolumeChanged -= UpdateMusicVolume;

    }
    #endregion

    #region Public
    public void SetMusicVolume(float value)
    {
        _audioSettings.musicVolume = value;
    }
    public void SetSoundVolume(float value)
    {
        _audioSettings.soundVolume = value;
    }
    public void MuteMusic()
    {
        _mainMusic.volume = 0.0f;
    }
    public void UnmuteMusic()
    {
        _mainMusic.volume = _audioSettings.musicVolume;
    }
    #endregion

    #region Private Methods
    private void UpdateMusicVolume(float value)
    {
        _mainMusic.volume = value;
    }
    #endregion
}
