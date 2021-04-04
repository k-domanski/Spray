using UnityEngine;

[CreateAssetMenu(fileName = "new enemy params", menuName = "Enemies/Params")]
public class EnemyParams : ScriptableObject
{
    public float maxSpeed;
    public float stoppingDistance;
    public float mass;

    [Header("Knockback")]
    public float speedRecovery;
}