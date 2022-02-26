using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(StateController))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(LivingEntity))]
[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    public EnemyParams settings;
    public GunController gunController { get; private set; }
    public StateController stateController { get; private set; }
    private Rigidbody _rigidbody;
    public Enemy leader { get; set; } = null;
    public Player target { get; set; } = null;
    public Vector3 velocity { get; set; } = Vector3.zero;
    public NavMeshAgent agent { get; private set; }
    public float speed => settings.maxSpeed - _speedReduction;

    private LivingEntity _livingEntity;
    //private NavMeshPath _path;
    private bool _pathExists;

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
    public bool isCharging { get; set; } = false;
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

        //_path = new NavMeshPath();
        _pathExists = false;
        // /* Hands */
        // _qLBase = _leftHand.transform.rotation;
        // _qRBase = _rightHand.transform.rotation;
        // _qLInter = Quaternion.identity;
        // _qRInter = Quaternion.identity;
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updatePosition = false;
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


        //var force = velocity - _rigidbody.velocity;
        //force.y = 0;
        //force *= settings.mass;
        //_rigidbody.AddForce(force, ForceMode.VelocityChange);

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

    public void SetDestination(Vector3 position)
    {
        //_pathExists =  agent.CalculatePath(position, _path);
        agent.SetDestination(position);
    }

    public void Move(float maxStateSpeed)
    {
        if (maxStateSpeed <= 0.01f)
        {
            _rigidbody.velocity = Vector3.zero;
            return;
        }
        agent.nextPosition = transform.position;
        //if (!_pathExists)
        //    return;
        if (agent.path.corners.Length > 1)
        {

            for (int j = 0; j < agent.path.corners.Length - 1; j++)
                Debug.DrawLine(agent.path.corners[j], agent.path.corners[j + 1], Color.red);

            var maxSpeed = maxStateSpeed;
            //int i = 0;
            var targetCorner = agent.path.corners[1];

            //while (Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(targetCorner.x, targetCorner.z)) < 0.5f && i < agent.path.corners.Length)
            //{
            //    targetCorner = agent.path.corners[i++];
            //}

            Vector3 desiredVelocity = targetCorner - transform.position;
            desiredVelocity.y = 0f;
            float distance = Vector3.Distance(transform.position, target.transform.position);

            if (distance < settings.stoppingDistance)
                desiredVelocity = desiredVelocity.normalized * (maxSpeed * ((distance - 1f) / settings.stoppingDistance));
            else
                desiredVelocity = desiredVelocity.normalized * maxSpeed;

            Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - velocity, maxSpeed);
            steering = steering / settings.mass;

            velocity = Vector3.ClampMagnitude(velocity + steering, maxSpeed);
            if (_rigidbody.velocity.magnitude > 0.3f)
                //enemy.transform.LookAt(enemy.transform.position + enemy.velocity.normalized);
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(velocity.normalized, Vector3.up), settings.maxRotationSpeed);
        }


        //var vel = Vector3.Lerp(_rigidbody.velocity, velocity * _speedReduction, 0.2f);
        var vel = Vector3.MoveTowards(_rigidbody.velocity, velocity * _speedReduction, settings.acceleration * Time.fixedDeltaTime);
        vel.y = _rigidbody.velocity.y;
        _rigidbody.velocity = vel;
    }

    public void SetTrigger(bool isTrigger)
    {
        GetComponent<CapsuleCollider>().isTrigger = isTrigger;
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