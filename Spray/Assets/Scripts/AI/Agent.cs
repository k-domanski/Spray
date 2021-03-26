using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(Rigidbody))]
public class Agent : MonoBehaviour
{
    private Rigidbody _rigidbody;
    private Vector3 _destination;
    [SerializeField] private EnemyParams _settings;
    private Vector3 _circlePosition;
    private float _circleDistance = 3f;
    private float _circleRadius = 2f;
    private float _wanderAngle = 10f;
    private float _angleChange = 0.5f;
    private float _predictionStep;
    [SerializeField] private Target _target;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {

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
    private void Seek(Vector3 target)
    {
        Vector3 desiredVelocity = target - transform.position;
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
        _circlePosition = _rigidbody.velocity.normalized;
        _circlePosition *= _circleDistance;

        Vector3 displacement = -Vector3.forward;
        displacement *= _circleRadius;
        SetAngle(ref displacement, _wanderAngle);
        _wanderAngle += Random.Range(-_angleChange, _angleChange);

        Vector3 wanderForce = _circlePosition + displacement;

        Vector3 steering = wanderForce;
        steering = Vector3.ClampMagnitude(steering, _settings.maxSpeed);
        steering = steering / _rigidbody.mass;
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity + steering, _settings.maxSpeed);
    }

    private void Pursuit()
    {
        var position = _target.transform.position;
        Vector3 distance = position - transform.position;
        _predictionStep = distance.magnitude / _settings.maxSpeed;
        Vector3 futurePosition = position + _target.GetComponent<Rigidbody>().velocity * _predictionStep;
        Seek(position);
    }

    private void SetAngle(ref Vector3 vector, float angle)
    {
        var len = vector.magnitude;
        vector.x = Mathf.Cos(angle) * len;
        vector.z = Mathf.Sin(angle) * len;
    }
}