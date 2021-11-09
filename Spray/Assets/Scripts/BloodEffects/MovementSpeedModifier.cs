using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedModifier", menuName = "BloodEffects/SpeedModifier")]
public class MovementSpeedModifier : BloodEffectBase
{
    [SerializeField, Range(-1,1)] private float _amount;
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

    private void Enable()
    {
        _player.speedMultiplier = _amount;
        _applied = true;
    }

    private void Initialize(GameObject gameObject)
    {
        _player = gameObject.GetComponent<Player>();
        Enable();
    }

    public override void Remove()
    {
        _player.speedMultiplier = -_amount;
        _applied = false;
        currentDuration = 0;
    }
}
