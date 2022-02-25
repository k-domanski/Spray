using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using TMPro;
using UnityEngine.Timeline;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = System.Numerics.Vector2;
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
    [SerializeField] private Transform _modelOffsetTransform;
    public PlayerSettings playerSettings => _playerSettings;
    public Rigidbody _rigidbody { get; private set; }
    public Vector3 velocity => _rigidbody.velocity;
    public GunController mainWeapon { get => _guns[0]; set => _guns[0] = value; }
    public GunController secondaryWeapon { get => _guns[1]; set => _guns[1] = value; }
    public GunController currentWeapon => _currentWeapon;
    public LivingEntity livingEntity { get; private set; }

    public float maxColorLevel = 100.0f;
    public float colorIncreaseStep = 5.0f;
    public Dictionary<ColorType, float> colorLevels { get; private set; } = new Dictionary<ColorType, float>();
    #endregion

    #region Private
    private PlayerController _playerController;
    private SimpleShooting _simpleShooting;
    private AudioSource _audio;
    private GunController _currentWeapon;
    private GunController _secCurrentWeapon;
    private Animator _animator;
    private ColorSampler _colorSampler;
    private bool _isShooting = false;
    private float _time = 0;
    private int _weaponIndex;
    private float _playerSpeed;
    private bool _isDead = false;

    private float _throttle = 0.0f;
    private float _accelerationMultiplier = 0.0f;
    private Vector3 _desiredMoveDirection = Vector3.zero;
    private Quaternion _tiltRotation = Quaternion.identity;
    private Quaternion _lookRotation = Quaternion.identity;

    private float _tiltOffsetMultiplier = 0.0f;
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
        _colorSampler = GetComponent<ColorSampler>();

        _currentWeapon = mainWeapon;
        _secCurrentWeapon = _guns[2];
        mainWeapon.Equip();
        secondaryWeapon.Unequip();
        _weaponName.text = _currentWeapon.name;

        _playerProxy.Register(this);

        colorLevels.Add(ColorType.Red, 0);
        colorLevels.Add(ColorType.Green, 0);
        colorLevels.Add(ColorType.Blue, 0);
        colorLevels.Add(ColorType.Yellow, 0);
    }

    private void Start()
    {
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        Systems.scoreSystem.Reset();
    }

    private void OnEnable()
    {
        livingEntity.onDeath.AddListener(Die);
        _guns[0].onWeaponOverheat += ShowRightGunOverheat;
        _guns[0].onWeaponOverheat += ShowLeftGunOverheat;
        _colorSampler.onColorSampled += AddToColorLevel;
    }

    private void OnDisable()
    {
        livingEntity.onDeath.RemoveListener(Die);
        _guns[0].onWeaponOverheat -= ShowRightGunOverheat;
        _guns[0].onWeaponOverheat -= ShowLeftGunOverheat;
        _colorSampler.onColorSampled -= AddToColorLevel;
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
            Vector3 shootDirection = _playerController.fireDirection;
            _currentWeapon.Shoot(shootDirection, _playerSettings.damageMultiplier, _time);
            _secCurrentWeapon.Shoot(shootDirection, _playerSettings.damageMultiplier, _time);
        }
        else
        {
            _time = 0;
        }

        ApplyModelOffset(Time.deltaTime);
    }
    #endregion

    #region Public
    public void LookAt(Vector3 direction, float deltaTime)
    {
        if (direction == Vector3.zero)
        {
            return;
        }

        Vector3 axis = Vector3.Cross(Vector3.up, _desiredMoveDirection);
        float desiredAngle = _playerSettings.tiltAngle * Mathf.Max(0, _throttle);
        Quaternion tiltDesiredRotation = Quaternion.AngleAxis(desiredAngle, axis);
        _tiltRotation = Quaternion.RotateTowards(_tiltRotation, tiltDesiredRotation, deltaTime * _accelerationMultiplier * _playerSettings.tiltRotationSpeed);

        Quaternion lookDesiredRotation = Quaternion.LookRotation(direction.normalized, Vector3.up);
        _lookRotation = Quaternion.RotateTowards(_lookRotation, lookDesiredRotation, deltaTime * _playerSettings.maxRotationSpeed);

        _rigidbody.rotation = _tiltRotation * _lookRotation;
    }

    public void Move(Vector3 direction, float deltaTime)
    {
        Vector3 currentVelocity = _rigidbody.velocity;
        Vector3 currentDirection = currentVelocity.normalized;
        Vector3 desiredDirection = direction.normalized;
        _desiredMoveDirection = desiredDirection;

        // 1 if user input, 0 otherwise
        _throttle = desiredDirection.sqrMagnitude;
        float dot = Vector3.Dot(currentDirection, desiredDirection);
        // Speed ratio, used to ease out the sudden direction change (dot from -1 to 1)
        float speedRatio = currentVelocity.magnitude / _playerSpeed;
        float speedFactor = Mathf.Lerp(1.0f, speedRatio, _playerSettings.accelerationSpeedFactor * _throttle);
        // Re-mapping from dot product [-1, 1] range to [0, 1] range and smooth with speed ratio
        float t = (dot * 0.5f + 0.5f) * speedFactor;
        // Applying boost based on 'turn sharpness', speed and throttle
        float accelerationBoost = Mathf.Lerp(1.0f, Mathf.Lerp(_playerSettings.accelerationBoost, 1.0f, t), _throttle);
        // If the movement is held or released
        float deceleration = Mathf.Max(_playerSettings.decelerationRate, _throttle);

        _accelerationMultiplier = accelerationBoost * deceleration;
        float acceleration = _playerSettings.acceleration * _accelerationMultiplier;

        //TODO: Reduce speed here
        _playerSpeed = (_playerSettings.maxSpeed - GetSpeedReduction()) * _playerSettings.speedMultiplier;

        Vector3 desiredVelocity = Vector3.MoveTowards(currentVelocity, desiredDirection * _playerSpeed, acceleration * deltaTime);
        AdjustAnimation(desiredVelocity.normalized, transform.forward);
        _rigidbody.velocity = desiredVelocity;
    }

    public void ApplyModelOffset(float deltaTime)
    {
        Vector3 positionOffset = _modelOffsetTransform.localPosition;
        Quaternion rotationOffset = _modelOffsetTransform.localRotation;

        Vector3 sinHeight = Vector3.up * 0.25f * Mathf.Sin(Time.time * Mathf.PI * 0.25f);

        _tiltOffsetMultiplier = Mathf.MoveTowards(_tiltOffsetMultiplier, 1.0f - _throttle, deltaTime * 1.0f);
        float pitch = _playerSettings.tiltOffsetAngle * _tiltOffsetMultiplier;
        float roll = _playerSettings.tiltOffsetAngle * _tiltOffsetMultiplier;
        Quaternion tiltOffset =
            Quaternion.Euler(Mathf.Sin(Time.time * _playerSettings.tiltOffsetPitchFrequency) * pitch,
                            0,
                            Mathf.Cos(Time.time * _playerSettings.tiltOffsetRollFrequency) * roll);

        Vector3 newPositionOffset = sinHeight;
        Quaternion newRotationOffset = tiltOffset;
        _modelOffsetTransform.localPosition = newPositionOffset;
        _modelOffsetTransform.localRotation = newRotationOffset;
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

    public void AddToColorLevel(ColorType color)
    {
        if (!colorLevels.ContainsKey(color))
        {
            return;
        }

        float currentLevel = colorLevels[color];
        float step = Mathf.Min(maxColorLevel - currentLevel, colorIncreaseStep);
        colorLevels[color] += step;
        //print($"Added: {step} to {color} - current {colorLevels[color]}");
        //TODO: event on new color level or read in update
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
    private void ShowLeftGunOverheat(bool overheated)
    {
        GetComponent<PlayerVisuals>().Overheat(1, overheated);
    }
    private void ShowRightGunOverheat(bool overheated)
    {
        GetComponent<PlayerVisuals>().Overheat(0, overheated);
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