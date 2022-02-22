using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ProjectileModifier", menuName = "BloodEffects/ProjectileModifier")]
public class WeaponProjectileModifier : BloodEffectBase
{
    [SerializeField] private ProjectileCount _projectileCount;
    private WeaponStats _weaponStats;
    private ProjectileCount _baseWeaponProjectileCount;
    private bool _applied;
    public override void Apply(GameObject gameObject)
    {
        Tick();

        Initialize(gameObject);

        if(!_applied)
        {
            ChangeWeaponProjectileCount();
        }
    }

    private void ChangeWeaponProjectileCount()
    {
        _baseWeaponProjectileCount = _weaponStats.projectileCount;
        _weaponStats.projectileCount = _projectileCount;
        _applied = true;
    }

    private void Initialize(GameObject gameObject)
    {
        _weaponStats = gameObject.GetComponent<Player>().mainWeapon.weaponStats;
    }

    public override void Remove()
    {
        _weaponStats.projectileCount = _baseWeaponProjectileCount;
        _applied = false;
        currentDuration = 0;
    }
}
