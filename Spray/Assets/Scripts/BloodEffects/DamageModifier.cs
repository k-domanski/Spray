using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageModifier", menuName = "BloodEffects/DamageModifier")]
public class DamageModifier : BloodEffectBase
{
    [SerializeField, Range(-1, 1)] private float _amount;
    private Player _player;
    private bool _applied;
    public override void Apply(GameObject gameObject)
    {
        Tick();
        if (_player == null)
        {
            Initialize(gameObject);
        }

        if (!_applied)
        {
            Enable();
        }
    }

    private void Initialize(GameObject gameObject)
    {
        _player = gameObject.GetComponent<Player>();
        Enable();
    }

    private void Enable()
    {
        _player.damageMultiplier = _amount;
        _applied = true;
    }

    public override void Remove()
    {
        _player.damageMultiplier = -_amount;
        _applied = false;
        currentDuration = 0;
    }
}
