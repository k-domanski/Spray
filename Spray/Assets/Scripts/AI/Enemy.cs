using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(StateController))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public EnemyParams settings;
    public GunController gunController { get; private set; }
    public StateController stateController { get; private set; }
    private Rigidbody _rigidbody;
    public Enemy leader { get; set; } = null;
    public Player target { get; set; } = null;
    public Vector3 velocity { get; set; } = Vector3.zero;
    public float speed => settings.maxSpeed - _speedReduction;

    private float _speedReduction = 1.0f;
    private Coroutine _slowCoroutine;

    private void Awake()
    {
        stateController = GetComponent<StateController>();
        _rigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
    }

    private void Start()
    {
        _rigidbody.mass = settings.mass;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        target = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = velocity * _speedReduction;
    }

    public void Knockback(Vector3 direction, float power)
    {
        _rigidbody.AddForce(power * -velocity.normalized, ForceMode.Impulse);
        if (_slowCoroutine == null)
        {
            _slowCoroutine = StartCoroutine(SlowdownCoroutine());
        }
        else
        {
             StopCoroutine(SlowdownCoroutine());
            _slowCoroutine = null;
        }
    }

    private IEnumerator SlowdownCoroutine()
    {
        _speedReduction = 0.1f;
        while (_speedReduction < 1)
        {
            yield return null;
            _speedReduction += settings.speedRecovery * Time.deltaTime;
            
        }
        _speedReduction = 1.0f;
        _slowCoroutine = null;
    }

    public void Die()
    {
        Systems.aiManager.enemies.Remove(this);
        gameObject.SetActive(false);
    }
}