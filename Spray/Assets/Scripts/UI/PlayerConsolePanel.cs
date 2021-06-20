using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerConsolePanel : MonoBehaviour
{
    [SerializeField] private Toggle _godmode;

    void OnEnable()
    {
        _godmode.onValueChanged.AddListener(OnGodMode);
    }

    void OnDisable()
    {
        _godmode.onValueChanged.RemoveListener(OnGodMode);
    }


    public void OnGodMode(bool value)
    {
        Systems.cheatConsole.PlayerGodMode(value);
    }
}
