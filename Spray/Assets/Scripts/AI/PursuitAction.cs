using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Pursuit")]
public class PursuitAction : Action
{
    public override void Act(Enemy enemy)
    {
        if (enemy.target == null)
            return;

        var targetPosition = enemy.target.transform.position;
        var position = enemy.transform.position;

        Vector3 distance = targetPosition - position;
        var predictionStep = distance.magnitude / enemy.settings.maxSpeed;
        Vector3 futurePosition = targetPosition + enemy.target.velocity * predictionStep;

        Vector3 desiredVelocity = targetPosition - position;
        //float distance = desiredVelocity.magnitude;

        if (distance.magnitude < enemy.settings.stoppingDistance)
            desiredVelocity = desiredVelocity.normalized * enemy.settings.maxSpeed * (distance.magnitude / enemy.settings.stoppingDistance);
        else
            desiredVelocity = desiredVelocity.normalized * enemy.settings.maxSpeed;

        Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - enemy.velocity, enemy.settings.maxSpeed);
        steering = steering / enemy.settings.mass;

        enemy.velocity = Vector3.ClampMagnitude(enemy.velocity + steering, enemy.settings.maxSpeed);
    }
}