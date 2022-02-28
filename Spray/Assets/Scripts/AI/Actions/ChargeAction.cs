using UnityEngine;

[CreateAssetMenu(menuName = "AI/Actions/Charge")]
public class ChargeAction : Action
{
    private int defaultLayer = (1 << 6) + (1 << 8);
    public override void ActionStart(Enemy enemy)
    {
        defaultLayer = LayerMask.GetMask("Level", "Entrance");
        enemy.loadCharge = false;
        enemy.charging = false;
        enemy.postCharge = false;
        enemy.chargeTimer = 0f;
    }
    public override void Act(Enemy enemy)
    {
        enemy.chargeTimer -= Time.fixedDeltaTime;

        if (enemy.chargeTimer <= 0f)
        {
            if (enemy.loadCharge)
            {
                //attack
                enemy.charging = true;
                enemy.SetTrigger(true);
                enemy.EnableDamage(true);
                enemy.isCharging = true;
                enemy.destinationPoint = enemy.transform.position + (enemy.transform.forward * enemy.settings.chargeDistance);
                //enemy.destinationPoint = new Vector3(Mathf.Clamp(enemy.destinationPoint.x, -23, 23), enemy.destinationPoint.y, Mathf.Clamp(enemy.destinationPoint.z, -23, 23));
                enemy.loadCharge = false;
            }
            else if (enemy.postCharge || !enemy.charging)
            {
                enemy.postCharge = false;
                enemy.isCharging = false;
                enemy.chargeTimer = enemy.settings.chargeLoadingTime;
                enemy.loadCharge = true;
            }
        }

        if (enemy.loadCharge)
        {
            //var dir = _destinationPoint - enemy.transform.position;
            enemy.transform.LookAt(enemy.target.transform);
        }

        if (enemy.charging)
        {
            var enemyPos = enemy.transform.position;
            //Vector3 dir = enemy.target.transform.position - enemyPos;
            Vector3 dir = enemy.destinationPoint - enemyPos;
            dir.y = 0;
            Debug.DrawRay(enemyPos, dir.normalized * enemy.settings.chargeDistance);
            if (Physics.Raycast(enemyPos, dir.normalized, out var hit, dir.magnitude, defaultLayer, QueryTriggerInteraction.Ignore))
            {
                //if (!hit.collider.gameObject.TryGetComponent<Player>(out _))
                //{
                //probably hit wall so stop before this wall
                bool res = UnityEngine.AI.NavMesh.SamplePosition(hit.point, out var navHit, 10f, UnityEngine.AI.NavMesh.AllAreas);
                if (res)
                {
                    enemy.destinationPoint = new Vector3(navHit.position.x, enemy.transform.position.y, navHit.position.z);
                    //enemy.destinationPoint.y = enemy.transform.position.y;
                }
                //}
            }
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, enemy.destinationPoint, enemy.settings.chargeSpeed * Time.fixedDeltaTime);
            enemy.agent.nextPosition = enemy.transform.position;
            if (Vector3.Distance(enemy.transform.position, enemy.destinationPoint) <= 0.5f)
            {
                enemy.charging = false;
                enemy.SetTrigger(false);
                enemy.EnableDamage(false);
                enemy.postCharge = true;
                enemy.chargeTimer = enemy.settings.postChargeCooldown;
            }
        }

        enemy.SetDestination(enemy.transform.position);
        enemy.velocity = Vector3.MoveTowards(enemy.velocity, Vector3.zero, enemy.settings.acceleration * Time.deltaTime);
        enemy.Move(0f);
    }

    public override void ActionEnd(Enemy enemy)
    {
        enemy.isCharging = false;
        enemy.SetTrigger(false);
        enemy.EnableDamage(false);
    }
}