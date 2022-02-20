using UnityEngine;

[RequireComponent(typeof(ProgressBar))]
public class PlayerHealthBar : MonoBehaviour
{
    #region Private
    private ProgressBar _progressBar;
    [SerializeField] private PlayerProxy _player;
    #endregion

    #region Messages
    void Awake()
    {
        _progressBar = GetComponent<ProgressBar>();
        //_player = FindObjectOfType<Player>();
        //_player = GetComponent<Player>();
    }
    private void Start()
    {
        _player.Get().livingEntity.onHealthChanged.AddListener(HealthChangedHandler);
        HealthChangedHandler(0, _player.Get().livingEntity.currentHealth);
    }
    #endregion

    //private void OnDisable()
    //{
    //    _player.Get().livingEntity.onHealthChanged.RemoveListener(HealthChangedHandler);
    //}

    #region Private Methods
    private void HealthChangedHandler(float oldHealth, float newHealth)
    {
        _progressBar.value = newHealth / _player.Get().livingEntity.maxHealth;
    }
    #endregion
}
