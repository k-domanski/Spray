using UnityEngine;

[CreateAssetMenu(fileName = "new enemy params", menuName = "Enemies/Params")]
public class EnemyParams : ScriptableObject
{
    public float maxSpeed;
    public float stoppingDistance;
    public float mass;
    public float maxQueueAhead;
    public float maxQueueRadius;
    public float shootingRange;
    public float maxRotationSpeed;
}