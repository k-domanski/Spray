using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour
{

    #region Properties
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    public float maxHealth
    {
        get => _maxHealth;
        set => _maxHealth = value;
    }
    public float currentHealth
    {
        get => _currentHealth;

        set
        {
            value = Mathf.Clamp(value, 0.0f, _maxHealth);
            if (_currentHealth == value)
                return;
            var oldHealth = _currentHealth;
            _currentHealth = value;

            onHealthChanged?.Invoke(oldHealth, value);
        }
    }
    public bool canReceiveDamage = true;

    public float damageMultiplier
    {
        get => _damageMultiplier;

        set => _damageMultiplier = value;
    }
    #endregion

    #region Private
    private float _damageMultiplier = 1.0f;
    #endregion

    #region Events
    public UnityEvent<float, Vector3> onDamageTaken;
    public UnityEvent<LivingEntity> onDeath;
    public UnityEvent<float, float> onHealthChanged;
    #endregion

    #region Public
    public void DealDamage(float damage, Vector3 impactDirection)
    {
        if (!canReceiveDamage)
        {
            return;
        }

        if (currentHealth > 0)
        {
            currentHealth -= CalculateDamage(damage);
            onDamageTaken?.Invoke(damage, impactDirection);
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }
    #endregion

    #region Private Methods
    private void Die()
    {
        onDeath?.Invoke(this);
    }

    private float CalculateDamage(float damage)
    {
        return _damageMultiplier * damage;
    }
    #endregion
}
