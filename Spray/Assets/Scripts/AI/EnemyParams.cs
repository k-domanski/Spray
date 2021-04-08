using UnityEngine;

[CreateAssetMenu(fileName = "new enemy params", menuName = "Enemies/Params")]
public class EnemyParams : ScriptableObject
{
    public float maxHealth;
    [Header("Movement")]
    public float maxSpeed;
    public float stoppingDistance;
    public float mass;
    public float maxQueueAhead;
    public float maxQueueRadius;
    public float maxRotationSpeed;
    [Header("Ranged attack")]
    public float shootingRange;

    [Header("Knockback")]
    public float speedRecovery;
}