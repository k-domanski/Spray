using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(StateController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LivingEntity))]
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

    private LivingEntity _livingEntity;

    private float _speedReduction = 1.0f;
    private Coroutine _slowCoroutine;

    private BoxCollider _attackRangeChecker;
    public bool targetInMeleeRange { get; private set; }
    private bool _canAttack;
    // public bool canAttack
    // {
    //     get => _canAttack;
    //     set => _canAttack = value;
    // }
    public bool canAttack => !_meleeAnimation.isAnimating;
    private MeleeAnimation _meleeAnimation;

    private void Awake()
    {
        stateController = GetComponent<StateController>();
        _rigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        _livingEntity = GetComponent<LivingEntity>();
        _meleeAnimation = GetComponent<MeleeAnimation>();
        targetInMeleeRange = false;
        if (settings.attackTriggerSize == Vector2.zero) return;

        _attackRangeChecker = gameObject.AddComponent<BoxCollider>();

        _attackRangeChecker.center = settings.attackTriggerPosition * Vector3.forward;
        _attackRangeChecker.size = new Vector3(settings.attackTriggerSize.x, 1f, settings.attackTriggerSize.y);
        _attackRangeChecker.isTrigger = true;

        // /* Hands */
        // _qLBase = _leftHand.transform.rotation;
        // _qRBase = _rightHand.transform.rotation;
        // _qLInter = Quaternion.identity;
        // _qRInter = Quaternion.identity;

    }

    private void Start()
    {
        _livingEntity.maxHealth = settings.maxHealth;
        _livingEntity.currentHealth = settings.maxHealth;
        _rigidbody.mass = settings.mass;
        _rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
        target = FindObjectOfType<Player>();
    }

    private void OnEnable()
    {
        _livingEntity.onDeath.AddListener(Die);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
        _livingEntity.onDeath.RemoveListener(Die);
    }
    private void FixedUpdate()
    {
        var vel = Vector3.Lerp(_rigidbody.velocity, velocity * _speedReduction, 0.2f);
        vel.y = _rigidbody.velocity.y;
        _rigidbody.velocity = vel;

        var sqrt_mag = Vector3.SqrMagnitude(target.transform.position - transform.position);
        targetInMeleeRange = sqrt_mag < settings.attackRange;
    }

    public void Knockback(Vector3 direction, float power)
    {
        if (!gameObject.activeInHierarchy)
            return;
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
    public void MeleeAttack()
    {
        _meleeAnimation.MeleeAttack();
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

    public void Die(LivingEntity entity)
    {
        if (entity != _livingEntity)
            return;
        StopAllCoroutines();
        Systems.aiManager.enemies.Remove(this);
        Systems.scoreSystem.AddScore(settings.score);
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var _))
        {
            // targetInMeleeRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var _))
        {
            // targetInMeleeRange = false;
        }
    }
}