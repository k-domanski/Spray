using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Settings/Camera Settings", fileName = "Camera Settings")]
public class CameraSettings : ScriptableObject
{
    #region Properties
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _maxSpeed;
    [SerializeField, Range(0.0f, 89.9f)] private float _elevation;
    [SerializeField, Range(-180.0f, 180.0f)] private float _azimuth;
    [SerializeField] private float _azimuthStep;
    [SerializeField, Range(1.0f, 100.0f)] private float _distance;
    [SerializeField, Range(3.0f, 20.0f)] private float _orthographicSize = 5.0f;

    public float smoothTime => _smoothTime;
    public float maxSpeed => _maxSpeed;
    public float elevation => _elevation * Mathf.Deg2Rad;
    public float azimuth => _azimuth * Mathf.Deg2Rad;
    public float azimuthStep => _azimuthStep * Mathf.Deg2Rad;
    public float distance => _distance;
    public float orthographicSize => _orthographicSize;
    #endregion
}
