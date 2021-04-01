using System;
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
        _rigidbody.velocity = velocity;
    }

    public void Die()
    {
        Systems.aiManager.enemies.Remove(this);
        gameObject.SetActive(false);
    }
}