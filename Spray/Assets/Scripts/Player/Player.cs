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
    [SerializeField] private PlayerProxy _playerProxy;
    [SerializeField] private PlayerSettings _playerSettings;
    [SerializeField] private TextMeshProUGUI _weaponName;
    [SerializeField] private List<GunController> _guns;
    [SerializeField] private AnimationPlaybackSpeed _animationTiming;
    [SerializeField] private DeathPanel _playerDeathPanel;//TODO: move it to UIManager or somewhere
    public PlayerSettings playerSettings => _playerSettings;
    public Rigidbody _rigidbody { get; private set; }
    public Vector3 velocity => _rigidbody.velocity;
    public GunController mainWeapon { get => _guns[0]; set => _guns[0] = value; }
    public GunController secondaryWeapon { get => _guns[1]; set => _guns[1] = value; }
    public GunController currentWeapon => _currentWeapon;
    public LivingEntity livingEntity { get; private set; }
    #endregion

    #region Private
    private PlayerController _playerController;
    private SimpleShooting _simpleShooting;
    private AudioSource _audio;
    private GunController _currentWeapon;
    private GunController _secCurrentWeapon;
    private Animator _animator;
    private bool _isShooting = false;
    private float _time = 0;
    private int _weaponIndex;
    private float _playerSpeed;
    private bool _isDead = false;
    #endregion

    #region Messages
    private void Awake()
    {
        livingEntity = GetComponent<LivingEntity>();
        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
        _simpleShooting = GetComponent<SimpleShooting>();
        _audio = GetComponent<AudioSource>();
        _animator = GetComponent<Animator>();

        _currentWeapon = mainWeapon;
        _secCurrentWeapon = _guns[2];
        mainWeapon.Equip();
        secondaryWeapon.Unequip();
        _weaponName.text = _currentWeapon.name;

        _playerProxy.Register(this);
    }

    private void Start()
    {
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        Systems.scoreSystem.Reset();
    }

    private void OnEnable()
    {
        livingEntity.onDeath.AddListener(Die);
    }

    private void OnDisable()
    {
        livingEntity.onDeath.RemoveListener(Die);
    }

    private void OnDestroy()
    {
        _playerProxy.Unregister(this);
    }

    private void Update()
    {
        if (_isShooting)
        {
            _time += Time.deltaTime;
            _currentWeapon.Shoot(_playerController.lookDirection, _playerSettings.damageMultiplier, _time);
            _secCurrentWeapon.Shoot(_playerController.lookDirection, _playerSettings.damageMultiplier, _time);
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
        if (direction == Vector3.zero)
        {
            return;
        }

        _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation,
                                                       Quaternion.LookRotation(direction.normalized, Vector3.up),
                                                       deltaTime * _playerSettings.maxRotationSpeed);
    }

    public void Move(Vector3 direction, float deltaTime)
    {
        Vector3 currentVelocity = _rigidbody.velocity;
        Vector3 currentDirection = currentVelocity.normalized;
        Vector3 desiredDirection = direction.normalized;

        // 1 if user input, 0 otherwise
        float throttle = desiredDirection.sqrMagnitude;
        float dot = Vector3.Dot(currentDirection, desiredDirection);
        // Speed ratio, used to ease out the sudden direction change (dot from -1 to 1)
        float speedRatio = currentVelocity.magnitude / _playerSpeed;
        float speedFactor = Mathf.Lerp(1.0f, speedRatio, _playerSettings.accelerationSpeedFactor * throttle);
        // Re-mapping from dot product [-1, 1] range to [0, 1] range and smooth with speed ratio
        float t = (dot * 0.5f + 0.5f) * speedFactor;
        // Applying boost based on 'turn sharpness', speed and throttle
        float accelerationBoost = Mathf.Lerp(1.0f, Mathf.Lerp(_playerSettings.accelerationBoost, 1.0f, t), throttle);
        // If the movement is held or released
        float deceleration = Mathf.Max(_playerSettings.decelerationRate, throttle);

        float acceleration = _playerSettings.acceleration * accelerationBoost * deceleration;

        //TODO: Reduce speed here
        _playerSpeed = (_playerSettings.maxSpeed - GetSpeedReduction()) * _playerSettings.speedMultiplier;

        Vector3 desiredVelocity = Vector3.MoveTowards(currentVelocity, desiredDirection * _playerSpeed, acceleration * deltaTime);
        AdjustAnimation(desiredVelocity.normalized, transform.forward);
        _rigidbody.velocity = desiredVelocity;

        // if (_rigidbody.velocity.magnitude > 0.1f && !_audio.isPlaying)
        //     _audio.Play();
        // else if (_audio.isPlaying && _rigidbody.velocity.magnitude < 0.1f)
        //     _audio.Pause();
    }
    public void Shoot(bool start)
    {
        _isShooting = start;
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
        return 0;

        if (!_isShooting)
            return _currentWeapon.weaponStats.playerBaseSpeedReduction;

        return _currentWeapon.weaponStats.playerSpeedReductionWhileShooting;
    }
    private void AdjustAnimation(Vector3 moveDirection, Vector3 lookDirection)
    {
        moveDirection.y = 0.0f;
        moveDirection.Normalize();
        lookDirection.y = 0.0f;
        lookDirection.Normalize();
        var x = Vector3.Cross(lookDirection, moveDirection).y;
        var y = Vector3.Dot(moveDirection, lookDirection);
        _animator.speed = _animationTiming.GetPlaybackSpeed(_playerSpeed);
        _animator.SetFloat("Horizontal", x);
        _animator.SetFloat("Vertical", y);
    }

    private void Die(LivingEntity entity)
    {
        if (entity == livingEntity && !_isDead)
        {
            _isDead = true;
            Debug.Log("YOU DIED!!!!!!!!!");
            this.Delay(() => Systems.sceneManager.LoadScene(GameScene.Preload), 2.0f);
            _playerDeathPanel.Show();
        }
    }
    #endregion
}