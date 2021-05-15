using UnityEngine;

[CreateAssetMenu(menuName = "AI/Decisions/ShootAndGoDecision")]
public class ShootAndGoDecision : Decision
{
    public override bool Decide(Enemy enemy)
    {
        if (Vector3.Distance(enemy.target.transform.position, enemy.transform.position) > enemy.settings.stopAndShootDistance)
        {
            return true;
        }

        return false;
    }
}