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
    #endregion

    #region Messages
    void Awake()
    {
        _camera = Camera.main;
        SetupCamera();
    }

    void Update()
    {
        SetupCamera();

        var cameraPos = _camera.transform.position;
        var playerPos = _player.transform.position;
#if UNITY_EDITOR
        Vector3 pos = playerPos + _offset;
#else
        Vector3 pos = Vector3.SmoothDamp(cameraPos, playerPos + _offset, ref vel, _settings.smoothTime, _settings.maxSpeed, Time.deltaTime);
#endif
        _camera.transform.position = pos;
    }
    #endregion

    #region Private Methods
    private void SetupCamera()
    {
        _camera.orthographicSize = _settings.orthographicSize;

        var distance = _settings.distance;

        _offset.x = Mathf.Sin(_settings.azimuth) * distance;
        _offset.z = Mathf.Cos(_settings.azimuth) * distance;
        _offset.y = Mathf.Sin(_settings.elevation) * distance;

        _camera.transform.forward = -_offset.normalized;
    }
    #endregion
}
