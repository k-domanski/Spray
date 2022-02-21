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
    public float currentHeat => _currentHeat;
    #endregion

    #region Private
    private bool _canShoot = true;
    private bool _overHeated = true;
    private float _cooldownActivationTimer = 0;
    private float _currentHeat = 0;
    private float _angle;
    private int _projectileCount;
    #endregion

    #region Events
    public event System.Action<bool> onWeaponOverheat;
    #endregion

    #region Messages
    private void Awake()
    {
        
        TryGetComponent<LineRenderer>(out _laser);
        if (_shotAudio == null)
            _shotAudio = GetComponent<AudioSource>();
        _muzzleFlash.gameObject.SetActive(false);

        _angle = Mathf.PI / _weaponStats.spreadAngle;
        _projectileCount = (int)_weaponStats.projectileCount;
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

        _cooldownActivationTimer += Time.deltaTime;
        if(_cooldownActivationTimer >= _weaponStats.cooldownActivationTime)
        {
            _currentHeat -= _weaponStats.cooldownSpeed * Time.deltaTime;
            if(_currentHeat <= 0)
            {
                if(_overHeated)
                    onWeaponOverheat?.Invoke(false);
                _overHeated = false;
                _currentHeat = 0;
            }
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
        if (_canShoot & !_overHeated)
        {
            _muzzleFlash.gameObject.SetActive(true);
            _canShoot = false;
            _cooldownActivationTimer = 0f;
            // if (_shotAudio != null)
            //     _shotAudio.Play();

            //TODO: Cache and change to list so it can be changed dynamically
            _projectileCount = (int)weaponStats.projectileCount;
            Vector3[] directions = new Vector3[_projectileCount];
            directions = CalculateProjectileDirection(aimDirection, _projectileCount);

            for (int i = 0; i < _projectileCount; i++)
            {
                var dir = CalculateRecoil(directions[i], time);
                _weaponStats.CreateProjectile(_muzzlePoint.position, dir, playerMultiplier, _placeBulletHoles, _decalChance);
            }
            this.Delay(() => SetShooting(), 1f / _weaponStats.fireRate);

            _currentHeat += _weaponStats.heatStepPerShot;
            if(_currentHeat >= _weaponStats.maxHeatValue)
            {
                if(!_overHeated)
                    onWeaponOverheat?.Invoke(true);
                _overHeated = true;
            }
        }
    }
    #endregion

    #region Private Methods
    private Vector3[] CalculateProjectileDirection(Vector3 aimDirection, int projectileCount)
    {
        Vector3[] directions = new Vector3[projectileCount];

        directions[0] = aimDirection;
        Vector3 direction = new Vector3();
        int angleMultiplier = 1;

        for(int i =1; i < projectileCount; i++)
        {
            float angle = _angle * angleMultiplier;

            if(i%2 == 0)
            {
                direction.x = (aimDirection.x * Mathf.Cos(angle)) - (aimDirection.z * Mathf.Sin(angle));
                direction.z = (aimDirection.x * Mathf.Sin(angle)) + (aimDirection.z * Mathf.Cos(angle));
                angleMultiplier++;
            }
            else
            {
                direction.x = (aimDirection.x * Mathf.Cos(-angle)) - (aimDirection.z * Mathf.Sin(-angle));
                direction.z = (aimDirection.x * Mathf.Sin(-angle)) + (aimDirection.z * Mathf.Cos(-angle));
            }

            directions[i] = direction;
        }

        return directions;
    }

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
