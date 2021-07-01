using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Systems : DontDestroyBehaviour<Systems>
{
    #region Static
    public static GameManager gameManager => instance._gameManager;
    public static AudioManager audioManager => instance._audioManager;
    public static SceneManager sceneManager => instance._sceneManager;
    public static InputManager inputManager => instance._inputManager;
    public static DecalSystem decalSystem => instance._decalSystem;
    public static AIManager aiManager => instance._aiManager;
    public static ScoreSystem scoreSystem => instance._scoreSystem;
    public static CheatConsole cheatConsole => instance._cheatConsole;
    #endregion
    #region Properties
    [Header("References")]
    [SerializeField] private GameManager _gameManager;
    [SerializeField] private AudioManager _audioManager;
    [SerializeField] private SceneManager _sceneManager;
    [SerializeField] private InputManager _inputManager;
    [SerializeField] private DecalSystem _decalSystem;
    [SerializeField] private AIManager _aiManager;
    [SerializeField] private ScoreSystem _scoreSystem;
    [SerializeField] private CheatConsole _cheatConsole;
    #endregion

    #region Protected
    protected override void OnAwake()
    {
        _inputManager.Initialize();
    }

    protected override void Destroy()
    {
        aiManager.enemies.Clear();
        decalSystem.ClearDecals();
        //inputManager.Clear();
    }

    #endregion
}
