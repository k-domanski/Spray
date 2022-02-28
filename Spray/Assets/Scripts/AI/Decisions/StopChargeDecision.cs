using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/StopChargeDecision")]
public class StopChargeDecision : Decision
{
    private int defaultLayer = (1 << 6) + (1 << 8);
    public override bool Decide(Enemy enemy)
    {
        if (enemy.isCharging)
            return false;
        var enemyPos = enemy.transform.position;
        if (Vector3.Distance(enemy.target.transform.position, enemyPos) > enemy.settings.chargeActivationDistance)
            return true;
        Vector3 dir = enemy.target.transform.position - enemyPos;
        dir.y = 0;
        Debug.DrawRay(enemyPos, dir.normalized * enemy.settings.chargeActivationDistance);
        if (Physics.Raycast(enemyPos, dir.normalized, out var hit, enemy.settings.chargeActivationDistance, defaultLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.TryGetComponent<Player>(out _))
                return false;
            if (hit.collider.GetComponentInParent<Player>() != null)
                return false;
        }
        return true;
    }
}
