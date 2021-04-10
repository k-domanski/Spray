using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    #region Properties
    [SerializeField] private WeaponStats _weaponStats;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private AudioSource _shotAudio;
    [SerializeField] private ParticleSystem _muzzleFlash;
    #endregion

    #region Private
    private bool _canShoot = true;
    #endregion

    #region Messages
    private void Awake()
    {
        if (_shotAudio == null)
            _shotAudio = GetComponent<AudioSource>();
        _muzzleFlash.gameObject.SetActive(false);
    }
    #endregion

    #region Public
    public void Shoot(Vector3 aimDirection, float time = 0f)
    {
        if (_canShoot)
        {
            _muzzleFlash.gameObject.SetActive(true);
            _canShoot = false;
            if (_shotAudio != null)
                _shotAudio.Play();

            var dir = CalculateRecoil(aimDirection, time);
            _weaponStats.CreateProjectile(_muzzlePoint.position, dir);
            this.Delay(() => SetShooting(), 1f / _weaponStats.fireRate);
        }
    }
    #endregion

    #region Private Methods
    private Vector3 CalculateRecoil(Vector3 aimDirection, float time)
    {
        if (!_weaponStats.hasRecoil)
            return aimDirection;

        time = Mathf.Clamp(time, 0, 5f);
        float width = UnityEngine.Random.Range(-time, time) / _weaponStats.accuracy;
        float height = UnityEngine.Random.Range(-time, time) / _weaponStats.accuracy;

        Vector3 newDirection = Vector3.zero;
        if (Mathf.Abs(gameObject.transform.forward.x) > 0.7f)
            newDirection = aimDirection + new Vector3(width, height, 0);
        if (Mathf.Abs(gameObject.transform.forward.z) > 0.7f)
            newDirection = aimDirection + new Vector3(0, height, width);

        return newDirection;

    }
    private void SetShooting()
    {
        _canShoot = true;
        _muzzleFlash.gameObject.SetActive(false);
    }
    #endregion
}
