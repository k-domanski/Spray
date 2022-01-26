using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegeneration", menuName = "BloodEffects/HealthRegeneration")]
public class HealthRegenerationEffect : BloodEffectBase
{
    [SerializeField] private float _amount;
    private LivingEntity _livingEntity;
    private PlayerVisuals _playerVisuals;
    private bool _applied = false;
    public override void Apply(GameObject gameObject)
    {
        Tick();

        if (_livingEntity == null)
        {
            _livingEntity = gameObject.GetComponent<LivingEntity>();
        }

        if (_playerVisuals == null)
        {
            _playerVisuals = gameObject.GetComponent<PlayerVisuals>();
        }
        RegenerateHealth(_livingEntity);

        if (!_applied)
        {
            _playerVisuals.ShowHealingEffect(true);
        }
    }

    public override void Remove()
    {
        _playerVisuals.ShowHealingEffect(false);
        currentDuration = 0;
    }

    private void RegenerateHealth(LivingEntity livingEntity)
    {
        if (livingEntity.currentHealth <= livingEntity.maxHealth)
            livingEntity.currentHealth += _amount * Time.deltaTime;
    }
}
