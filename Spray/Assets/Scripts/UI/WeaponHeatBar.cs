using UnityEngine;

[RequireComponent(typeof(ProgressBar))]
public class WeaponHeatBar : MonoBehaviour
{
    #region Private
    private ProgressBar _progressBar;
    [SerializeField] private PlayerProxy _player;
    #endregion

    #region Messages
    private void Awake()
    {
        _progressBar = GetComponent<ProgressBar>();
    }

    private void Update()
    {
        _progressBar.value = _player.Get().currentWeapon.currentHeat / _player.Get().currentWeapon.weaponStats.maxHeatValue;
    }
    #endregion
}