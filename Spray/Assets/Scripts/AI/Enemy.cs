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
    public GameObject weaponPlaceholder;
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
    public bool canAttack
    {
        get => _canAttack;
        set
        {
            _canAttack = value;
            if (!value)
                this.Delay(() => canAttack = true, settings.attackCooldown);
        }
    }

    private void Awake()
    {
        stateController = GetComponent<StateController>();
        _rigidbody = GetComponent<Rigidbody>();
        gunController = GetComponent<GunController>();
        _livingEntity = GetComponent<LivingEntity>();
        targetInMeleeRange = false;
        canAttack = true;
        if (settings.attackTriggerSize == Vector2.zero) return;

        _attackRangeChecker = gameObject.AddComponent<BoxCollider>();

        _attackRangeChecker.center = settings.attackTriggerPosition * Vector3.forward;
        _attackRangeChecker.size = new Vector3(settings.attackTriggerSize.x, 1f, settings.attackTriggerSize.y);
        _attackRangeChecker.isTrigger = true;
        var scale = weaponPlaceholder.transform.localScale;
        weaponPlaceholder.transform.localScale = new Vector3(scale.x, scale.y, scale.z * settings.attackRange);
        weaponPlaceholder.SetActive(false);
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
        var vel = Vector3.Lerp(_rigidbody.velocity, velocity * _speedReduction, 0.5f);
        vel.y = _rigidbody.velocity.y;
        _rigidbody.velocity = vel;
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

    public void ShowAttack()
    {
        weaponPlaceholder.SetActive(true);
        var rot = weaponPlaceholder.transform.localEulerAngles;
        weaponPlaceholder.transform.localEulerAngles = new Vector3(rot.x, settings.attackAngle / 2f, rot.z);
        this.Delay(() => StartCoroutine(showAttackEnumerator()), settings.preAttackTime);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var _))
        {
            targetInMeleeRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<Player>(out var _))
        {
            targetInMeleeRange = false;
        }
    }

    private IEnumerator showAttackEnumerator()
    {
        var time = settings.attackCooldown - ( settings.preAttackTime);
        time /= 2f;
        var timeCounter = 0f;
        var angle = settings.attackAngle / time;
        //Debug.Log(weaponPlaceholder.transform.localEulerAngles - (180f * Vector3.up));
        Debug.Log(angle);
        while (timeCounter < time)
        //while ((weaponPlaceholder.transform.localEulerAngles.y - 180f) > (-settings.attackAngle))
        {
            Debug.Log(weaponPlaceholder.transform.localEulerAngles - (180f * Vector3.up));
            weaponPlaceholder.transform.Rotate(Vector3.up, -angle * Time.deltaTime);
            timeCounter += Time.deltaTime;
            yield return null;
        }
        weaponPlaceholder.SetActive(false);
    }
}