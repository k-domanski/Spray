using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/ShootingDecision")]
public class ShootDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        if (Vector3.Distance(enemy.target.transform.position, enemy.transform.position) < enemy.settings.shootingRange)
        {
            return true;
        }

        return false;
    }
}