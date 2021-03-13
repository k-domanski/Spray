using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Player _player;
    [SerializeField] private float _smoothTime;
    [SerializeField] private float _maxSpeed;
    [SerializeField] private Vector3 _cameraOffset;
    private Camera _camera;
    private Vector3 vel = Vector3.zero;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void LateUpdate()
    {
        Vector3 pos = Vector3.SmoothDamp(_camera.transform.position, _player.transform.position + _cameraOffset, ref vel, _smoothTime, _maxSpeed, Time.deltaTime);
        _camera.transform.position = pos;
    }
}
