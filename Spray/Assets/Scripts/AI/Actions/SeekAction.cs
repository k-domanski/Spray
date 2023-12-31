using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Seek")]
public class SeekAction : Action
{
    public override void Act(Enemy enemy)
    {
        //var maxSpeed = enemy.settings.maxSpeed / 2f;

        //Vector3 desiredVelocity = enemy.target.transform.position - enemy.transform.position;
        //desiredVelocity.y = 0f;
        //float distance = desiredVelocity.magnitude;

        //if (distance < enemy.settings.stoppingDistance)
        //    desiredVelocity = desiredVelocity.normalized * maxSpeed * ((distance - 1f) / enemy.settings.stoppingDistance);
        //else
        //    desiredVelocity = desiredVelocity.normalized * maxSpeed;

        //Vector3 steering = Vector3.ClampMagnitude(desiredVelocity - enemy.velocity, maxSpeed);
        //steering += Queue(enemy, steering);
        //steering = steering / enemy.settings.mass;

        //enemy.velocity = Vector3.ClampMagnitude(enemy.velocity + steering, maxSpeed);

        enemy.SetDestination(enemy.target.transform.position);
        enemy.Move(enemy.settings.maxSpeed / 2f);
    }

    public override void ActionEnd(Enemy enemy)
    {
        
    }

    public override void ActionStart(Enemy enemy)
    {
        
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