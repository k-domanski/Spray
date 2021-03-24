using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{
    [SerializeField] private WeaponStats _weaponStats;
    [SerializeField] private Transform _muzzlePoint;
    [SerializeField] private AudioSource _shotAudio;
    private bool _canShoot = true;

    private void Start()
    {
        _shotAudio = GetComponent<AudioSource>();
    }

    public void Shoot(Vector3 aimDirection)
    {
        if (_canShoot)
        {
            _canShoot = false;
            _shotAudio.Play();
            _weaponStats.CreateProjectile(_muzzlePoint.position, aimDirection);
            this.Delay(() => { _canShoot = true; }, 1f / _weaponStats.fireRate);
        }
        
    }
}
