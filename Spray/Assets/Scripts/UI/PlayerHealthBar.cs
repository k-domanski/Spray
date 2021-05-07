using UnityEngine;

[RequireComponent(typeof(ProgressBar))]
public class PlayerHealthBar : MonoBehaviour
{
    #region Private
    private ProgressBar _progressBar;
    private Player _player;
    #endregion

    #region Messages
    void Awake()
    {
        _progressBar = GetComponent<ProgressBar>();
        _player = FindObjectOfType<Player>();
        //_player = GetComponent<Player>();
    }
    private void Start()
    {
        _player.livingEntity.onHealthChanged.AddListener(HealthChangedHandler);
        HealthChangedHandler(0, _player.livingEntity.currentHealth);
    }
    #endregion

    private void OnDestroy()
    {
        _player.livingEntity.onHealthChanged.RemoveListener(HealthChangedHandler);
    }

    #region Private Methods
    private void HealthChangedHandler(float oldHealth, float newHealth)
    {
        _progressBar.value = newHealth / _player.livingEntity.maxHealth;
    }
    #endregion
}
