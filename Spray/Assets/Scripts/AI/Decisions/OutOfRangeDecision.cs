using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/OutOfRangeDecision")]
public class OutOfRangeDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        return Vector3.Distance(enemy.target.transform.position, enemy.transform.position) > enemy.settings.shootingRange;
    }
}