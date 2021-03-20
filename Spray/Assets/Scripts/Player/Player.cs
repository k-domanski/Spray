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
    [Header("Player movement")]
    [SerializeField] private float _maxRotationSpeed;
    [SerializeField] private float _maxMovementSpeed;
    [SerializeField] private float _acceleration;

    public Rigidbody _rigidbody { get; private set; }
    private PlayerController _playerController;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _playerController = GetComponent<PlayerController>();
    }

    private void Start()
    {
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
    }

    public void LookAt(Vector3 direction, float deltaTime)
    {
        //float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);

        //float dir = angle > 0f ? 1f : angle < 0f ? -1f : 0f;

        ////Quaternion rotation = Quaternion.Euler(Vector3.up * _maxRotationSpeed * deltaTime * dir);

        //float speed = _maxRotationSpeed * deltaTime;

        //Quaternion rotation = Quaternion.Euler(Vector3.up * (Mathf.Abs(angle) < speed ? angle : (speed * dir)));

        //_rigidbody.MoveRotation(_rigidbody.rotation * rotation);

        _rigidbody.rotation = Quaternion.RotateTowards(_rigidbody.rotation, Quaternion.LookRotation(direction.normalized, Vector3.up), deltaTime * _maxRotationSpeed);
    }

    public void Move(Vector3 direction, float deltaTime)
    {
        //Vector3 speed = Vector3.Lerp(_rigidbody.velocity, direction * _maxMovementSpeed, _smoothTimeSpeed * deltaTime);
        Vector3 speed = Vector3.MoveTowards(_rigidbody.velocity, direction * _maxMovementSpeed, _acceleration * deltaTime);

        _rigidbody.velocity = speed;
    }

    public void Shoot(bool start)
    {

    }

    public void Knockout(Vector3 direction, float force)
    {
        _rigidbody.AddForce(direction * force);
    }
}