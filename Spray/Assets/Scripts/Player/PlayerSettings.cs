using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player Settings", fileName = "Player Settings")]
public class PlayerSettings : ScriptableObject
{
    #region Properties
    [SerializeField, Range(1.0f, 30.0f)] private float _maxSpeed;
    [Header("Acceleration")]
    [SerializeField, Range(0.1f, 50.0f)] private float _acceleration;
    [SerializeField, Range(1.0f, 5.0f)] private float _accelerationBoost;
    [SerializeField, Range(0.0f, 1.0f)] private float _accelerationSpeedFactor;
    [SerializeField, Range(0.0f, 1.0f)] private float _decelerationRate;
    [Tooltip("Maximum rotations per second")]
    [SerializeField, Range(0.0f, 5.0f)] private float _rotationsPerSecond;
    [Header("Shooting on Controller Test")]
    [SerializeField] private bool _autoShoot;
    [SerializeField, Range(0.01f, 1.0f)] private float _controllerAimDeadZone;
    [Header("Multipliers")]
    [SerializeField] private float _speedMultiplier = 1;
    [SerializeField] private float _damageMultiplier = 1;


    public float maxSpeed => _maxSpeed;
    public float acceleration => _acceleration;
    public float accelerationBoost => _accelerationBoost;
    public float accelerationSpeedFactor => _accelerationSpeedFactor;
    public float decelerationRate => _decelerationRate;
    public float maxRotationSpeed => _rotationsPerSecond * 360.0f;
    public bool autoShoot => _autoShoot;
    public float controllerAimDeadZone => _controllerAimDeadZone;

    public float speedMultiplier { get => _speedMultiplier; set => _speedMultiplier += value; }
    public float damageMultiplier { get => _damageMultiplier; set => _damageMultiplier += value; }
    #endregion
}
