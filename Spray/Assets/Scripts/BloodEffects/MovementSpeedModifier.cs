using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SpeedModifier", menuName = "BloodEffects/SpeedModifier")]
public class MovementSpeedModifier : BloodEffectBase
{
    [SerializeField, Range(-1,1)] private float _amount;
    public EJetSpeed jetSpeed = EJetSpeed.Normal;
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
        //_player.playerSettings.speedMultiplier = _amount;
        _applied = true;
        _player.GetComponent<PlayerVisuals>().ChangeJetColor(jetSpeed);
    }

    private void Initialize(GameObject gameObject)
    {
        _player = gameObject.GetComponent<Player>();
        Enable();
    }

    public override void Remove()
    {
        //_player.playerSettings.speedMultiplier = -_amount;
        _applied = false;
        currentDuration = 0;
        _player.GetComponent<PlayerVisuals>().ChangeJetColor(EJetSpeed.Normal);
    }
}
