using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/OutOfRangeDecision")]
public class OutOfRangeDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        if (Vector3.Distance(enemy.target.transform.position, enemy.transform.position) > enemy.settings.shootingRange)
        {
            return true;
        }

        return false;
    }
}