using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "AI/Actions/Wander")]
public class WanderAction : Action
{
    private Vector3 _point = Vector3.zero;
    public override void Act(Enemy enemy)
    {
        if (enemy.agent.remainingDistance < 1f)
        {
            enemy.wanderTimer -= Time.fixedDeltaTime;//act is in fixed update
            if (enemy.wanderTimer < 0f)
            {

                float length = Random.Range(10, 20);
                //var dir = Random.insideUnitCircle;
                _point.x = Random.Range(-20f, 20f);
                _point.z = Random.Range(-20f, 20f);//random point on map
                var dir = (_point - enemy.transform.position).normalized;

                dir *= length;

                Vector3 randomPoint = new Vector3(dir.x, 0f, dir.z) + enemy.transform.position;
                bool res = NavMesh.SamplePosition(randomPoint, out var hit, 10f, NavMesh.AllAreas);
                if (res)
                {
                    var randomPointInNavMesh = hit.position;
                    enemy.SetDestination(randomPointInNavMesh);
                    enemy.wanderTimer = enemy.settings.wanderTime;
                }
            }
            enemy.Move(0);
            return;
        }

        enemy.Move(enemy.settings.maxSpeed / 2f);
    }

    public override void ActionStart(Enemy enemy)
    {

    }
    public override void ActionEnd(Enemy enemy)
    {

    }
}