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
           
           if (_muzzleFlash.isPlaying)
            {
                Debug.Log("Flash");
                //_muzzleFlash.Stop();
            }
            _muzzleFlash.Emit(new ParticleSystem.EmitParams(), 10);
            _muzzleFlash.gameObject.SetActive(true);
            Debug.Log(_muzzleFlash.isEmitting);
            _muzzleFlash.gameObject.SetActive(false);
            
            _weaponStats.CreateProjectile(_muzzlePoint.position, aimDirection);
            this.Delay(() => { _canShoot = true; }, 1f / _weaponStats.fireRate);
        }
        
    }
}
