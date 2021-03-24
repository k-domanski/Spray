using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Player Settings", fileName = "Player Settings")]
public class PlayerSettings : ScriptableObject
{
    #region Properties
    [SerializeField, Range(1.0f, 30.0f)] private float _maxSpeed;
    [SerializeField, Range(0.1f, 50.0f)] private float _acceleration;
    [Tooltip("Maximum rotations per second")]
    [SerializeField, Range(0.0f, 5.0f)] private float _rotationsPerSecond;

    public float maxSpeed => _maxSpeed;
    public float acceleration => _acceleration;
    public float maxRotationSpeed => _rotationsPerSecond * 360.0f;
    #endregion
}
