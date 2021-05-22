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

    [Header("Close combat attack")] 
    public float attackRange;
    public float attackAngle;
    public float attackTriggerPosition;
    public Vector2 attackTriggerSize;
    public float attackDamage;
    public float attackCooldown;
    public float preAttackTime;

    [Header("Knockback")]
    public float speedRecovery;
}