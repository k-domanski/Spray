using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Systems : DontDestroyBehaviour<Systems>
{
    #region Static
    public static GameManager gameManager => instance._gameManager;
    public static AudioManager audioManager => instance._audioManager;
    public static SceneManager sceneManager => instance._sceneManager;
    public static InputManager inputManager=> instance._inputManager;
    #endregion
    #region Properties
    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private InputManager _inputManager;
    #endregion

    #region Protected
    protected override void OnAwake()
    {
        _inputManager.Initialize();
    }
    #endregion
}
