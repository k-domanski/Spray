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

    private void OnDestroy()
    {
        _mainControlls.Disable();
        _mainControlls.Dispose();
    }
}
