using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatConsole : MonoBehaviour
{
    [SerializeField] private PlayerProxy _playerProxy;
    private bool _playerGodMode = false;
    public void PlayerGodMode(bool enable)
    {
        if (!_playerProxy.IsSet())
        {
            return;
        }

        _playerProxy.Get().livingEntity.canReceiveDamage = !enable;
        _playerGodMode = enable;
    }
}
