using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Pursuit")]
public class PursuitAction : Action
{
    private readonly Quaternion _rightRotation = Quaternion.Euler(0f, 45f, 0f);
    private readonly Quaternion _leftRotation = Quaternion.Euler(0f, -45f, 0f);
    public override void Act(Enemy enemy)
    {
        if (enemy.target == null)
            return;

        var targetPosition = enemy.target.transform.position;
        targetPosition.y = 0f;
        var position = enemy.transform.position;
        position.y = 0f;

        var targetDir = targetPosition - position;
        var distance = targetDir.magnitude;
        //var predictionStep = distance / enemy.settings.maxSpeed;
        //Vector3 futurePosition = targetPosition + (enemy.target.velocity * predictionStep);

        //var angle = Vector3.SignedAngle(targetDir, futurePosition, Vector3.up);

        Vector3 desiredVelocity = targetPosition - position;
        //if (Mathf.Abs(angle) > 180f)
          //  desiredVelocity = targetDir;

          //queue avoiding
        var rightIsFree = true;
        var leftIsFree = true;
        Physics.SphereCast(enemy.transform.position, 0.5f, desiredVelocity.normalized, out var hitInfo, 2.0f);
        if (hitInfo.collider != null)
        {
            if (hitInfo.collider.TryGetComponent<Enemy>(out var comp))
            {
                var newDesiredVelocity = _rightRotation * desiredVelocity;
                Physics.SphereCast(enemy.transform.position, 0.5f, newDesiredVelocity.normalized, out var rightHitInfo, 2.0f);
                if (rightHitInfo.collider != null)
                {
                    if (rightHitInfo.collider.TryGetComponent<Enemy>(out var newComp))
                    {
                        Debug.Log(rightHitInfo.transform);
                        rightIsFree = false;
                    }
                }

                newDesiredVelocity = _leftRotation * desiredVelocity;
                Physics.SphereCast(enemy.transform.position, 0.5f, newDesiredVelocity.normalized, out var leftHitInfo, 2.0f);
                if (leftHitInfo.collider != null)
                {
                    if (leftHitInfo.collider.TryGetComponent<Enemy>(out var newComp))
                    {
                        leftIsFree = false;
                    }
                }

                if (rightIsFree)
                    desiredVelocity = _rightRotation * desiredVelocity;
                else if (leftIsFree)
                    desiredVelocity = _leftRotation * desiredVelocity;
            }
        }

        if (distance < enemy.settings.stoppingDistance)
            desiredVelocity = desiredVelocity.normalized * enemy.settings.maxSpeed * (distance / enemy.settings.stoppingDistance);
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
        var neighbour = Systems.aiManager.GetNeighbour(enemy);
        if (neighbour != null)
        {
            breakForce = -steering * 0.8f;
            breakForce -= velocity;

            //if (Vector3.Distance(enemy.transform.position, neighbour.transform.position) <= enemy.settings.maxQueueRadius)
            //  enemy.velocity *= 0.3f;
        }

        return breakForce;
    }
}