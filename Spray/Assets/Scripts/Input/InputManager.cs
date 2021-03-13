using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private MainControlls _mainControlls;
    public static InputManager instance { get; private set; } = null;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
        

        _mainControlls = new MainControlls();
    }

    private void Start()
    {
        _mainControlls.Enable();
    }

    public void SetCallbacks<T>(T instanceCallbacks)
    {
        if (instanceCallbacks is MainControlls.IPlayerActions playerActions)
            _mainControlls.Player.SetCallbacks(playerActions);
    }

    private void OnDestroy()
    {
        _mainControlls.Disable();
        _mainControlls.Dispose();
    }
}
