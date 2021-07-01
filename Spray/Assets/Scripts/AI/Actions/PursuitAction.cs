using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Pursuit")]
public class PursuitAction : Action
{

    private bool _tryAvoidingQueue = false;
    public override void Act(Enemy enemy)
    {
        if (enemy.target == null)
            return;

        var targetPosition = enemy.target.transform.position;
        targetPosition.y = 0f;
        var position = enemy.transform.position;
        position.y = 0f;

        Vector3 distance = targetPosition - position;
        var predictionStep = distance.magnitude / enemy.settings.maxSpeed;
        Vector3 futurePosition = targetPosition + enemy.target.velocity * predictionStep;

        Vector3 desiredVelocity = futurePosition - position;

        if (_tryAvoidingQueue)
        {
            desiredVelocity = Quaternion.Euler(0f, 30f, 0f) * desiredVelocity;
        }

        //float distance = desiredVelocity.magnitude;

        if (distance.magnitude < enemy.settings.stoppingDistance)
            desiredVelocity = desiredVelocity.normalized * enemy.settings.maxSpeed * (distance.magnitude / enemy.settings.stoppingDistance);
        else
            desiredVelocity = desiredVelocity.normalized * enemy.settings.maxSpeed;

        Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - enemy.velocity, enemy.settings.maxSpeed);
        steering += Queue(enemy, steering);
        steering = steering / enemy.settings.mass;


        enemy.velocity = Vector3.ClampMagnitude(enemy.velocity + steering, enemy.settings.maxSpeed);
        if (enemy.velocity.magnitude > 0.3f)
            //enemy.transform.LookAt(enemy.transform.position + enemy.velocity.normalized);
            enemy.transform.rotation = Quaternion.RotateTowards(enemy.transform.rotation, Quaternion.LookRotation(enemy.velocity.normalized, Vector3.up), enemy.settings.maxRotationSpeed);
    }

    private Vector3 Queue(Enemy enemy, Vector3 steering)
    {
        Vector3 breakForce = Vector3.zero;
        var velocity = enemy.velocity;
        _tryAvoidingQueue = false;
        var neighbour = Systems.aiManager.GetNeighbour(enemy);
        if (neighbour != null)
        {
            breakForce = -steering * 0.8f;
            breakForce -= velocity;

            _tryAvoidingQueue = true;
            //if (Vector3.Distance(enemy.transform.position, neighbour.transform.position) <= enemy.settings.maxQueueRadius)
            //  enemy.velocity *= 0.3f;
        }

        return breakForce;
    }
}