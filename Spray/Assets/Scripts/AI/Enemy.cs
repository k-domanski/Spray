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
        //Debug.Log($"{velocity}");
    }

    public void Knockback(Vector3 direction, float power)
    {
        var amount = (power * settings.maxSpeed);
        //_rigidbody.AddForce(amount * direction, ForceMode.VelocityChange);
        //_rigidbody.AddForce(amount * direction, ForceMode.Impulse);
        _rigidbody.AddForce(amount * -velocity, ForceMode.Impulse);
    }
}