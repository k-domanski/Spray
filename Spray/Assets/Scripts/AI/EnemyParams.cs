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
    public float wanderTime;

    [Header("Ranged attack")]
    public float shootingRange;
    public float stopAndShootDistance;
    public bool predictPosition;

    [Header("Melee Attack")]
    public float attackRange;
    public float attackAngle;
    public float attackTriggerPosition;
    public Vector2 attackTriggerSize;
    public float attackDamage;

    public float attackStartTime;
    public float attackSwingTime;
    public float attackEndTime;

    [Header("Charge Attack")]
    public float chargeActivationDistance;
    public float chargeLoadingTime;
    public float chargeDistance;
    public float chargeSpeed;
    public float postChargeCooldown;
    public float chargeDamage;


    [Header("Knockback")]
    public float speedRecovery;
}