using UnityEngine;

[CreateAssetMenu(fileName = "new enemy params", menuName = "Enemies/Params")]
public class EnemyParams : ScriptableObject
{
    public float maxHealth;
    public int score;
    [Header("Movement")]
    public float maxSpeed;
    public float stoppingDistance;
    public float mass;
    public float maxQueueAhead;
    public float maxQueueRadius;
    public float maxRotationSpeed;
    public float acceleration;
    [Header("Ranged attack")]
    public float shootingRange;
    public float stopAndShootDistance;
    public bool predictPosition;

    [Header("Knockback")]
    public float speedRecovery;
}