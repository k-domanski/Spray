using UnityEngine;
using UnityEngine.Events;

public class LivingEntity : MonoBehaviour
{

    #region Properties
    [SerializeField] private float _currentHealth;
    [SerializeField] private float _maxHealth;
    public float maxHealth { get { return _maxHealth; } }
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
    #endregion

    #region Events
    public UnityEvent<LivingEntity> onDamageTaken;
    public UnityEvent<LivingEntity> onDeath;
    public UnityEvent<float, float> onHealthChanged;
    #endregion

    #region Public
    public void DealDamage(float damage)
    {
        if (currentHealth > 0)
        {
            currentHealth -= damage;
            onDamageTaken?.Invoke(this);
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
    #endregion
}
