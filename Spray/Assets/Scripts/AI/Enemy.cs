using System;
using UnityEngine;

[RequireComponent(typeof(StateController))]
[RequireComponent(typeof(Rigidbody))]
public class Enemy : MonoBehaviour
{
    public EnemyParams settings;
    public StateController stateController { get; private set; }
    private Rigidbody _rigidbody;
    public Enemy leader { get; set; } = null;
    public Player target { get; set; } = null;
    public Vector3 velocity { get; set; } = Vector3.zero;
    private void Awake()
    {
        stateController = GetComponent<StateController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody.mass = settings.mass;
        target = FindObjectOfType<Player>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = velocity;
    }
}