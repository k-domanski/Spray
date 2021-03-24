using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _destination;
    [SerializeField] private EnemyParams _settings;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Seek();
    }

    public void SetDestination(Vector3 destination)
    {
        _destination = destination;
    }

    private void Seek()
    {
        Vector3 desiredVelocity = _destination - transform.position;
        float distance = desiredVelocity.magnitude;
        
        if (distance < _settings.stoppingDistance)
            desiredVelocity = desiredVelocity.normalized * _settings.maxSpeed * (distance / _settings.stoppingDistance);
        else
            desiredVelocity = desiredVelocity.normalized * _settings.maxSpeed;

        Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - _rigidbody.velocity, _settings.maxSpeed);
        steering = steering / _rigidbody.mass;

        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity + steering, _settings.maxSpeed);
    }

    private void Wander()
    {

    }
}