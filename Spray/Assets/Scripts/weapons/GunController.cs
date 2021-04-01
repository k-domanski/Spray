using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private WeaponStats _weaponStats;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private AudioSource _shotAudio;
    [SerializeField] private ParticleSystem _muzzleFlash;
    private bool _canShoot = true;

    private void Awake()
    {
        _shotAudio = GetComponent<AudioSource>();
        _muzzleFlash.gameObject.SetActive(false);
    }

    public void Shoot(Vector3 aimDirection)
    {
        _muzzleFlash.gameObject.SetActive(true);
        if (_canShoot)
        {
            _canShoot = false;
            if (_shotAudio != null)
                _shotAudio.Play();

            _weaponStats.CreateProjectile(_muzzlePoint.position, aimDirection);
            this.Delay(() => SetShooting(), 1f / _weaponStats.fireRate);
        }
    }

    private void SetShooting()
    {
        _canShoot = true;
        _muzzleFlash.gameObject.SetActive(false);
    }
}
