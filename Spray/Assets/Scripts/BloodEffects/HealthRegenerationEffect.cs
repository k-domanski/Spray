using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HealthRegeneration", menuName = "BloodEffects/HealthRegeneration")]
public class HealthRegenerationEffect : BloodEffectBase
{
    [SerializeField] private float _amount;
    private LivingEntity _livingEntity;
    public override void Apply(GameObject gameObject)
    {
        Tick();

        if (_livingEntity == null)
        {
            _livingEntity = gameObject.GetComponent<LivingEntity>();
            RegenerateHealth(_livingEntity);
        }
        else
        {
            RegenerateHealth(_livingEntity);
        }
    }

    public override void Remove()
    {
        currentDuration = 0;
    }

    private void RegenerateHealth(LivingEntity livingEntity)
    {
        if(livingEntity.currentHealth <= livingEntity.maxHealth)
            livingEntity.currentHealth += _amount * Time.deltaTime;
    }
}