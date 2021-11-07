using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageReduction", menuName = "BloodEffects/DamageReduction")]
public class DamageReduction : BloodEffectBase
{
    [SerializeField, Range(0, 1), Tooltip("Amount of damage reduction in %")] 
    private float _amount;
    [SerializeField] private GameObject _vfxPrefab;
    private GameObject _shieldObject;

    private LivingEntity _livingEntity;
    private bool _isShieldSpawned;
    private float _entityBaseDamageMultiplier;
    public override void Apply(GameObject gameObject)
    {
        currentDuration -= Time.deltaTime;

        

        if (_livingEntity == null || _shieldObject == null)
        {
            _livingEntity = gameObject.GetComponent<LivingEntity>();
            _entityBaseDamageMultiplier = _livingEntity.damageMultiplier;
            SpawnShield(gameObject);
        }
        else
        {
            if (!this.IsActive())
                Disable();

            if (!_isShieldSpawned && this.IsActive())
                Enable();
        }
    }

    private void Disable()
    {
        _isShieldSpawned = false;
        _livingEntity.damageMultiplier = _entityBaseDamageMultiplier;
        _shieldObject.SetActive(_isShieldSpawned);
    }

    private void Enable()
    {
        _isShieldSpawned = true;
        _livingEntity.damageMultiplier -= _amount;
        _shieldObject.SetActive(_isShieldSpawned);
    }
    private void SpawnShield(GameObject gameObject)
    {
        _shieldObject = GameObject.Instantiate(_vfxPrefab);
        _shieldObject.transform.SetParent(gameObject.transform, false);
        _livingEntity.damageMultiplier -= _amount;
        _isShieldSpawned = true;
    }
}
