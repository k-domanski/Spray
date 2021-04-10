using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    #region Properties
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField]private List<GunController> _guns;
    public PlayerSettings playerSettings => _playerSettings;
    public Rigidbody _rigidbody { get; private set; }
    public Vector3 velocity => _rigidbody.velocity;
    public GunController mainWeapon { get => _guns[0]; set => _guns[0] = value; }
    public GunController secondaryWeapon { get => _guns[1]; set => _guns[1] = value; }
    public GunController currentWeapon => _currentWeapon;
    #endregion

    #region Private
    private PlayerController _playerController;
    private SimpleShooting _simpleShooting;
    private AudioSource _audio;
    private GunController _currentWeapon;
    private bool _isShooting = false;
    private float _time = 0;
    private int _weaponIndex;
    #endregion

    #region Messages
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
        _simpleShooting = GetComponent<SimpleShooting>();
        _audio = GetComponent<AudioSource>();

        _currentWeapon = mainWeapon;
        mainWeapon.Equip();
        secondaryWeapon.Unequip();
        _weaponName.text = _currentWeapon.name;
    }

    private void Start()
    {
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    private void Update()
    {
        if (_isShooting)
        {
            _time += Time.deltaTime;
            _currentWeapon.Shoot(_playerController.aimDirection, _time);
        }
        else
        {
            _time = 0;
        }
    }
    #endregion

    #region Public
    public void LookAt(Vector3 direction, float deltaTime)
    {
        _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation,
                                                       Quaternion.LookRotation(direction.normalized, Vector3.up),
                                                       deltaTime * _playerSettings.maxRotationSpeed);
    }

    public void Move(Vector3 direction, float deltaTime)
    {
        var currentVelocity = _rigidbody.velocity;
        var currentDirection = currentVelocity.normalized;
        var desiredDirection = direction.normalized;

        // Re-mapping from dot product [-1, 1] range to [0, 1] range
        var t = (Vector3.Dot(currentDirection, desiredDirection) + 1.0f) / 2.0f;
        // Applying boost based on 'turn sharpness'
        var accelerationBoost = Mathf.Lerp(_playerSettings.accelerationBoost, 1.0f, t);

        var acceleration = _playerSettings.acceleration * accelerationBoost;

        //TODO: Reduce speed here
        var playerSpeed = _playerSettings.maxSpeed - GetSpeedReduction();

        Vector3 desiredVelocity = Vector3.MoveTowards(currentVelocity, direction * playerSpeed, acceleration * deltaTime);

        _rigidbody.velocity = desiredVelocity;

        if (_rigidbody.velocity.magnitude > 0.1f && !_audio.isPlaying)
            _audio.Play();
        else if (_audio.isPlaying && _rigidbody.velocity.magnitude < 0.1f)
            _audio.Pause();
    }
    public void Shoot(bool start)
    {
        _isShooting = start;
        //if(start)
        //{
        //    _simpleShooting.Fire();
        //}
    }
    public void Knockout(Vector3 direction, float force)
    {
        _rigidbody.AddForce(direction * force);
    }
    public void ChangeWeapon()
    {
        _weaponIndex = _weaponIndex == 0 ? 1 : 0;
        // Current Gun unequip
        _currentWeapon.Unequip();

        _currentWeapon = _guns[_weaponIndex];

        // Current Gun equip
        _currentWeapon.Equip();

        _weaponName.text = _currentWeapon.name;
    }
    #endregion

    #region Private Methods
    private float GetSpeedReduction()
    {
        if(!_isShooting)
            return _currentWeapon.weaponStats.playerBaseSpeedReduction;

        return _currentWeapon.weaponStats.playerSpeedReductionWhileShooting;
    }
    #endregion
}