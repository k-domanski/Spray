using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraController : MonoBehaviour
{
    #region Properties
    [SerializeField] private CameraSettings _settings;
    [SerializeField] private Player _player;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _maxSpeed;
    #endregion

    #region Private
    private Camera _camera;
    private Vector3 vel = Vector3.zero;
    private Vector3 _offset;
    private Vector3 _cameraPivot = Vector3.zero;
    private float _azimuthOffset = 0.0f;
    private float _azimuthOffsetTarget = 0.0f;
    #endregion

    #region Messages
    void Awake()
    {
        _camera = Camera.main;
        _cameraPivot = _player.transform.position;
        SetupCamera();
    }

    void Update()
    {
        if (!_player.isAlive)
        {
            return;
        }

        SetupCamera();

        _azimuthOffset = Mathf.MoveTowards(_azimuthOffset, _azimuthOffsetTarget, _settings.rotationSpeed * Time.deltaTime);

        var cameraPos = _camera.transform.position;
        var playerPos = _player.transform.position;

        if (Application.isPlaying)
        {
            // Smooth out only the camera translation
            _cameraPivot = Vector3.SmoothDamp(_cameraPivot, playerPos, ref vel, _settings.smoothTime, _settings.maxSpeed, Time.deltaTime);
        }
        else
        {
            _cameraPivot = playerPos;
        }
        // Camera rotation and translation by rotation (_offset) leave unaffected
        _camera.transform.position = _cameraPivot + _offset;
    }
    #endregion

    #region Public
    public void RotateCamera(float direction)
    {
        _azimuthOffsetTarget = _azimuthOffsetTarget + _settings.azimuthStep * direction;
    }
    #endregion

    #region Private Methods
    private void SetupCamera()
    {
        _camera.orthographicSize = _settings.orthographicSize;

        var distance = _settings.distance;

        _offset.x = Mathf.Sin(_settings.azimuth + _azimuthOffset) * distance;
        _offset.z = Mathf.Cos(_settings.azimuth + _azimuthOffset) * distance;
        _offset.y = Mathf.Sin(_settings.elevation) * distance;

        _camera.transform.forward = -_offset.normalized;
    }
    #endregion
}
