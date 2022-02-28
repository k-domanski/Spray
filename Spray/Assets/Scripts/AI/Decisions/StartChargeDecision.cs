using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/StartChargeDecision")]
public class StartChargeDecision : Decision
{
    private int defaultLayer = (1 << 6) + (1 << 8);
    public override bool Decide(Enemy enemy)
    {
        var enemyPos = enemy.transform.position;
        if (Vector3.Distance(enemy.target.transform.position, enemyPos) > enemy.settings.chargeActivationDistance)
            return false;
        Vector3 dir = enemy.target.transform.position - enemyPos;
        dir.y = 0;
        Debug.DrawRay(enemyPos, dir.normalized * enemy.settings.chargeActivationDistance);
        if (Physics.Raycast(enemyPos, dir.normalized, out var hit, enemy.settings.chargeActivationDistance, defaultLayer, QueryTriggerInteraction.Ignore))
        {
            if (hit.collider.gameObject.TryGetComponent<Player>(out _))
                return true;
            if(hit.collider.GetComponentInParent<Player>() != null)
                return true;
        }
        return false;
    }
}
