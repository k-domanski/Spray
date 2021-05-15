using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/ShootingDecision")]
public class ShootDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        return Vector3.Distance(enemy.target.transform.position, enemy.transform.position) < enemy.settings.shootingRange;
    }
}