using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    private MainControlls _mainControlls;
    public void Initialize()
    {
        _mainControlls = new MainControlls();
        _mainControlls.Enable();
    }

    public void SetCallbacks<T>(T instanceCallbacks)
    {
        if (instanceCallbacks is MainControlls.IPlayerActions playerActions)
            _mainControlls.Player.SetCallbacks(playerActions);
    }

    public void Clear()
    {
        if (_mainControlls != null)
        {
            _mainControlls.Disable();
            _mainControlls.Dispose();
        }
    }
}
