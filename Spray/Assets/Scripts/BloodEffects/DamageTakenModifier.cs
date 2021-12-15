using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageTakenModifier", menuName = "BloodEffects/DamageTakenModifier")]
public class DamageTakenModifier : BloodEffectBase
{
    [SerializeField, Range(-1, 1), Tooltip("Amount of damage taken multiplier")]
    private float _amount;
    [SerializeField] private GameObject _vfxPrefab;
    public bool isReducedDamage = false;
    private GameObject _vfx;
    private LivingEntity _livingEntity;
    private bool _applied;
    private PlayerVisuals playerVisuals;

    public override void Apply(GameObject gameObject)
    {
        Tick();
        Initialize(gameObject);

        if (!_applied)
        {
            Enable();
        }
    }

    public override void Remove()
    {
        _livingEntity.damageTakenMultiplier = -_amount;
        _applied = false;
        if (isReducedDamage)
        {
            playerVisuals.ShowShield(false);
        }

        currentDuration = 0;
    }

    private void Initialize(GameObject gameObject)
    {
        _livingEntity = gameObject.GetComponent<LivingEntity>();
        playerVisuals = gameObject.GetComponent<PlayerVisuals>();
    }

    private void Enable()
    {
        _livingEntity.damageTakenMultiplier = _amount;
        _applied = true;
        if (isReducedDamage)
        {
            playerVisuals.ShowShield(true);
        }
    }

    private void SpawnVfx(GameObject gameObject)
    {
        _vfx = Instantiate(_vfxPrefab);
        _vfx.transform.SetParent(gameObject.transform, false);
    }
}
