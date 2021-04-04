using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

[RequireComponent(typeof(PlayerController))]
[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [SerializeField] private PlayerSettings _playerSettings;
    // [Header("Player movement")]
    // [SerializeField] private float _maxRotationSpeed;
    // [SerializeField] private float _maxMovementSpeed;
    // [SerializeField] private float _acceleration;

    public PlayerSettings playerSettings => _playerSettings;

    public Rigidbody _rigidbody { get; private set; }
    public Vector3 velocity => _rigidbody.velocity;
    private PlayerController _playerController;
    private SimpleShooting _simpleShooting;
    private AudioSource _audio;
    private GunController _gunController;
    private bool _isShooting = false;
    private float _time = 0;
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
        _simpleShooting = GetComponent<SimpleShooting>();
        _audio = GetComponent<AudioSource>();
        _gunController = GetComponentInChildren<GunController>();
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
            _gunController.Shoot(_playerController.aimDirection, _time);
        }
        else
        {
            _time = 0;
        }
    }

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

        Vector3 desiredVelocity = Vector3.MoveTowards(currentVelocity, direction * _playerSettings.maxSpeed, acceleration * deltaTime);


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
}