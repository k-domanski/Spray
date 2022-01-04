using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/ShootingDecision")]
public class ShootDecision : Decision
{
    private int defaultLayer = (1 << 6) + (1 << 8);
    public override bool Decide(Enemy enemy)
    {
        var enemyPos = enemy.transform.position;
        Vector3 dir = enemy.target.transform.position - enemyPos;
        dir.y = 0;
        Debug.DrawRay(enemyPos, dir.normalized * enemy.settings.shootingRange);
        if (Physics.Raycast(enemyPos, dir.normalized, out var hit, enemy.settings.shootingRange, defaultLayer, QueryTriggerInteraction.Ignore))
        {
            return hit.collider.gameObject.TryGetComponent<Player>(out _);
        }
        return false;
    }
}