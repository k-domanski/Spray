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
    [SerializeField] private bool _placeBulletHoles = true;
    [SerializeField] private float _decalChance = 1.0f;
    [SerializeField] private bool _laserVisible = true;
    private LineRenderer _laser;

    public WeaponStats weaponStats => _weaponStats;
    #endregion

    #region Private
    private bool _canShoot = true;
    #endregion

    #region Messages
    private void Awake()
    {
        
        TryGetComponent<LineRenderer>(out _laser);
        if (_shotAudio == null)
            _shotAudio = GetComponent<AudioSource>();
        _muzzleFlash.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (_laser && _laserVisible)
        {
            _laser.SetPosition(0, _muzzlePoint.position);
            RaycastHit hitInfo;
            Vector3 direction = (_muzzlePoint.position - transform.position).normalized;
            direction.y = 0;
            if (Physics.Raycast(_muzzlePoint.position, direction, out hitInfo))
            {
                if (hitInfo.collider)
                {
                    _laser.SetPosition(1, hitInfo.point);
                    _laser.material.color = hitInfo.transform.TryGetComponent<Enemy>(out var enemy) ? Color.red : Color.green;
                }
            }
        }
        else if (!_laserVisible)
        {
            _laser.SetPosition(0, _muzzlePoint.position);
            _laser.SetPosition(1, _muzzlePoint.position);
        }
    }
    #endregion

    #region Public
    public void Equip()
    {
        gameObject.SetActive(true);
        SetShooting();
    }
    public void Unequip()
    {
        _muzzleFlash.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
    public void Shoot(Vector3 aimDirection, float playerMultiplier, float time = 0f)
    {
        if (_canShoot)
        {
            _muzzleFlash.gameObject.SetActive(true);
            _canShoot = false;
            // if (_shotAudio != null)
            //     _shotAudio.Play();

            var dir = CalculateRecoil(aimDirection, time);
            _weaponStats.CreateProjectile(_muzzlePoint.position, dir, playerMultiplier, _placeBulletHoles, _decalChance);
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
